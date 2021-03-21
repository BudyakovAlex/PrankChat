using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Data.Dtos
{
    public class CreateWithdrawalDto
    {
        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("credit_card_id")]
        public int CreditCardId { get; set; }
    }
}
