using System;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Models.Data
{
    public class Notification
    {
        public Notification(
            int id,
            string title,
            string description,
            string text,
            bool? isDelivered,
            NotificationType? type,
            DateTime? createdAt,
            User relatedUser,
            Order relatedOrder,
            Video relatedVideo,
            Transaction relatedTransaction)
        {
            Id = id;
            Title = title;
            Description = description;
            Text = text;
            IsDelivered = isDelivered;
            Type = type;
            CreatedAt = createdAt;
            RelatedUser = relatedUser;
            RelatedOrder = relatedOrder;
            RelatedVideo = relatedVideo;
            RelatedTransaction = relatedTransaction;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Text { get; set; }

        public bool? IsDelivered { get; set; }

        public NotificationType? Type { get; set; }

        public DateTime? CreatedAt { get; set; }

        public User RelatedUser { get; set; }

        public Order RelatedOrder { get; set; }

        public Video RelatedVideo { get; set; }

        public Transaction RelatedTransaction { get; set; }
    }
}
