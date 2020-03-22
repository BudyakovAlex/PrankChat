using System;
using PrankChat.Mobile.iOS.Delegates;
using UserNotifications;

namespace PrankChat.Mobile.iOS.PlatformBusinessServices
{
    public class NotificationWrapper
    {
        public static NotificationWrapper Instance { get; } = new NotificationWrapper();

        public void ScheduleLocalNotification(string title, string message)
        {
            var notificationCenter = UNUserNotificationCenter.Current;
            var content = new UNMutableNotificationContent
            {
                Title = title,
                Body = message,
                Sound = UNNotificationSound.Default
            };

            var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(1, false);
            var identifier = $"local_notification_{Guid.NewGuid().ToString()}";

            var request = UNNotificationRequest.FromIdentifier(identifier, content, trigger);
            UNUserNotificationCenter.Current.Delegate = new PrankChatNotificationDelegate();
            notificationCenter.AddNotificationRequest(request, (err) =>
            {
                if (err != null)
                {
                    // Report error
                    System.Console.WriteLine("Notification Error: {0}", err);
                }
                else
                {
                    // Report Success
                    System.Console.WriteLine("Notification Scheduled: {0}", request);
                }
            });
        }
    }
}
