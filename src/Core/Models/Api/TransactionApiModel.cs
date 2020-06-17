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
        public int? BalanceBefore { get; set; }

        [JsonProperty("balance_after")]
        public double? BalanceAfter { get; set; }

        [JsonProperty("frozen_before")]
        public int? FrozenBefore { get; set; }

        [JsonProperty("frozen_after")]
        public int? FrozenAfter { get; set; }

        [JsonProperty("user")]
        public DataApiModel<UserApiModel> User { get; set; }
    }
}
