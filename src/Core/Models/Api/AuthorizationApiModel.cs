using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Models.Api
{
    internal class AuthorizationApiModel
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
