using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Notifications
{
    public interface IPushNotificationProvider
	{
		Task<bool> TryUpdateTokenAsync();

		Task UnregisterNotificationsAsync();
	}
}
