using System;
using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class PaymentApiModel
    {
        public int Id { get; set; }

        public double? Amount { get; set; }

        public string Provider { get; set; }

        public string Status { get; set; }

        [JsonProperty("payment_link")]
        public string PaymentLink { get; set; }
    }
}
