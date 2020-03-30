using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class CreateWithdrawalApiModel
    {
        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("credit_card_id")]
        public int CreditCardId { get; set; }
    }
}
