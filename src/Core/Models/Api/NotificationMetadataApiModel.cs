using System;
using Newtonsoft.Json;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class NotificationMetadataApiModel
    {
        public int? Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        [JsonProperty("is_delivered")]
        public bool? IsDelivered { get; set; }

        public NotificationType Type { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        public UserApiModel Recipient { get; set; }

        [JsonProperty("related_user")]
        public DataApiModel<UserApiModel> RelatedUser { get; set; }

        [JsonProperty("related_order")]
        public DataApiModel<OrderApiModel> RelatedOrder { get; set; }

        [JsonProperty("related_video")]
        public DataApiModel<VideoMetadataApiModel> RelatedVideo { get; set; }

        [JsonProperty("relation_transaction")]
        public DataApiModel<TransactionApiModel> RelationTransaction { get; set; }
    }
}
