using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Managers.Notifications
{
    public interface INotificationsManager
    {
        Task<PaginationModel<NotificationDataModel>> GetNotificationsAsync();

        Task MarkNotificationsAsReadedAsync();

        Task<int> GetUnreadNotificationsCountAsync();

        Task SendNotificationTokenAsync(string token);

        Task UnregisterNotificationsAsync();
    }
}