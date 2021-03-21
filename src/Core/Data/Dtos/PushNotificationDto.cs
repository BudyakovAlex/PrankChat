using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Data.Dtos
{
    public class PushNotificationDto
    {
        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

        [JsonProperty("fcm_token")]
        public string Token { get; set; }
    }
}
