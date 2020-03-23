using System.Collections.Generic;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.FilterTypes;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order
{
    public class OrderSortComparer : IComparer<OrderDataModel>
    {
        public OrderSortComparer(int currentUserId, OrderFilterType filterType = OrderFilterType.All)
        {
            CurrentUserId = currentUserId;
            FilterType = filterType;
        }

        public int CurrentUserId { get; }

        public OrderFilterType FilterType { get; }

        public int Compare(OrderDataModel x, OrderDataModel y)
        {
            if (FilterType == OrderFilterType.MyOwn)
            {
                var result = IsOrderInactive(x).CompareTo(IsOrderInactive(y));
                return result;
            }

            return x.GetOrderType(CurrentUserId).CompareTo(y.GetOrderType(CurrentUserId));
        }

        private bool IsOrderInactive(OrderDataModel order) => order?.Status == Models.Enums.OrderStatusType.New;
    }
}
