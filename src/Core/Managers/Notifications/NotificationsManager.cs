using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Notifications;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using System.Threading.Tasks;
using PrankChat.Mobile.Core.Mappers;

namespace PrankChat.Mobile.Core.Managers.Notifications
{
    public class NotificationsManager : INotificationsManager
    {
        private readonly INotificationsService _notificationsService;

        public NotificationsManager(INotificationsService notificationsService)
        {
            _notificationsService = notificationsService;
        }

        public Task<PaginationModel<NotificationDataModel>> GetNotificationsAsync()
        {
            return _notificationsService.GetNotificationsAsync();
        }

        public Task MarkNotificationsAsReadedAsync()
        {
            return _notificationsService.MarkNotificationsAsReadedAsync();
        }

        public Task<int> GetUnreadNotificationsCountAsync()
        {
            return _notificationsService.GetUnreadNotificationsCountAsync();
        }

        public Task SendNotificationTokenAsync(string token)
        {
            return _notificationsService.SendNotificationTokenAsync(token);
        }

        public Task UnregisterNotificationsAsync()
        {
            return _notificationsService.UnregisterNotificationsAsync();
        }
    }
}