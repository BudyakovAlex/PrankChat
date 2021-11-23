using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Extensions
{
    public static class UserExtensions
    {
        public static OrderTagType GetOrderTagType(this User user, int? customerId, OrderStatusType? orderStatusType)
        {
            switch (orderStatusType)
            {
                case OrderStatusType.Active:
                case OrderStatusType.New:
                    var isMine = customerId.HasValue && customerId == user?.Id;
                    return isMine ? OrderTagType.New : OrderTagType.NewNotMine;

                case OrderStatusType.VideoInProcess:
                case OrderStatusType.InWork:
                    return OrderTagType.InWork;

                case OrderStatusType.InArbitration:
                case OrderStatusType.ProcessCloseArbitration:
                    return OrderTagType.InArbitration;

                case OrderStatusType.WaitFinish:
                    return OrderTagType.Wait;

                case OrderStatusType.VideoWaitModeration:
                    return OrderTagType.InModeration;

                case OrderStatusType.Finished:
                case OrderStatusType.ClosedAfterArbitrationCustomerWin:
                case OrderStatusType.ClosedAfterArbitrationExecutorWin:
                    return OrderTagType.Finished;

                default:
                    return OrderTagType.None;
            }
        }

        public static OrderType GetOrderType(this User user, int? customerId, OrderStatusType orderStatusType)
        {
            if (customerId.HasValue && user?.Id == customerId)
            {
                return orderStatusType switch
                {
                    OrderStatusType.New => OrderType.MyOrderInModeration,
                    OrderStatusType.Finished => OrderType.MyOrderCompleted,
                    OrderStatusType.ClosedAfterArbitrationCustomerWin => OrderType.MyOrderCompleted,
                    OrderStatusType.ClosedAfterArbitrationExecutorWin => OrderType.MyOrderCompleted,
                    _ => OrderType.MyOrder,
                };
            }

            return orderStatusType == OrderStatusType.Finished ||
                   orderStatusType == OrderStatusType.ClosedAfterArbitrationCustomerWin ||
                   orderStatusType == OrderStatusType.ClosedAfterArbitrationExecutorWin
               ? OrderType.NotMyOrderCompleted
               : OrderType.NotMyOrder;
        }
    }
}
