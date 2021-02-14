using System;
using Newtonsoft.Json;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Data.Dtos
{
    public class WithdrawalDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("amount")]
        public double? Amount { get; set; }

        [JsonProperty("status")]
        public WithdrawalStatusType Status { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }
    }
}
