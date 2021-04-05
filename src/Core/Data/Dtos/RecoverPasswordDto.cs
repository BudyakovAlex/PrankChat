using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Data.Dtos
{
    public class RecoverPasswordDto
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
