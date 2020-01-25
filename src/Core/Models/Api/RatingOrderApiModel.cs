using System;
using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class RatingOrderApiModel : OrderApiModel
    {
        [JsonProperty("positive_arbitration_values_count")]
        public int Likes { get; set; }

        [JsonProperty("negative_arbitration_values_count")]
        public int Dislikes { get; set; }
    }
}
