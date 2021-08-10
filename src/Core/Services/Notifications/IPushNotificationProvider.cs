using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Services.Notifications
{
    public interface IPushNotificationProvider
	{
		Task<bool> TryUpdateTokenAsync();

		Task UnregisterNotificationsAsync();
	}
}
