using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Data.Dtos
{
    public class RefillDto
    {
        [JsonProperty("amount")]
        public double Amount { get; set; }
    }
}
