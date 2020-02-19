using System;
using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class RefillApiData
    {
        [JsonProperty("amount")]
        public double Amount { get; set; }
    }
}
