using System;
using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class OrderDetailsApiModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("active_to")]
        public DateTime ActiveTo { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("сustomer")]
        public UserApiModel Сustomer { get; set; }

        [JsonProperty("executor")]
        public UserApiModel Executor { get; set; }
    }
}
