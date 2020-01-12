using System;
using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class OrderApiModel
    {
        [JsonProperty("is_active")]
        public bool IsActive { get; set; }

        [JsonProperty("customer_id")]
        public int CustomerId { get; set; }

        [JsonProperty("executor_id")]
        public int ExecutorId { get; set; }

        [JsonProperty("dateFrom")]
        public DateTime DateFrom { get; set; }

        [JsonProperty("dateTo")]
        public DateTime DateTo { get; set; }

        [JsonProperty("priceFrom")]
        public long PriceFrom { get; set; }

        [JsonProperty("priceTo")]
        public long PriceTo { get; set; }
    }
}
