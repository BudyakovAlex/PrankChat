using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class TransactionApiModel
    {
        public int Id { get; set; }

        public double? Amount { get; set; }

        public string Comment { get; set; }

        public string Direction { get; set; }

        public string Reason { get; set; }

        [JsonProperty("balance_before")]
        public double? BalanceBefore { get; set; }

        [JsonProperty("balance_after")]
        public double? BalanceAfter { get; set; }

        [JsonProperty("frozen_before")]
        public double? FrozenBefore { get; set; }

        [JsonProperty("frozen_after")]
        public double? FrozenAfter { get; set; }

        [JsonProperty("user")]
        public DataApiModel<UserApiModel> User { get; set; }
    }
}
