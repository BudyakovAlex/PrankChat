using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Data.Dtos
{
    public class CreateCardDto
    {
        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("card_user_name")]
        public string CardUserName { get; set; }
    }
}
