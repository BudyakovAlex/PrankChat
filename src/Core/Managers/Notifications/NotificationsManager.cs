using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Notifications;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
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