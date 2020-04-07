using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Models.Data
{
    public class CardDataModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("card_user_name")]
        public string CardUserName { get; set; }
    }
}
