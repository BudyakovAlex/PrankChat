using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Infrastructure.Extensions
{
    public static class UserDataModelExtensions
    {
        public static OrderTagType GetOrderTagType(this UserDataModel userDataModel, int? customerId, OrderStatusType? orderStatusType)
        {
            if (userDataModel is null)
            {
                return OrderTagType.None;
            }

            switch (orderStatusType)
            {
                case OrderStatusType.Active:
                case OrderStatusType.New:
                    var isMine = customerId.HasValue && customerId == userDataModel.Id;
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

        public static OrderType GetOrderType(this UserDataModel userDataModel, int? customerId, OrderStatusType orderStatusType)
        {
            if (customerId.HasValue && userDataModel?.Id == customerId)
            {
                switch (orderStatusType)
                {
                    case OrderStatusType.New:
                        return OrderType.MyOrderInModeration;
                    case OrderStatusType.Finished:
                    case OrderStatusType.ClosedAfterArbitrationCustomerWin:
                    case OrderStatusType.ClosedAfterArbitrationExecutorWin:
                        return OrderType.MyOrderCompleted;
                    default:
                        return OrderType.MyOrder;
                }
            }

            return orderStatusType == OrderStatusType.Finished ||
                   orderStatusType == OrderStatusType.ClosedAfterArbitrationCustomerWin ||
                   orderStatusType == OrderStatusType.ClosedAfterArbitrationExecutorWin
               ? OrderType.NotMyOrderCompleted
               : OrderType.NotMyOrder;
        }
    }
}
