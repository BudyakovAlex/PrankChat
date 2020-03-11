using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Notifications
{
	public interface IPushNotificationService
	{
		Task UpdateToken();
	}
}
