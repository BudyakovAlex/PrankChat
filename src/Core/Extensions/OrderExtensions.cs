using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.FilterTypes;
using PrankChat.Mobile.Core.Models.Enums;
using System;

namespace PrankChat.Mobile.Core.Extensions
{
    public static class OrderExtensions
    {
        public static TimeSpan? GetActiveOrderTime(this Order order) => order?.Status switch
        {
            null => null,
            OrderStatusType.VideoInProcess => order?.VideoUploadedIn < TimeSpan.Zero ? TimeSpan.Zero : order?.VideoUploadedIn,
            OrderStatusType.VideoWaitModeration => order?.VideoUploadedIn < TimeSpan.Zero ? TimeSpan.Zero : order?.VideoUploadedIn,
            OrderStatusType.VideoProcessError => order?.VideoUploadedIn < TimeSpan.Zero ? TimeSpan.Zero : order?.VideoUploadedIn,
            OrderStatusType.WaitFinish => order?.CloseOrderIn < TimeSpan.Zero ? TimeSpan.Zero : order?.CloseOrderIn,
            OrderStatusType.Finished => null,
            OrderStatusType.InArbitration => order?.ArbitrationFinishIn < TimeSpan.Zero ? TimeSpan.Zero : order?.ArbitrationFinishIn,
            _ => order?.FinishIn < TimeSpan.Zero ? TimeSpan.Zero : order?.FinishIn,
        };

        public static string GetOrderStatusTitle(this Order order, User currentUser) => order?.Status switch
        {
            OrderStatusType.New => Resources.OrderStatusNew,
            OrderStatusType.Rejected => Resources.OrderStatusRejected,
            OrderStatusType.Cancelled => Resources.OrderStatusCancelled,
            OrderStatusType.Active => Resources.OrderStatusActive,
            OrderStatusType.InWork => Resources.OrderStatusInWork,
            OrderStatusType.VideoWaitModeration => Resources.OrderStatusInWork,
            OrderStatusType.VideoInProcess => Resources.OrderStatusInWork,
            OrderStatusType.VideoProcessError => Resources.OrderStatusInWork,
            OrderStatusType.InArbitration => Resources.OrderStatusInArbitration,
            OrderStatusType.ProcessCloseArbitration => Resources.OrderStatusProcessCloseArbitration,
            OrderStatusType.ClosedAfterArbitrationCustomerWin when order?.Customer?.Id == currentUser?.Id => Resources.OrderStatusClosedAfterArbitrationCustomerWin,
            OrderStatusType.ClosedAfterArbitrationExecutorWin when order?.Customer?.Id == currentUser?.Id => Resources.OrderStatusClosedAfterArbitrationExecutorWin,
            OrderStatusType.ClosedAfterArbitrationCustomerWin when order?.Executor?.Id == currentUser?.Id => Resources.OrderStatusClosedAfterArbitrationExecutorWin,
            OrderStatusType.ClosedAfterArbitrationExecutorWin when order?.Executor?.Id == currentUser?.Id => Resources.OrderStatusClosedAfterArbitrationCustomerWin,
            OrderStatusType.WaitFinish => Resources.OrderStatusWaitFinish,
            OrderStatusType.Finished => Resources.OrderStatusFinished,
            _ => string.Empty,
        };

        public static string GetUrlResource(this OrderFilterType filterType) => filterType switch
        {
            OrderFilterType.All => RestConstants.AllOrders,
            OrderFilterType.MyOwn => RestConstants.AllOrders,
            OrderFilterType.InProgress => RestConstants.InWork,
            OrderFilterType.New => RestConstants.NewOrders,
            OrderFilterType.MyOrdered => RestConstants.ProfileOwnOrders,
            OrderFilterType.MyCompletion => RestConstants.ProfileOwnOrdersInExecute,
            _ => throw new ArgumentException($"Uri path for {filterType} not found"),
        };

        public static bool CheckIsTimeAvailable(this Order Order)
        {
            var timeValue = Order.GetActiveOrderTime();

            return Order?.Status != null &&
                   timeValue != null &&
                   timeValue >= TimeSpan.Zero &&
                   (Order.VideoUploadedAt != null &&
                   (Order.Status.Value == OrderStatusType.WaitFinish ||
                   Order.Status.Value == OrderStatusType.VideoInProcess ||
                   Order.Status.Value == OrderStatusType.VideoWaitModeration) ||
                   Order.FinishIn != null);
        }
    }
}