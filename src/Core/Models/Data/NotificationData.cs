using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Models.Data
{
    public class PushNotificationData
    {
        public string Title { get; }

        public string Body { get; }

        public NotificationType? Type { get; }

        public int? OrderId { get; }

        public PushNotificationData(string title, string body)
        {
            Title = title;
            Body = body;
        }

        public PushNotificationData(string title, string body, NotificationType notificationType)
        {
            Title = title;
            Body = body;
            Type = notificationType;
        }

        public PushNotificationData(string title, string body, NotificationType notificationType, int orderId)
        {
            Title = title;
            Body = body;
            Type = notificationType;
            OrderId = orderId;
        }
    }
}
