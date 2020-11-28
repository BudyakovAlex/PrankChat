using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using Plugin.DeviceInfo;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Abstract;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Authorization;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Notifications;
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
    public class NotificationsManager : INotificationsManager
    {
        private readonly INotificationsService _notificationsService;

        public NotificationsManager(INotificationsService notificationsService)
        {
            _notificationsService = notificationsService;
        }

        public async Task<PaginationModel<NotificationDataModel>> GetNotificationsAsync()
        {
            return await _notificationsService.GetNotificationsAsync();
        }

        public Task MarkNotificationsAsReadedAsync()
        {
            return _notificationsService.MarkNotificationsAsReadedAsync();
        }

        public async Task<int> GetUnreadNotificationsCountAsync()
        {
            return await _notificationsService.GetUnreadNotificationsCountAsync();
        }

        public Task SendNotificationTokenAsync(string token)
        {
            return _notificationsService.SendNotificationTokenAsync(token);
        }

        public async Task UnregisterNotificationsAsync()
        {
            await _notificationsService.UnregisterNotificationsAsync();
        }
    }
}