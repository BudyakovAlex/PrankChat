using System;
using System.Globalization;
using System.Threading.Tasks;
using Android.OS;
using Java.Net;
using Java.Util.Concurrent;
using Javax.Net.Ssl;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Providers.Configuration;
using PrankChat.Mobile.Core.Providers.UserSession;
using PrankChat.Mobile.Core.Services.Network;
using PrankChat.Mobile.Droid.Plugins.HttpClient.Builders;
using PrankChat.Mobile.Droid.Plugins.HttpClient.HostnameVerfier;
using PrankChat.Mobile.Droid.Plugins.HttpClient.Requests;
using PrankChat.Mobile.Droid.Plugins.HttpClient.TrustManagers;
using Square.OkHttp3;

namespace PrankChat.Mobile.Droid.Plugins.HttpClient
{
    public class PlatformHttpClient : IPlatformHttpClient
    {
        private const int ConnectTimeOutHttpClientInMinutes = 5;
        private const string SslKey = "SSL";

        private readonly IEnvironmentConfigurationProvider _environmentConfigurationProvider;
        private readonly IUserSessionProvider _userSessionProvider;

        public PlatformHttpClient(
            IEnvironmentConfigurationProvider environmentConfigurationProvider,
            IUserSessionProvider userSessionProvider)
        {
            _environmentConfigurationProvider = environmentConfigurationProvider;
            _userSessionProvider = userSessionProvider;
        }

        public async Task<string> UploadVideoAsync(UploadVideoDto uploadVideoDto, Action<double, double> onChangedProgressAction = null)
        {
            var strictMode = new StrictMode.ThreadPolicy.Builder()
                .PermitAll()
                .Build();
            StrictMode.SetThreadPolicy(strictMode);
            var environment = _environmentConfigurationProvider.Environment;
            var version = new Version(environment.ApiVersion);
            var currentCulture = CultureInfo.CurrentCulture;
            var accessToken = await _userSessionProvider.GetAccessTokenAsync().ConfigureAwait(false);
            var contentType = string.Format(RestConstants.ContentTypeCookieValueTemplate, RestConstants.DefaultBoundary);

            var url = new URL(string.Format(RestConstants.UrlStringTemplate, environment.ApiUrl, version.Major));
            var httpClient = CreateUnsafeHttpClient();
            var requestBuilder = new Request.Builder().Url(url);

            var taskCompletionSource = new TaskCompletionSource<string>();

            // TODO: Refactoring this code.
            var formDataBuilder = FormDataBuilder.Create()
                .AttachStringContent("order_id", uploadVideoDto.OrderId.ToString())
                .AttachStringContent("title", uploadVideoDto.Title)
                .AttachStringContent("description", uploadVideoDto.Description)
                .AttachFileContent("video", uploadVideoDto.FilePath);

            var request = requestBuilder
                .Post(new VideoRequestBody(onChangedProgressAction, formDataBuilder, contentType))
                .AddHeader(RestConstants.AuthorizationCookieKey, string.Format(RestConstants.AuthorizationCookieValueTemplate, accessToken))
                .AddHeader(RestConstants.AcceptLanguageCookieKey, currentCulture.TwoLetterISOLanguageName)
                .AddHeader(RestConstants.ContentTypeKey, contentType)
                .Build();

            var call = httpClient.NewCall(request);
            call.Enqueue(
                (call, response) => _ = OnResponseAsync(call, response, taskCompletionSource),
                (call, exception) => OnResponseFailure(call, exception, taskCompletionSource));

            return await taskCompletionSource.Task;
        }

        private async Task OnResponseAsync(ICall call, Response response, TaskCompletionSource<string> taskCompletionSource)
        {
            var body = await response.Body().StringAsync();
            taskCompletionSource.TrySetResult(body);
        }

        private void OnResponseFailure(ICall call, Exception exception, TaskCompletionSource<string> taskCompletionSource)
        {
            taskCompletionSource.TrySetException(exception);
        }

        private OkHttpClient CreateUnsafeHttpClient()
        {
            var trustManagers = new[] { new UnsafeX509ExtendedTrustManager() };
            var sslContext = SSLContext.GetInstance(SslKey);
            sslContext.Init(null, trustManagers, new Java.Security.SecureRandom());
            var sslSocketFactory = sslContext.SocketFactory;

            var httpClient = new OkHttpClient.Builder()
                .SslSocketFactory(sslSocketFactory, trustManagers[0])
                .HostnameVerifier(new UnsafeHostnameVerifier())
                .ConnectTimeout(ConnectTimeOutHttpClientInMinutes, TimeUnit.Minutes)
                .ReadTimeout(ConnectTimeOutHttpClientInMinutes, TimeUnit.Minutes)
                .WriteTimeout(ConnectTimeOutHttpClientInMinutes, TimeUnit.Minutes)
                .Build();
            return httpClient;
        }
    }
}
