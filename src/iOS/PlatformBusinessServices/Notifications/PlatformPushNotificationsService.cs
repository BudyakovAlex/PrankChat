using PrankChat.Mobile.Core.ApplicationServices.Notifications;

namespace PrankChat.Mobile.iOS.PlatformBusinessServices.Notifications
{
    public class PlatformPushNotificationsService : IPlatformPushNotificationsService
    {
        public PlatformPushNotificationsService()
        {
        }

        public void AttachNotifications()
        {
            NotificationWrapper.Instance.AttachNotifications();
        }

        public void DetachNotifications()
        {
            NotificationWrapper.Instance.DetachNotifications();
        }
    }
}
