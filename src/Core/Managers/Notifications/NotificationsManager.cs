using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using Plugin.DeviceInfo;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Abstract;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Authorization;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.BusinessServices.Logger;
using PrankChat.Mobile.Core.Configuration;
using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Api.Base;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Managers.Notifications
{
    public class NotificationsManager : BaseRestService, INotificationsManager
    {
        private readonly IMvxMessenger _messenger;
        private readonly IMvxLog _log;

        private readonly HttpClient _client;

        public NotificationsManager(
            ISettingsService settingsService,
            IAuthorizationService authorizeService,
            IMvxLogProvider logProvider,
            IMvxMessenger messenger,
            ILogger logger) : base(settingsService, authorizeService, logProvider, messenger, logger)
        {
            _messenger = messenger;
            _log = logProvider.GetLogFor<NotificationsManager>();

            var configuration = ConfigurationProvider.GetConfiguration();
            _client = new HttpClient(configuration.BaseAddress,
                                     configuration.ApiVersion,
                                     settingsService,
                                     _log,
                                     logger,
                                     messenger);

            _messenger.Subscribe<UnauthorizedMessage>(OnUnauthorizedUser, MvxReference.Strong);
        }

        public async Task<PaginationModel<NotificationDataModel>> GetNotificationsAsync()
        {
            var notificationBundle = await _client.GetAsync<BaseBundleApiModel<NotificationApiModel>>("notifications");
            return CreatePaginationResult<NotificationApiModel, NotificationDataModel>(notificationBundle);
        }

        public Task MarkNotificationsAsReadedAsync()
        {
            return _client.PostAsync<DataApiModel>("notifications/read");
        }

        public async Task<int> GetUnreadNotificationsCountAsync()
        {
            var bundle = await _client.GetAsync<DataApiModel<NotificationsSummaryApiModel>>("notifications/undelivered");
            return bundle?.Data?.UndeliveredCount ?? 0;
        }

        public Task SendNotificationTokenAsync(string token)
        {
            var pushNotificationApiMode = new PushNotificationApiMode()
            {
                Token = token,
                DeviceId = CrossDeviceInfo.Current.Id,
            };

            return _client.PostAsync("me/device", pushNotificationApiMode, true);
        }

        public async Task UnregisterNotificationsAsync()
        {
            await _client.DeleteAsync($"/api/v1/me/device/{CrossDeviceInfo.Current.Id}", true);
        }

        private PaginationModel<TDataModel> CreatePaginationResult<TApiModel, TDataModel>(BaseBundleApiModel<TApiModel> data)
            where TDataModel : class
            where TApiModel : class
        {
            var mappedModels = MappingConfig.Mapper.Map<List<TDataModel>>(data?.Data ?? new List<TApiModel>());
            var paginationData = data?.Meta?.FirstOrDefault();
            var totalItemsCount = paginationData?.Value?.Total ?? mappedModels.Count;
            return new PaginationModel<TDataModel>(mappedModels, totalItemsCount);
        }
    }
}