using System;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Presentation.Navigation.Results
{
    public class OrderDetailsResult
    {
        public OrderStatusType? Status { get; }

        public DateTime? ActiveTo { get; }

        public int? NegativeArbitrationValuesCount { get; }

        public int? PositiveArbitrationValuesCount { get; }

        public OrderDetailsResult(OrderDataModel order)
        {
            Status = order.Status;
            ActiveTo = order.ActiveTo;
            NegativeArbitrationValuesCount = order.NegativeArbitrationValuesCount;
            PositiveArbitrationValuesCount = order.PositiveArbitrationValuesCount;
        }
    }
}
