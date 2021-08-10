using PrankChat.Mobile.Core.Mappers;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.Services.Network.Http.Notifications;
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

        public async Task<Pagination<Notification>> GetNotificationsAsync()
        {
            var response = await _notificationsService.GetNotificationsAsync();
            return response.Map(item => item.Map());
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