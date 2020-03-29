using System;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Models.Data
{
    public class WithdrawalDataModel
    {
        public int Id { get; set; }

        public double? Amount { get; set; }

        public WithdrawalStatusType Status { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
