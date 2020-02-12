using System;

namespace PrankChat.Mobile.Core.Models.Data
{
    public class PaymentDataModel
    {
        public int Id { get; set; }

        public double? Amount { get; set; }

        public string Provider { get; set; }

        public string Status { get; set; }

        public string PaymentLink { get; set; }
    }
}
