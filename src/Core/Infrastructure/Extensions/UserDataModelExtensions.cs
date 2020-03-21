using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Infrastructure.Extensions
{
    public static class UserDataModelExtensions
    {
        public static OrderType GetOrderType(this UserDataModel userDataModel, int? customerId, OrderStatusType orderStatusType)
        {
            if (customerId == userDataModel?.Id)
            {
                switch (orderStatusType)
                {
                    case OrderStatusType.New:
                        return OrderType.MyOrderInModeration;
                    case OrderStatusType.Finished:
                        return OrderType.MyOrderCompleted;
                    default:
                        return OrderType.MyOrder;
                }
            }

            return orderStatusType == OrderStatusType.Finished ? OrderType.NotMyOrderCompleted : OrderType.NotMyOrder;
        }
    }
}
