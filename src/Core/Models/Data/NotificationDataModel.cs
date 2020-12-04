using System;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Models.Data
{
    public class NotificationDataModel
    {
        public NotificationDataModel(int id,
                                     string title,
                                     string description,
                                     string text,
                                     bool? isDelivered,
                                     NotificationType? type,
                                     DateTime? createdAt,
                                     UserDataModel relatedUser,
                                     OrderDataModel relatedOrder,
                                     VideoDataModel relatedVideo,
                                     TransactionDataModel relatedTransaction)
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

        public UserDataModel RelatedUser { get; set; }

        public OrderDataModel RelatedOrder { get; set; }

        public VideoDataModel RelatedVideo { get; set; }

        public TransactionDataModel RelatedTransaction { get; set; }
    }
}
