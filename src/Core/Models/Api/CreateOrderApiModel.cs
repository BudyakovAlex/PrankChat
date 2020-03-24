using System;
using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class CreateOrderApiModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("active_for")]
        public int ActiveFor { get; set; }

        [JsonProperty("auto_prolongation")]
        public bool AutoProlongation { get; set; }
    }
}
