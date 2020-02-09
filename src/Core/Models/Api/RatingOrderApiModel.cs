using System;
using Newtonsoft.Json;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class RatingOrderApiModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("price")]
        public long? Price { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("status")]
        public OrderStatusType? Status { get; set; }

        [JsonProperty("auto_prolongation")]
        public bool? AutoProlongation { get; set; }

        [JsonProperty("customer")]
        public DataApiModel<UserApiModel> Customer { get; set; }

        [JsonProperty("positive_arbitration_values_count")]
        public int Likes { get; set; }

        [JsonProperty("negative_arbitration_values_count")]
        public int Dislikes { get; set; }
    }
}
