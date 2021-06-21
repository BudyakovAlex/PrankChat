using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Data.Dtos.Base;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Notifications
{
    public interface INotificationsService
    {
        Task<BaseBundleDto<NotificationDto>> GetNotificationsAsync();

        Task MarkNotificationsAsReadedAsync();

        Task<int> GetUnreadNotificationsCountAsync();

        Task SendNotificationTokenAsync(string token);

        Task UnregisterNotificationsAsync();
    }
}