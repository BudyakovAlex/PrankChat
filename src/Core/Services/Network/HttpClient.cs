using MvvmCross;
using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using Newtonsoft.Json;
using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Data.Enums;
using PrankChat.Mobile.Core.Exceptions;
using PrankChat.Mobile.Core.Exceptions.Network;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Mappers;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Plugins;
using PrankChat.Mobile.Core.Providers.UserSession;
using PrankChat.Mobile.Core.Services.Dialogs;
using PrankChat.Mobile.Core.Services.ErrorHandling.Messages;
using PrankChat.Mobile.Core.Services.Network.Http.Authorization;
using PrankChat.Mobile.Core.Services.Network.JsonSerializers;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Services.Network
{
    public class HttpClient
    {
        private const string ApiId = "api";

        private readonly IRestClient _client;
        private readonly IMvxMessenger _messenger;

        private readonly IMvxLog _mvxLog;
        private readonly IUserSessionProvider _userSessionProvider;

        private readonly Version _apiVersion;
        private readonly string _baseAddress;

        private IUserInteraction _userInteraction;

        public HttpClient(string baseAddress,
                          string apiVersion,
                          IUserSessionProvider userSessionProvider,
                          IMvxLog mvxLog,
                          IMvxMessenger messenger)
        {
            _baseAddress = baseAddress;
            _apiVersion = new Version(apiVersion);
            _userSessionProvider = userSessionProvider;
            _mvxLog = mvxLog;
            _messenger = messenger;

            _client = new RestClient($"{baseAddress}/{ApiId}/v{_apiVersion.Major}").UseSerializer(() => new JsonNetSerializer());
            _client.Timeout = TimeSpan.FromMinutes(15).Milliseconds;
        }

        private IUserInteraction UserInteraction => _userInteraction ?? (_userInteraction = Mvx.IoCProvider.Resolve<IUserInteraction>());

        public async Task<IRestResponse> ExecuteRawAsync(string endpoint, Method method, bool includeAccessToken)
        {
            if (!Connectivity.NetworkAccess.HasConnection())
            {
                UserInteraction.ShowToast(Resources.No_Intentet_Connection, ToastType.Negative);
                return null;
            }

            var request = new RestRequest(endpoint, method);

            if (includeAccessToken)
            {
                await AddAuthorizationHeaderAsync(request);
            }

            AddLanguageHeader(request);

            var response = await _client.ExecuteAsync(request);
            return response;
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

        public async Task<bool> PostFileAsync(string endpoint, string propertyName, string filePath, bool exceptionThrowingEnabled = false, params KeyValuePair<string, string>[] parameters)
        {
            if (!Connectivity.NetworkAccess.HasConnection())
            {
                return false;
            }

            var response = default(HttpResponseMessage);
            try
            {
                using (var client = new System.Net.Http.HttpClient() { Timeout = TimeSpan.FromMinutes(15) })
                {
                    var accessToken = await _userSessionProvider.GetAccessTokenAsync();
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                    var currentCulture = CultureInfo.CurrentCulture;
                    client.DefaultRequestHeaders.Add("Accept-Language", currentCulture.TwoLetterISOLanguageName);

                    var buffer = default(byte[]);
                    using (var fileStream = File.OpenRead(filePath))
                    {
                        buffer = new byte[fileStream.Length];
                        await fileStream.ReadAsync(buffer, 0, (int)fileStream.Length);
                    }

                    var formDataBuilder = FormDataBuilder.Create()
                                                         .AttachFileContent(propertyName, Path.GetFileName(filePath), buffer, (i, e) => { });
                    var paramsToLog = new List<Parameter>();
                    foreach (var parameter in parameters)
                    {
                        paramsToLog.Add(new Parameter(parameter.Key, parameter.Value, ParameterType.RequestBody));
                        formDataBuilder.AttachStringContent(parameter.Key, parameter.Value);
                    }

                    var multipartData = formDataBuilder.Build();
                    var url = new Uri($"{_baseAddress}/{ApiId}/v{_apiVersion.Major}/{endpoint}");

                    var headers = multipartData.Headers.Select(header => new Parameter(header.Key, header.Value, ParameterType.HttpHeader)).ToList();
                    var requestParameters = paramsToLog.Union(headers).ToList();

                    response = await client.PostAsync(url, multipartData);
                    return response.IsSuccessStatusCode;
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
                var error = new ProblemDetailsException(Resources.Error_Unexpected_Network)
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

        //TODO: refactor it
        public async Task<TResult> PostVideoFileAsync<TEntity, TResult>(
            string endpoint,
            TEntity item,
            bool exceptionThrowingEnabled = false,
            Action<double, double> onChangedProgressAction = null,
            CancellationToken cancellationToken = default) where TEntity : UploadVideoDto where TResult : new()
        {
            var response = default(HttpResponseMessage);
            try
            {
                using (var client = new System.Net.Http.HttpClient() { Timeout = TimeSpan.FromMinutes(15) })
                {
                    var accessToken = await _userSessionProvider.GetAccessTokenAsync();
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

                    var parameters = new[]
                    {
                        new Parameter("order_id", item.OrderId.ToString(), ParameterType.RequestBody),
                        new Parameter("title", item.Title, ParameterType.RequestBody),
                        new Parameter("description", item.Description, ParameterType.RequestBody),
                        new Parameter("video", item.FilePath, ParameterType.RequestBody)
                    };

                    var headers = multipartData.Headers.Select(header => new Parameter(header.Key, header.Value, ParameterType.HttpHeader)).ToList();
                    var requestParameters = parameters.Union(headers).ToList();

                    response = await client.PostAsync(url, multipartData, cancellationToken);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        return JsonConvert.DeserializeObject<TResult>(responseJson);
                    }

                    var errorContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    var problemDetails = JsonConvert.DeserializeObject<ProblemDetailsDto>(errorContent);
                    var problemDetailsData = problemDetails.Map(); //  MappingConfig.Mapper.Map<ProblemDetailsDataModel>(problemDetails);
                    throw problemDetailsData;
                }
            }
            catch (JsonSerializationException)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new NetworkException($"Network error - {errorContent} with code {response.StatusCode} for request {response.RequestMessage.RequestUri}");
            }
            catch (Exception ex) when (!string.IsNullOrEmpty(ex.Message) && (ex.Message.Contains("Socket closed") || ex.Message.Contains("Failed to connect")))
            {
                //TODO: add no internet connection message
                var error = new ProblemDetailsException(Resources.Error_Unexpected_Network)
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
                UserInteraction.ShowToast(Resources.No_Intentet_Connection, ToastType.Negative);
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
                UserInteraction.ShowToast(Resources.No_Intentet_Connection, ToastType.Negative);
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
                    UserInteraction.ShowToast(Resources.No_Intentet_Connection, ToastType.Negative);
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
                    UserInteraction.ShowToast(Resources.No_Intentet_Connection, ToastType.Negative);
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
                    var problemDetails = JsonConvert.DeserializeObject<ProblemDetailsDto>(response.Content);
                   // var problemDetailsData = problemDetails.Map(); // MappingConfig.Mapper.Map<ProblemDetailsDataModel>(problemDetails);
                    throw problemDetails.Map();
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
            var accessToken = await _userSessionProvider.GetAccessTokenAsync();
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