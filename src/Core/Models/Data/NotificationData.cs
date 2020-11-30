using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Models.Data
{
    public class PushNotificationData
    {
        public PushNotificationData(string title, string body)
        {
            Title = title ?? string.Empty;
            Body = body ?? string.Empty;
        }

        public PushNotificationData(string title, string body, NotificationType notificationType)
        {
            Title = title ?? string.Empty;
            Body = body ?? string.Empty;
            Type = notificationType;
        }

        public PushNotificationData(string title, string body, NotificationType notificationType, int orderId)
        {
            Title = title ?? string.Empty;
            Body = body ?? string.Empty;
            Type = notificationType;
            OrderId = orderId;
        }
        public string Title { get; }

        public string Body { get; }

        public NotificationType? Type { get; }

        public int? OrderId { get; }
    }
}
