using System;
using Newtonsoft.Json;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class UserApiModel
    {
        public int Id { get; set; }

        public string Avatar { get; set; }

        public string Name { get; set; }

        public string Login { get; set; }

        public string Email { get; set; }

        public double? Balance { get; set; }

        public DateTime? Birthday { get; set; }

        public GenderType? Sex { get; set; }

        public string Description { get; set; }

        [JsonProperty("document_verified_at")]
        public DateTime? DocumentVerifiedAt { get; set; }

        [JsonProperty("orders_own_count")]
        public int? OrdersOwnCount { get; set; }

        [JsonProperty("orders_execute_count")]
        public int? OrdersExecuteCount { get; set; }

        [JsonProperty("orders_execute_finished_count")]
        public int? OrdersExecuteFinishedCount { get; set; }

        [JsonProperty("subscribers_count")]
        public int? SubscribersCount { get; set; }

        [JsonProperty("subscriptions_count")]
        public int? SubscriptionsCount { get; set; }
    }
}
