using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Data.Dtos
{
    public class AccessTokenDto
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
