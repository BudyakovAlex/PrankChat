using System;
using System.Globalization;
using System.Threading.Tasks;
using Android.OS;
using Java.Net;
using Java.Util.Concurrent;
using Javax.Net.Ssl;
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
        private const string AuthorizationCookieKey = "Authorization";
        private const string AuthorizationCookieValueTemplate = "Bearer {0}";
        private const string AcceptLanguageCookieKey = "Accept-Language";
        private const string UrlStringTemplate = "{0}/api/v{1}/videos";
        private const string ContentTypeCookieValueTemplate = "multipart/form-data; boundary=\"{0}\"";

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
            StrictMode.SetThreadPolicy(new StrictMode.ThreadPolicy.Builder().PermitAll().Build());
            var environment = _environmentConfigurationProvider.Environment;
            var version = new Version(environment.ApiVersion);
            var currentCulture = CultureInfo.CurrentCulture;
            var accessToken = await _userSessionProvider.GetAccessTokenAsync().ConfigureAwait(false);
            var contentType = string.Format(ContentTypeCookieValueTemplate, FormDataBuilder.DefaultBoundary);

            var url = new URL(string.Format(UrlStringTemplate, environment.ApiUrl, version.Major));
            var httpClient = CreateUnsafeHttpClient();
            var requestBuilder = new Request.Builder().Url(url);

            var taskCompletionSource = new TaskCompletionSource<string>();

            // TODO: Refactoring this code.
            var formDataBuilder =
                FormDataBuilder.Create()
                    .AttachStringContent("order_id", uploadVideoDto.OrderId.ToString())
                    .AttachStringContent("title", uploadVideoDto.Title)
                    .AttachStringContent("description", uploadVideoDto.Description)
                    .AttachFileContent("video", uploadVideoDto.FilePath);

            var request =
                requestBuilder
                    .Post(new VideoRequestBody(onChangedProgressAction, formDataBuilder, contentType))
                    .AddHeader(AuthorizationCookieKey, string.Format(AuthorizationCookieValueTemplate, accessToken))
                    .AddHeader(AcceptLanguageCookieKey, currentCulture.TwoLetterISOLanguageName)
                    .AddHeader(FormDataBuilder.ContentTypeKey, contentType)
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
            var trustManagers = new[] { new AllowAllSSLX509ExtendedTrustManager() };
            var sslContext = SSLContext.GetInstance("SSL");
            sslContext.Init(null, trustManagers, new Java.Security.SecureRandom());
            var sslSocketFactory = sslContext.SocketFactory;

            var httpClient = new OkHttpClient.Builder()
                    .SslSocketFactory(sslSocketFactory, trustManagers[0])
                    .HostnameVerifier(new AllowAllSSLHostnamVerifier())
                    .ConnectTimeout(5, TimeUnit.Minutes)
                    .ReadTimeout(5, TimeUnit.Minutes)
                    .WriteTimeout(5, TimeUnit.Minutes)
                    .Build();
            return httpClient;
        }
    }
}
