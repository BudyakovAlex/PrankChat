using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Data.Dtos
{
    public class ExternalAuthorizationDto
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}