using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class CreateCardApiModel
    {
        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("card_user_name")]
        public string CardUserName { get; set; }
    }
}
