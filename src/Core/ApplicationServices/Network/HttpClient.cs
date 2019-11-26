using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Logging;
using Newtonsoft.Json;
using PrankChat.Mobile.Core.ApplicationServices.Network.Errors;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using RestSharp;

namespace PrankChat.Mobile.Core.ApplicationServices.Network
{
    public class HttpClient
    {
        private const string ApiId = "api";
        private readonly RestClient _client;
        private readonly IMvxLog _mvxLog;
        private readonly ISettingsService _settingsService;

        public HttpClient(string baseAddress, Version apiVersion, ISettingsService settingsService, IMvxLog mvxLog)
        {
            _settingsService = settingsService;
            _mvxLog = mvxLog;
            _client = new RestClient($"{baseAddress}/{ApiId}/v{apiVersion.Major}");
        }

        public async Task<TResult> UnauthorizedGet<TResult>(string endpoint) where TResult : class, new()
        {
            var request = new RestRequest(endpoint, Method.GET);
            return await ExecuteTask<TResult>(request, false);
        }

        public async Task UnauthorizedPost<TEntity>(string endpoint, TEntity item) where TEntity : class
        {
            var request = new RestRequest(endpoint, Method.POST);
            request.AddJsonBody(item);
            await ExecuteTask(request, false);
        }

        public async Task<TResult> Get<TResult>(string endpoint) where TResult : class, new()
        {
            var request = new RestRequest(endpoint, Method.GET);
            return await ExecuteTask<TResult>(request, true);
        }

        public async Task Post<TEntity>(string endpoint, TEntity item) where TEntity : class
        {
            var request = new RestRequest(endpoint, Method.POST);
            request.AddJsonBody(item);
            await ExecuteTask(request, true);
        }

        public async Task Delete<TEntity>(string endpoint, TEntity item) where TEntity : class
        {
            var request = new RestRequest(endpoint, Method.DELETE);
            request.AddJsonBody(item);
            await ExecuteTask(request, true);
        }

        public async Task Put<TEntity>(string endpoint, TEntity item) where TEntity : class
        {
            var request = new RestRequest(endpoint, Method.PUT);
            request.AddJsonBody(item);
            await ExecuteTask(request, true);
        }

        private async Task ExecuteTask(IRestRequest request, bool includeAccessToken, CancellationToken? cancellationToken = null)
        {
            try
            {
                _mvxLog.Debug($"[HTTP] {request.Method} {request.Resource}");
                if (includeAccessToken)
                {
                    request.AddHeader(HttpRequestHeader.Authorization.ToString(), $"Bearer {await _settingsService.GetAccessTokenAsync()}");
                }

                var content = cancellationToken.HasValue ? await _client.ExecuteTaskAsync(request, cancellationToken.Value) : await _client.ExecuteTaskAsync(request);
                CheckResponse(request, content);
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

        private async Task<T> ExecuteTask<T>(IRestRequest request, bool includeAccessToken, CancellationToken? cancellationToken = null) where T : new()
        {
            try
            {
                _mvxLog.Debug($"[HTTP] {request.Method} {request.Resource}");
                if (includeAccessToken)
                {
                    var accessToken = await _settingsService.GetAccessTokenAsync();
                    request.AddHeader(HttpRequestHeader.Authorization.ToString(), $"Bearer {accessToken}");
                }

                var content = cancellationToken.HasValue ? await _client.ExecuteTaskAsync<T>(request, cancellationToken.Value) : await _client.ExecuteTaskAsync<T>(request);
                CheckResponse(request, content);
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

        private void CheckResponse(IRestRequest request, IRestResponse response)
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
                _mvxLog.Error(response.StatusCode + response.ErrorMessage);
                throw new NetworkException($"Network error - {response.ErrorMessage} with code {response.StatusCode} for request {request.Resource}");
            }
        }
    }
}
