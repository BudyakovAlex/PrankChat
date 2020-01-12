using System;
using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class CreateOrderApiModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public long Price { get; set; }

        [JsonProperty("active_to")]
        public DateTime ActiveTo { get; set; }

        [JsonProperty("auto_prolongation")]
        public bool AutoProlongation { get; set; }
    }
}
