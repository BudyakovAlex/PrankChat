using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using Plugin.DeviceInfo;
using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Data.Dtos.Base;
using PrankChat.Mobile.Core.Providers.Configuration;
using PrankChat.Mobile.Core.Providers.UserSession;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Notifications
{
    public class NotificationsService : INotificationsService
    {
        private readonly HttpClient _client;

        public NotificationsService(
            IUserSessionProvider userSessionProvider,
            IEnvironmentConfigurationProvider environmentConfigurationProvider,
            IMvxLogProvider logProvider,
            IMvxMessenger messenger)
        {
            var log = logProvider.GetLogFor<NotificationsService>();

            var environment = environmentConfigurationProvider.Environment;
            _client = new HttpClient(
                environment.ApiUrl,
                environment.ApiVersion,
                userSessionProvider,
                log,
                messenger);
        }

        public Task<BaseBundleDto<NotificationDto>> GetNotificationsAsync()
        {
            return _client.GetAsync<BaseBundleDto<NotificationDto>>("notifications");
        }

        public Task MarkNotificationsAsReadedAsync()
        {
            return _client.PostAsync<ResponseDto>("notifications/read");
        }

        public async Task<int> GetUnreadNotificationsCountAsync()
        {
            var bundle = await _client.GetAsync<ResponseDto<NotificationsSummaryDto>>("notifications/undelivered");
            return bundle?.Data?.UndeliveredCount ?? 0;
        }

        public Task SendNotificationTokenAsync(string token)
        {
            var pushNotificationApiMode = new PushNotificationDto()
            {
                Token = token,
                DeviceId = CrossDeviceInfo.Current.Id,
            };

            return _client.PostAsync("me/device", pushNotificationApiMode, true);
        }

        public Task UnregisterNotificationsAsync()
        {
            return _client.DeleteAsync($"/api/v1/me/device/{CrossDeviceInfo.Current.Id}", true);
        }
    }
}