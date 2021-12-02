using System;
using System.Globalization;
using System.Threading.Tasks;
using Foundation;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Providers.Configuration;
using PrankChat.Mobile.Core.Providers.UserSession;
using PrankChat.Mobile.Core.Services.Network;
using PrankChat.Mobile.iOS.Plugins.HttpClient.Builders;
using PrankChat.Mobile.iOS.Plugins.HttpClient.Delegates;

namespace PrankChat.Mobile.iOS.Plugins.HttpClient
{
    public class PlatformHttpClient : NSObject, IPlatformHttpClient
    {
        private const string BackgroundSessionConfigurationIdetifier = "BackgroundSessionConfigurationIdetifier";
        private const string PostHttpMethod = "POST";

        private readonly IEnvironmentConfigurationProvider _environmentConfigurationProvider;
        private readonly IUserSessionProvider _userSessionProvider;

        public PlatformHttpClient(
            IEnvironmentConfigurationProvider environmentConfigurationProvider,
            IUserSessionProvider userSessionProvider)
        {
            _environmentConfigurationProvider = environmentConfigurationProvider;
            _userSessionProvider = userSessionProvider;
        }

        public Task<string> UploadVideoAsync(UploadVideoDto uploadVideoDto, Action<double, double> onChangedProgressAction = null)
        {
            var taskCompletionSource = new TaskCompletionSource<string>();
            _ = UploadVideoAsync(uploadVideoDto, taskCompletionSource, onChangedProgressAction).ConfigureAwait(false);
            return taskCompletionSource.Task;
        }

        private async Task UploadVideoAsync(
            UploadVideoDto uploadVideoDto,
            TaskCompletionSource<string> taskCompletionSource,
            Action<double, double> onChangedProgressAction = null)
        {
            var environment = _environmentConfigurationProvider.Environment;
            var version = new Version(environment.ApiVersion);
            var currentCulture = CultureInfo.CurrentCulture;
            var urlString = string.Format(RestConstants.UrlStringTemplate, environment.ApiUrl, version.Major);

            // Create body content string.
            // TODO: Refactoring this code.
            var body = FormDataBuilder
                           .Create()
                           .AttachStringContent("order_id", uploadVideoDto.OrderId.ToString())
                           .AttachStringContent("title", uploadVideoDto.Title)
                           .AttachStringContent("description", uploadVideoDto.Description)
                           .AttachFileContent("video", uploadVideoDto.FilePath)
                           .Build();

            // Create request.
            var request = new NSMutableUrlRequest(new NSUrl(urlString), NSUrlRequestCachePolicy.ReloadIgnoringCacheData, 60);

            // Set header values.
            var accessToken = await _userSessionProvider.GetAccessTokenAsync();
            request[RestConstants.AuthorizationCookieKey] = new NSString(string.Format(RestConstants.AuthorizationCookieValueTemplate, accessToken));
            request[RestConstants.AcceptLanguageCookieKey] = new NSString(currentCulture.TwoLetterISOLanguageName);
            request[RestConstants.ContentTypeKey] = new NSString(string.Format(RestConstants.ContentTypeCookieValueTemplate, RestConstants.DefaultBoundary));

            // Set method and body content.
            request.HttpMethod = PostHttpMethod;
            request.Body = body;

            var configuration = NSUrlSessionConfiguration.CreateBackgroundSessionConfiguration(BackgroundSessionConfigurationIdetifier);
            var nsUrlSession = NSUrlSession.FromConfiguration(configuration, new UploadVideoUrlSessionDataDelegate(taskCompletionSource, onChangedProgressAction), NSOperationQueue.MainQueue);

            var uploadTask = nsUrlSession.CreateUploadTask(request);
            uploadTask.Resume();
            nsUrlSession.FinishTasksAndInvalidate();
        }
    }
}
