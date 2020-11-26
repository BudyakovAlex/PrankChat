using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Notifications
{
    public interface INotificationsService
    {
        Task<PaginationModel<NotificationDataModel>> GetNotificationsAsync();

        Task MarkNotificationsAsReadedAsync();

        Task<int> GetUnreadNotificationsCountAsync();

        Task SendNotificationTokenAsync(string token);

        Task UnregisterNotificationsAsync();
    }
}