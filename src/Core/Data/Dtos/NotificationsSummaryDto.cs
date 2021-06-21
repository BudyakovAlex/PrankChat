using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Data.Dtos
{
    public class NotificationsSummaryDto
    {
        [JsonProperty("undelivered_count")]
        public int UndeliveredCount { get; set; }
    }
}