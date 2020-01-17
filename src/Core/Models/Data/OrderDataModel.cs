using System;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Models.Data
{
    public class OrderDataModel
    {
        public long PriceTo { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public OrderStatusType Status { get; set; }

        public DateTime ActiveTo { get; set; }

        public bool AutoProlongation { get; set; }
    }
}
