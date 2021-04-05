using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Data.Dtos
{
    internal class AuthorizationDto
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
