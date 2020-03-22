namespace PrankChat.Mobile.Core.Models.Data
{
    public class PushNotificationData
    {
        public string Title { get; set; }
        public string Body { get; set; }

        public PushNotificationData(string title, string body)
        {
            Title = title;
            Body = body;
        }
    }
}
