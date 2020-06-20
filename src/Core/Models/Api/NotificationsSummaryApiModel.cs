using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class NotificationsSummaryApiModel
    {
        [JsonProperty("undelivered_count")]
        public int UndeliveredCount { get; set; }
    }
}