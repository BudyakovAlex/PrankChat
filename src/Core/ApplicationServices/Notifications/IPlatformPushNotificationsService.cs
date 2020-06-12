namespace PrankChat.Mobile.Core.ApplicationServices.Notifications
{
    public interface IPlatformPushNotificationsService
    {
        void AttachNotifications();

        void DetachNotifications();
    }
}