using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class AppleAuthApiModel
    {
        [JsonProperty("user_name")]
        public string UserName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("identity_token")]
        public string IdentityToken { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}