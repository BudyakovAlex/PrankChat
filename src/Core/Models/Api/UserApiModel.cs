using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class UserApiModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Login { get; set; }

        public string Email { get; set; }

        public double Balance { get; set; }

        public string Avatar { get; set; }

        [JsonProperty("orders_own_count")]
        public int? OrdersOwnCount { get; set; }

        [JsonProperty("orders_execute_count")]
        public int? OrdersExecuteCount { get; set; }

        [JsonProperty("orders_execute_finished_count")]
        public int? OrdersExecuteFinishedCount { get; set; }

        public int? Subscribers { get; set; }

        public int? Subscriptions { get; set; }
    }
}
