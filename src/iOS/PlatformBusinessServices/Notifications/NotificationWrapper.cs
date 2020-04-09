using System;
using Firebase.CloudMessaging;
using Firebase.InstanceID;
using Foundation;
using MvvmCross;
using MvvmCross.Logging;
using PrankChat.Mobile.Core.ApplicationServices.Notifications;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.iOS.Delegates;
using UIKit;
using UserNotifications;
using PrankChat.Mobile.Core.Infrastructure.Extensions;

namespace PrankChat.Mobile.iOS.PlatformBusinessServices.Notifications
{
    public class NotificationWrapper
    {
        public static NotificationWrapper Instance { get; } = new NotificationWrapper();

        public void TokenRefreshNotification(object sender, NSNotificationEventArgs e)
        {
            var settingService = Mvx.IoCProvider.Resolve<ISettingsService>();
            settingService.PushToken = Messaging.SharedInstance.FcmToken;

            try
            {
                var pushNotificationService = Mvx.IoCProvider.Resolve<IPushNotificationService>();
                pushNotificationService.TryUpdateTokenAsync().FireAndForget();
            }
            catch (Exception ex)
            {
                var log = Mvx.IoCProvider.Resolve<IMvxLog>();
                log.ErrorException("Can not resolve IPushNotificationService", ex);
            }
        }

        /// <summary>
        /// Handles foreground notifications.
        /// </summary>
        public void HandleForegroundNotification(NSDictionary userInfo)
        {
            var payload = HandleNotificationPayload(userInfo);
            ShowLocalNotification(payload.title, payload.body);
        }

        public void HandleBackgroundNotification(NSDictionary userInfo)
        {
            var payload = HandleNotificationPayload(userInfo);
            //TryNavigateToSignalDetails(payload.signalId);
        }

        private void ShowLocalNotification(string title, string body)
        {
            ScheduleLocalNotification(title, body);
        }

        private void ScheduleLocalNotification(string title, string message)
        {
            var notificationCenter = UNUserNotificationCenter.Current;
            var content = new UNMutableNotificationContent
            {
                Title = title,
                Body = message,
                Sound = UNNotificationSound.Default
            };

            var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(1, false);
            var identifier = $"local_notification_{Guid.NewGuid()}";

            var request = UNNotificationRequest.FromIdentifier(identifier, content, trigger);
            UNUserNotificationCenter.Current.Delegate = new PrankChatNotificationDelegate();
            notificationCenter.AddNotificationRequest(request, (err) =>
            {
                if (err != null)
                {
                    // Report error
                    Console.WriteLine("Notification Error: {0}", err);
                }
                else
                {
                    // Report Success
                    Console.WriteLine("Notification Scheduled: {0}", request);
                }
            });
        }
    }
}
