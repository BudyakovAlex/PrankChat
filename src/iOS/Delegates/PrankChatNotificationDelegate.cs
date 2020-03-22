using System;
using Foundation;
using UserNotifications;

namespace PrankChat.Mobile.iOS.Delegates
{
    public class PrankChatNotificationDelegate : UNUserNotificationCenterDelegate
    {
		public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
		{
			var userInfo = response.Notification.Request.Content.UserInfo;
			if (userInfo.TryGetValue((NSString)"uri", out var uri))
			{
				//App.Current.OpenUrl(uri.ToString()).Ignore();
			}
			else if (userInfo.ValueForKeyPath((NSString)"data.pinpoint.deeplink") is NSString deeplink)
			{
				//App.Current.OpenUrl(deeplink.ToString()).Ignore();
			}

			completionHandler();
		}

		public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
		{
			completionHandler(UNNotificationPresentationOptions.Alert);
		}
	}
}
