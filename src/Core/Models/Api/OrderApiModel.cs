using System;
using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class OrderApiModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("price")]
        public long PriceTo { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("active_to")]
        public DateTime ActiveTo { get; set; }

        [JsonProperty("auto_prolongation")]
        public bool AutoProlongation { get; set; }
    }
}
