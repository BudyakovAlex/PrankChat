using System;
using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class TakeOrderApiModel
    {
        [JsonProperty("user_id")]
        public int ExecutorId { get; set; }
    }
}
