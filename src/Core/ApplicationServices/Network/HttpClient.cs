using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using Newtonsoft.Json;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages;
using PrankChat.Mobile.Core.ApplicationServices.Network.Errors;
using PrankChat.Mobile.Core.ApplicationServices.Network.JsonSerializers;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
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

        public async Task<TResult> UnauthorizedGet<TResult>(string endpoint, bool exceptionThrowingEnabled = false) where TResult : class, new()
        {
            var request = new RestRequest(endpoint, Method.GET);
            return await ExecuteTask<TResult>(request, false, exceptionThrowingEnabled);
        }

        public Task<string> UnauthorizedPost<TEntity>(string endpoint, TEntity item, bool exceptionThrowingEnabled = false) where TEntity : class
        {
            var request = new RestRequest(endpoint, Method.POST);
            request.AddJsonBody(item);
            return ExecuteTask(request, false, exceptionThrowingEnabled);
        }

        public async Task<TResult> UnauthorizedPost<TEntity, TResult>(string endpoint, TEntity item, bool exceptionThrowingEnabled = false)
            where TEntity : class
            where TResult : class, new()
        {
            var request = new RestRequest(endpoint, Method.POST);
            request.AddJsonBody(item);
            return await ExecuteTask<TResult>(request, false, exceptionThrowingEnabled);
        }

        public async Task<TResult> Get<TResult>(string endpoint, bool exceptionThrowingEnabled = false) where TResult : class, new()
        {
            var request = new RestRequest(endpoint, Method.GET);
            return await ExecuteTask<TResult>(request, true, exceptionThrowingEnabled);
        }

        public Task<TResult> Post<TEntity, TResult>(string endpoint, TEntity item, bool exceptionThrowingEnabled = false) where TEntity : class where TResult : new()
        {
            var request = new RestRequest(endpoint, Method.POST);
            request.AddJsonBody(item);
            return ExecuteTask<TResult>(request, true, exceptionThrowingEnabled);
        }

        public async Task Delete<TEntity>(string endpoint, TEntity item, bool exceptionThrowingEnabled = false) where TEntity : class
        {
            var request = new RestRequest(endpoint, Method.DELETE);
            request.AddJsonBody(item);
            await ExecuteTask(request, true, exceptionThrowingEnabled);
        }

        public async Task Put<TEntity>(string endpoint, TEntity item, bool exceptionThrowingEnabled = false) where TEntity : class
        {
            var request = new RestRequest(endpoint, Method.PUT);
            request.AddJsonBody(item);
            await ExecuteTask(request, true, exceptionThrowingEnabled);
        }

        private async Task<string> ExecuteTask(IRestRequest request, bool includeAccessToken, bool exceptionThrowingEnabled = false, CancellationToken? cancellationToken = null)
        {
            try
            {
                _mvxLog.Debug($"[HTTP] {request.Method} {request.Resource}");
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

        private async Task<T> ExecuteTask<T>(IRestRequest request, bool includeAccessToken, bool exceptionThrowingEnabled = false, CancellationToken? cancellationToken = null) where T : new()
        {
            try
            {
                _mvxLog.Debug($"[HTTP] {request.Method} {request.Resource}");
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
            catch
            {
                _mvxLog.Error(response.StatusCode + response.ErrorMessage);
                _messenger.Publish(new BadRequestErrorMessage(this));

                if (exceptionThrowingEnabled)
                {
                    throw;
                }
            }
        }

        private async Task AddAuthorizationHeader(IRestRequest request)
        {
            var accessToken = await _settingsService.GetAccessTokenAsync();
            request.AddHeader(HttpRequestHeader.Authorization.ToString(), $"Bearer {accessToken}");
        }
    }
}
