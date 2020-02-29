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
                return orderStatusType == OrderStatusType.New
                    ? OrderType.MyOrderInModeration
                    : OrderType.MyOrder;
            }

            return OrderType.NotMyOrder;
        }
    }
}