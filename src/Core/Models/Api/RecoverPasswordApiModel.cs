using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class RecoverPasswordApiModel
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
