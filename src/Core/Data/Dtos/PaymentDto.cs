using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Data.Dtos
{
    public class PaymentDto
    {
        public int Id { get; set; }

        public double? Amount { get; set; }

        public string Provider { get; set; }

        public string Status { get; set; }

        [JsonProperty("payment_link")]
        public string PaymentLink { get; set; }
    }
}
