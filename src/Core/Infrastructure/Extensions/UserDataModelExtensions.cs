using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Infrastructure.Extensions
{
    public static class UserDataModelExtensions
    {
        public static OrderType GetOrderType(this OrderDataModel orderDataModel, int? customerId)
        {
            return GetOrderTypeFor(customerId, orderDataModel.Id, orderDataModel.Status);
        }

        public static OrderType GetOrderType(this UserDataModel userDataModel, int? customerId, OrderStatusType orderStatusType)
        {
            return GetOrderTypeFor(customerId, userDataModel.Id, orderStatusType);
        }

        private static OrderType GetOrderTypeFor(int? orderCustomerId, int? currentUserId, OrderStatusType? orderStatusType)
        {
            if (orderStatusType == null)
                orderStatusType = OrderStatusType.New;

            if (currentUserId.HasValue && orderCustomerId.HasValue && orderCustomerId == currentUserId)
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
