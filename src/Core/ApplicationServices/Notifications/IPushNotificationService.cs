using System.Threading.Tasks;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.ApplicationServices.Notifications
{
	public interface IPushNotificationService
	{
		Task<bool> TryUpdateTokenAsync();
	}
}
