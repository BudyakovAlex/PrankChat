using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class NotificationMetadataApiModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        [JsonProperty("is_delivered")]
        public int IsDelivered { get; set; }

        public UserApiModel Recipient { get; set; }

        [JsonProperty("related_user")]
        public UserApiModel RelatedUser { get; set; }

        [JsonProperty("related_order")]
        public OrderApiModel RelatedOrder { get; set; }

        [JsonProperty("related_video")]
        public VideoMetadataApiModel RelatedVideo { get; set; }

        [JsonProperty("relation_transaction")]
        public TransactionApiModel RelationTransaction { get; set; }
    }
}
