using System;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Notification.Items
{
    public class NotificationItemViewModel : BaseItemViewModel
    {
        private DateTime _date;

        public string ProfileName { get; }

        public string ImageUrl { get; }

        public string Description { get; }

        public string DateText => _date.ToShortDateString();

        public string Status { get; }

        public NotificationItemViewModel(string profileName, string description, string imageUrl, DateTime notificationDate, string status)
        {
            ProfileName = profileName;
            Description = description;
            ImageUrl = imageUrl;
            _date = notificationDate;
            Status = status;
        }
    }
}
