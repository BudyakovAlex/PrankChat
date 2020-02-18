using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using Newtonsoft.Json;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages;
using PrankChat.Mobile.Core.ApplicationServices.Network.JsonSerializers;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Exceptions.Network;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Api;
using RestSharp;

namespace PrankChat.Mobile.Core.ApplicationServices.Network
{
    public class HttpClient
    {
        private const string ApiId = "api";
        private readonly IRestClient _client;
        private readonly IMvxMessenger _messenger;
        private readonly IMvxLog _mvxLog;
        private readonly ISettingsService _settingsService;

        public HttpClient(string baseAddress, Version apiVersion, ISettingsService settingsService, IMvxLog mvxLog, IMvxMessenger messenger)
        {
            _settingsService = settingsService;
            _mvxLog = mvxLog;
            _messenger = messenger;
            _client = new RestClient($"{baseAddress}/{ApiId}/v{apiVersion.Major}").UseSerializer(() => new JsonNetSerializer());
        }

        public async Task<TResult> UnauthorizedGet<TResult>(string endpoint, bool exceptionThrowingEnabled = false, params IncludeType[] includes) where TResult : class, new()
        {
            endpoint = TryAddIncludeFlag(endpoint, includes);
            var request = new RestRequest(endpoint, Method.GET);
            return await ExecuteTask<TResult>(request, endpoint, false, exceptionThrowingEnabled);
        }

        public Task<string> UnauthorizedPost<TEntity>(string endpoint, TEntity item, bool exceptionThrowingEnabled = false) where TEntity : class
        {
            var request = new RestRequest(endpoint, Method.POST);
            request.AddJsonBody(item);
            return ExecuteTask(request, endpoint, false, exceptionThrowingEnabled);
        }

        public async Task<TResult> UnauthorizedPost<TEntity, TResult>(string endpoint, TEntity item, bool exceptionThrowingEnabled = false)
            where TEntity : class
            where TResult : class, new()
        {
            var request = new RestRequest(endpoint, Method.POST);
            request.AddJsonBody(item);
            return await ExecuteTask<TResult>(request, endpoint, false, exceptionThrowingEnabled);
        }

        public async Task<TResult> Get<TResult>(string endpoint, bool exceptionThrowingEnabled = false, params IncludeType[] includes) where TResult : class, new()
        {
            endpoint = TryAddIncludeFlag(endpoint, includes);
            var request = new RestRequest(endpoint, Method.GET);
            return await ExecuteTask<TResult>(request, endpoint, true, exceptionThrowingEnabled);
        }

        public Task<TResult> Post<TEntity, TResult>(string endpoint, TEntity item, bool exceptionThrowingEnabled = false) where TEntity : class where TResult : new()
        {
            var request = new RestRequest(endpoint, Method.POST);
            request.AddJsonBody(item);
            return ExecuteTask<TResult>(request, endpoint, true, exceptionThrowingEnabled);
        }

        public Task<TResult> Post<TResult>(string endpoint, bool exceptionThrowingEnabled = false) where TResult : new()
        {
            var request = new RestRequest(endpoint, Method.POST);
            return ExecuteTask<TResult>(request, endpoint, true, exceptionThrowingEnabled);
        }

        public Task<TResult> PostVideoFile<TEntity, TResult>(string endpoint, TEntity item, bool exceptionThrowingEnabled = false) where TEntity : LoadVideoApiModel where TResult : new()
        {
            var request = new RestRequest(endpoint, Method.POST);
            request.AddParameter("order_id", item.OrderId);
            request.AddParameter("title", item.Title);
            request.AddParameter("description", item.Description);
            request.AddFile("video", item.FilePath);
            request.AlwaysMultipartFormData = true;
            return ExecuteTask<TResult>(request, endpoint, true, exceptionThrowingEnabled);
        }

        public Task<TResult> PostPhotoFile<TResult>(string endpoint, string path, bool exceptionThrowingEnabled = false) where TResult : new()
        {
            var request = new RestRequest(endpoint, Method.POST);
            request.AddFile("avatar", path);
            request.AlwaysMultipartFormData = true;
            return ExecuteTask<TResult>(request, endpoint, true, exceptionThrowingEnabled);
        }

        public async Task Delete<TEntity>(string endpoint, TEntity item, bool exceptionThrowingEnabled = false) where TEntity : class
        {
            var request = new RestRequest(endpoint, Method.DELETE);
            request.AddJsonBody(item);
            await ExecuteTask(request, endpoint, true, exceptionThrowingEnabled);
        }

        public async Task Put<TEntity>(string endpoint, TEntity item, bool exceptionThrowingEnabled = false) where TEntity : class
        {
            var request = new RestRequest(endpoint, Method.PUT);
            request.AddJsonBody(item);
            await ExecuteTask(request, endpoint, true, exceptionThrowingEnabled);
        }

        private async Task<string> ExecuteTask(IRestRequest request, string endpoint, bool includeAccessToken, bool exceptionThrowingEnabled = false, CancellationToken? cancellationToken = null)
        {
            try
            {
                _mvxLog.Debug($"[HTTP] {request.Method} {endpoint}");
                if (includeAccessToken)
                    await AddAuthorizationHeader(request);

                var content = cancellationToken.HasValue
                    ? await _client.ExecuteAsync<string>(request, cancellationToken.Value)
                    : await _client.ExecuteAsync<string>(request);

                CheckResponse(request, content, exceptionThrowingEnabled);
                return content.Content;
            }
            catch (AuthenticationProblemDetails)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new NetworkException(e.Message, e);
            }
        }

        private async Task<T> ExecuteTask<T>(IRestRequest request, string endpoint, bool includeAccessToken, bool exceptionThrowingEnabled = false, CancellationToken? cancellationToken = null) where T : new()
        {
            try
            {
                _mvxLog.Debug($"[HTTP] {request.Method} {endpoint}");
                if (includeAccessToken)
                    await AddAuthorizationHeader(request);

                var content = cancellationToken.HasValue
                    ? await _client.ExecuteAsync<T>(request, cancellationToken.Value)
                    : await _client.ExecuteAsync<T>(request);

                CheckResponse(request, content, exceptionThrowingEnabled);
                return content.Data;
            }
            catch (AuthenticationProblemDetails)
            {
                throw;
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

                switch (response.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                    case HttpStatusCode.Forbidden:
                        throw JsonConvert.DeserializeObject<AuthenticationProblemDetails>(response.Content);

                    case HttpStatusCode.InternalServerError:
                        var problemDetails = JsonConvert.DeserializeObject<ProblemDetailsApiModel>(response.Content);
                        throw new InternalServerProblemDetails(string.Join(Environment.NewLine, new[] { problemDetails.Title }.Concat(problemDetails.InvalidParams?.Select(x => x.ToString()))));
                }

                if (response.ErrorException != null)
                {
                    _mvxLog.ErrorException(response.ErrorMessage, response?.ErrorException);
                }
                else
                {
                    throw new NetworkException($"Network error - {response.ErrorMessage} with code {response.StatusCode} for request {request.Resource}");
                }
            }
            catch (Exception ex)
            {
                _mvxLog.ErrorException(response.StatusCode + response.ErrorMessage, ex);
                var errorMessages = new List<string>
                {
                    ex.Message
                };
                _messenger.Publish(new BadRequestErrorMessage(this, errorMessages.AsReadOnly()));

                if (exceptionThrowingEnabled)
                {
                    throw ex;
                }
            }
        }

        private async Task AddAuthorizationHeader(IRestRequest request)
        {
            var accessToken = await _settingsService.GetAccessTokenAsync();
            request.AddHeader(HttpRequestHeader.Authorization.ToString(), $"Bearer {accessToken}");
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
