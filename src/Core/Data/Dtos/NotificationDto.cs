using System;
using Newtonsoft.Json;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Data.Dtos
{
    public class NotificationDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        [JsonProperty("is_delivered")]
        public bool? IsDelivered { get; set; }

        public NotificationType? Type { get; set; }

        public string Description { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("related_user")]
        public ResponseDto<UserDto> RelatedUser { get; set; }

        [JsonProperty("related_order")]
        public ResponseDto<OrderDto> RelatedOrder { get; set; }

        [JsonProperty("related_video")]
        public ResponseDto<VideoDto> RelatedVideo { get; set; }

        [JsonProperty("related_transaction")]
        public ResponseDto<TransactionDto> RelatedTransaction { get; set; }
    }
}
