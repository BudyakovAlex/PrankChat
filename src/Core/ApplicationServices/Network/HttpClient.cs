﻿using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using Newtonsoft.Json;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages;
using PrankChat.Mobile.Core.ApplicationServices.Network.Builders;
using PrankChat.Mobile.Core.ApplicationServices.Network.JsonSerializers;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Configuration;
using PrankChat.Mobile.Core.Exceptions;
using PrankChat.Mobile.Core.Exceptions.Network;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using RestSharp;
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.ApplicationServices.Network
{
    public class HttpClient
    {
        private const string ApiId = "api";

        private readonly IRestClient _client;
        private readonly IMvxMessenger _messenger;
        private readonly IDialogService _dialogService;
        private readonly IMvxLog _mvxLog;

        private readonly string _baseAddress;
        private readonly Version _apiVersion;
        private readonly ISettingsService _settingsService;

        public HttpClient(string baseAddress,
                          Version apiVersion,
                          ISettingsService settingsService,
                          IMvxLog mvxLog,
                          IMvxMessenger messenger,
                          IDialogService dialogService)
        {
            _baseAddress = baseAddress;
            _apiVersion = apiVersion;
            _settingsService = settingsService;
            _mvxLog = mvxLog;
            _messenger = messenger;
            _dialogService = dialogService;
            _client = new RestClient($"{baseAddress}/{ApiId}/v{apiVersion.Major}").UseSerializer(() => new JsonNetSerializer());
            _client.Timeout = TimeSpan.FromMinutes(15).Milliseconds;
        }

        public async Task<TResult> UnauthorizedGetAsync<TResult>(string endpoint, bool exceptionThrowingEnabled = false, params IncludeType[] includes) where TResult : class, new()
        {
            endpoint = TryAddIncludeFlag(endpoint, includes);
            var request = new RestRequest(endpoint, Method.GET);
            return await ExecuteTaskAsync<TResult>(request, endpoint, false, exceptionThrowingEnabled);
        }

        public async Task<TResult> UnauthorizedPostAsync<TEntity, TResult>(string endpoint, TEntity item, bool exceptionThrowingEnabled = false)
            where TEntity : class
            where TResult : class, new()
        {
            var request = new RestRequest(endpoint, Method.POST);
            request.AddJsonBody(item);
            return await ExecuteTaskAsync<TResult>(request, endpoint, false, exceptionThrowingEnabled);
        }

        public async Task<TResult> GetAsync<TResult>(string endpoint, bool exceptionThrowingEnabled = false, params IncludeType[] includes) where TResult : class, new()
        {
            endpoint = TryAddIncludeFlag(endpoint, includes);
            var request = new RestRequest(endpoint, Method.GET);
            return await ExecuteTaskAsync<TResult>(request, endpoint, true, exceptionThrowingEnabled);
        }

        public Task<TResult> PostAsync<TEntity, TResult>(string endpoint, TEntity item, bool exceptionThrowingEnabled = false, CancellationToken? cancellationToken = null) where TEntity : class where TResult : new()
        {
            var request = new RestRequest(endpoint, Method.POST);
            request.AddJsonBody(item);
            return ExecuteTaskAsync<TResult>(request, endpoint, true, exceptionThrowingEnabled, cancellationToken);
        }

        public Task PostAsync<TEntity>(string endpoint, TEntity item, bool exceptionThrowingEnabled = false, CancellationToken? cancellationToken = null) where TEntity : class
        {
            var request = new RestRequest(endpoint, Method.POST);
            request.AddJsonBody(item);
            return ExecuteTaskAsync(request, endpoint, true, exceptionThrowingEnabled, cancellationToken);
        }

        public Task<TResult> PostAsync<TResult>(string endpoint, bool exceptionThrowingEnabled = false, CancellationToken? cancellationToken = null) where TResult : new()
        {
            var request = new RestRequest(endpoint, Method.POST);
            return ExecuteTaskAsync<TResult>(request, endpoint, true, exceptionThrowingEnabled, cancellationToken);
        }

        public async Task<TResult> PostVideoFileAsync<TEntity, TResult>(string endpoint, TEntity item, bool exceptionThrowingEnabled = false, Action<double, double> onChangedProgressAction = null, CancellationToken cancellationToken = default) where TEntity : LoadVideoApiModel where TResult : new()
        {
            var response = default(HttpResponseMessage);
            try
            {
                using (var client = new System.Net.Http.HttpClient())
                {
                    var accessToken = await _settingsService.GetAccessTokenAsync();
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                    var currentCulture = CultureInfo.CurrentCulture;
                    client.DefaultRequestHeaders.Add("Accept-Language", currentCulture.TwoLetterISOLanguageName);

                    var buffer = default(byte[]);
                    using (var fileStream = File.OpenRead(item.FilePath))
                    {
                        buffer = new byte[fileStream.Length];
                        await fileStream.ReadAsync(buffer, 0, (int)fileStream.Length);
                    }

                    var multipartData = FormDataBuilder.Create()
                                                       .AttachStringContent("order_id", item.OrderId.ToString())
                                                       .AttachStringContent("title", item.Title)
                                                       .AttachStringContent("description", item.Description)
                                                       .AttachFileContent("video", Path.GetFileName(item.FilePath), buffer, onChangedProgressAction, cancellationToken)
                                                       .Build();

                    var url = new Uri($"{_baseAddress}/{ApiId}/v{_apiVersion.Major}/{endpoint}");
                    response = await client.PostAsync(url, multipartData, cancellationToken);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        return JsonConvert.DeserializeObject<TResult>(responseJson);
                    }

                    var errorContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var problemDetails = JsonConvert.DeserializeObject<ProblemDetailsApiModel>(errorContent);
                    var problemDetailsData = MappingConfig.Mapper.Map<ProblemDetailsDataModel>(problemDetails);
                    throw problemDetailsData;
                }
            }
            catch (JsonSerializationException)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new NetworkException($"Network error - {errorContent} with code {response.StatusCode} for request {response.RequestMessage.RequestUri}");
            }
            catch (Exception ex) when (ex.Message.Contains("Socket closed") || ex.Message.Contains("Failed to connect"))
            {
                //TODO: add no internet connection message
                var error = new ProblemDetailsDataModel(Resources.Error_Unexpected_Network)
                {
                    MessageServerError = Resources.Error_Unexpected_Network
                };

                var problemException = new ServerErrorMessage(this, error);
                _mvxLog.ErrorException(Resources.Error_Unexpected_Network, error);
                _messenger.Publish(problemException);

                if (exceptionThrowingEnabled)
                {
                    throw;
                }

                return default;
            }
            catch (Exception ex)
            {
                if (exceptionThrowingEnabled)
                {
                    throw new NetworkException(ex.Message, ex);
                }

                return default;
            }
        }

        public Task<TResult> PostPhotoFile<TResult>(string endpoint, string path, string propertyName, bool exceptionThrowingEnabled = false) where TResult : new()
        {
            if (!Connectivity.NetworkAccess.HasConnection())
            {
                _dialogService.ShowToast(Resources.No_Intentet_Connection, ToastType.Negative);
                return default;
            }

            var request = new RestRequest(endpoint, Method.POST);
            request.AddFile(propertyName, path);
            request.AlwaysMultipartFormData = true;
            return ExecuteTaskAsync<TResult>(request, endpoint, true, exceptionThrowingEnabled);
        }

        public Task DeleteAsync(string endpoint, bool exceptionThrowingEnabled = false, CancellationToken? cancellationToken = null)
        {
            if (!Connectivity.NetworkAccess.HasConnection())
            {
                _dialogService.ShowToast(Resources.No_Intentet_Connection, ToastType.Negative);
                return Task.CompletedTask;
            }

            var request = new RestRequest(endpoint, Method.DELETE);
            return ExecuteTaskAsync(request, endpoint, true, exceptionThrowingEnabled, cancellationToken);
        }

        private async Task ExecuteTaskAsync(IRestRequest request, string endpoint, bool includeAccessToken, bool exceptionThrowingEnabled = false, CancellationToken? cancellationToken = null)
        {
            try
            {
                if (!Connectivity.NetworkAccess.HasConnection())
                {
                    _dialogService.ShowToast(Resources.No_Intentet_Connection, ToastType.Negative);
                    return;
                }

                _mvxLog.Debug($"[HTTP] {request.Method} {endpoint}");
                if (includeAccessToken)
                {
                    await AddAuthorizationHeaderAsync(request);
                }

                AddLanguageHeader(request);

                var content = cancellationToken.HasValue
                    ? await _client.ExecuteAsync(request, request.Method, cancellationToken.Value)
                    : await _client.ExecuteAsync(request, request.Method);

                CheckResponse(request, content, exceptionThrowingEnabled);
            }
            catch (Exception e)
            {
                throw new NetworkException(e.Message, e);
            }
        }

        private async Task<T> ExecuteTaskAsync<T>(IRestRequest request, string endpoint, bool includeAccessToken, bool exceptionThrowingEnabled = false, CancellationToken? cancellationToken = null) where T : new()
        {
            try
            {
                if (!Connectivity.NetworkAccess.HasConnection())
                {
                    _dialogService.ShowToast(Resources.No_Intentet_Connection, ToastType.Negative);
                    return default;
                }

                _mvxLog.Debug($"[HTTP] {request.Method} {endpoint}");
                if (includeAccessToken)
                {
                    await AddAuthorizationHeaderAsync(request);
                }

                AddLanguageHeader(request);

                var content = cancellationToken.HasValue
                    ? await _client.ExecuteAsync<T>(request, cancellationToken.Value)
                    : await _client.ExecuteAsync<T>(request);

                CheckResponse(request, content, exceptionThrowingEnabled);
                return content.Data;
            }
            catch (Exception e)
            {
                throw new NetworkException(e.Message, e);
            }
        }

        private void CheckResponse(IRestRequest request, IRestResponse response, bool exceptionThrowingEnabled = false)
        {
            try
            {
                if (response.IsSuccessful)
                {
                    return;
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _messenger.Publish(new UnauthorizedMessage(this));
                    return;
                }

                try
                {
                    var problemDetails = JsonConvert.DeserializeObject<ProblemDetailsApiModel>(response.Content);
                    var problemDetailsData = MappingConfig.Mapper.Map<ProblemDetailsDataModel>(problemDetails);
                    throw problemDetailsData;
                }
                catch (JsonSerializationException)
                {
                    throw new NetworkException($"Network error - {response.ErrorMessage} with code {response.StatusCode} for request {request.Resource}");
                }
            }
            catch (Exception ex)
            {
                _mvxLog.ErrorException(response.StatusCode + response.ErrorMessage, ex);
                _messenger.Publish(new ServerErrorMessage(this, ex));

                if (exceptionThrowingEnabled)
                {
                    throw;
                }
            }
        }

        private async Task AddAuthorizationHeaderAsync(IRestRequest request)
        {
            var accessToken = await _settingsService.GetAccessTokenAsync();
            request.AddHeader(HttpRequestHeader.Authorization.ToString(), $"Bearer {accessToken}");
        }

        private void AddLanguageHeader(IRestRequest request)
        {
            var currentCulture = CultureInfo.CurrentCulture;
            request.AddHeader("Accept-Language", currentCulture.TwoLetterISOLanguageName);
        }

        private string TryAddIncludeFlag(string apiPoint, IncludeType[] includes)
        {
            // TODO: Refactor this.
            if (includes == null || includes.Length == 0)
                return apiPoint;

            var startChar = apiPoint.Contains("?") ? "&" : "?";
            var values = includes.GetEnumMembersAttrValues();
            return $"{apiPoint}{startChar}include={string.Join(",", values)}".ToLowerInvariant();
        }
    }
}
