using System;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Models.Data
{
    public class NotificationMetadataDataModel
    {
        public int? Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public bool? IsDelivered { get; set; }

        public NotificationType? Type { get; set; }

        public DateTime? CreatedAt { get; set; }

        public UserDataModel Recipient { get; set; }

        public UserDataModel RelatedUser { get; set; }

        public OrderDataModel RelatedOrder { get; set; }

        public VideoMetadataDataModel RelatedVideo { get; set; }

        public TransactionDataModel RelationTransaction { get; set; }
    }
}
