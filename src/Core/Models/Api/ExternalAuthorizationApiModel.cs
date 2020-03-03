using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class ExternalAuthorizationApiModel
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}