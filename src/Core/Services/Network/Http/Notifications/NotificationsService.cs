using MvvmCross.Plugin.Messenger;
using Plugin.DeviceInfo;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Data.Dtos.Base;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Providers.Configuration;
using PrankChat.Mobile.Core.Providers.UserSession;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Services.Network.Http.Notifications
{
    public class NotificationsService : INotificationsService
    {
        private readonly HttpClient _client;

        public NotificationsService(
            IUserSessionProvider userSessionProvider,
            IEnvironmentConfigurationProvider environmentConfigurationProvider,
            IMvxMessenger messenger)
        {
            var environment = environmentConfigurationProvider.Environment;
            _client = new HttpClient(
                environment.ApiUrl,
                environment.ApiVersion,
                userSessionProvider,
                this.Logger(),
                messenger);
        }

        public Task<BaseBundleDto<NotificationDto>> GetNotificationsAsync()
        {
            return _client.GetAsync<BaseBundleDto<NotificationDto>>(RestConstants.Notifications);
        }

        public Task MarkNotificationsAsReadedAsync()
        {
            return _client.PostAsync<ResponseDto>(RestConstants.NotificationsRead);
        }

        public async Task<int> GetUnreadNotificationsCountAsync()
        {
            var bundle = await _client.GetAsync<ResponseDto<NotificationsSummaryDto>>(RestConstants.NotificationsUnreaded);
            return bundle?.Data?.UndeliveredCount ?? 0;
        }

        public Task SendNotificationTokenAsync(string token)
        {
            var pushNotificationApiMode = new PushNotificationDto()
            {
                Token = token,
                DeviceId = CrossDeviceInfo.Current.Id,
            };

            return _client.PostAsync(RestConstants.MyDevice, pushNotificationApiMode, true);
        }

        public Task UnregisterNotificationsAsync()
        {
            return _client.DeleteAsync($"{RestConstants.MyDevice}/{CrossDeviceInfo.Current.Id}", true);
        }
    }
}