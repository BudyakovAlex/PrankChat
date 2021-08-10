using System;
using Foundation;
using PrankChat.Mobile.Core.Services.Notifications;
using PrankChat.Mobile.iOS.PlatformBusinessServices.Notifications;
using UserNotifications;

namespace PrankChat.Mobile.iOS.Delegates
{
    public class PrankChatNotificationDelegate : UNUserNotificationCenterDelegate
    {
		public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
		{
			var userInfo = response.Notification.Request.Content.UserInfo;
			var pushNotificationData = NotificationWrapper.Instance.HandleNotificationPayload(userInfo);
			NotificationManager.Instance.TryNavigateToView(pushNotificationData?.OrderId);
			completionHandler?.Invoke();
		}

		public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
		{
			completionHandler?.Invoke(UNNotificationPresentationOptions.Alert);
		}
	}
}
