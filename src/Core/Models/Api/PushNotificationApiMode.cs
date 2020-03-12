using System;
using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class PushNotificationApiMode
    {
        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

        [JsonProperty("fcm_token")]
        public string Token { get; set; }
    }
}
