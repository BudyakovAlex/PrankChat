using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Models.Data
{
    public class PushNotificationData
    {
        public string Title { get; set; }

        public string Body { get; set; }

        public NotificationType? Type { get; set; }

        public int? OrderId { get; set; }

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
