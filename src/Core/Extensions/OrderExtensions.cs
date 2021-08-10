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
        public static TimeSpan? GetActiveOrderTime(this Order order)
        {
            return (order?.Status) switch
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
        }

        public static string GetOrderStatusTitle(this Order order, User currentUser)
        {
            return (order?.Status) switch
            {
                OrderStatusType.New => Resources.OrderStatus_New,
                OrderStatusType.Rejected => Resources.OrderStatus_Rejected,
                OrderStatusType.Cancelled => Resources.OrderStatus_Cancelled,
                OrderStatusType.Active => Resources.OrderStatus_Active,
                OrderStatusType.InWork => Resources.OrderStatus_InWork,
                OrderStatusType.VideoWaitModeration => Resources.OrderStatus_InWork,
                OrderStatusType.VideoInProcess => Resources.OrderStatus_InWork,
                OrderStatusType.VideoProcessError => Resources.OrderStatus_InWork,
                OrderStatusType.InArbitration => Resources.OrderStatus_InArbitration,
                OrderStatusType.ProcessCloseArbitration => Resources.OrderStatus_ProcessCloseArbitration,
                OrderStatusType.ClosedAfterArbitrationCustomerWin when order?.Customer?.Id == currentUser?.Id => Resources.OrderStatus_ClosedAfterArbitrationCustomerWin,
                OrderStatusType.ClosedAfterArbitrationExecutorWin when order?.Customer?.Id == currentUser?.Id => Resources.OrderStatus_ClosedAfterArbitrationExecutorWin,
                OrderStatusType.ClosedAfterArbitrationCustomerWin when order?.Executor?.Id == currentUser?.Id => Resources.OrderStatus_ClosedAfterArbitrationExecutorWin,
                OrderStatusType.ClosedAfterArbitrationExecutorWin when order?.Executor?.Id == currentUser?.Id => Resources.OrderStatus_ClosedAfterArbitrationCustomerWin,
                OrderStatusType.WaitFinish => Resources.OrderStatus_WaitFinish,
                OrderStatusType.Finished => Resources.OrderStatus_Finished,
                _ => string.Empty,
            };
        }

        public static string GetUrlResource(this OrderFilterType filterType)
        {
            return filterType switch
            {
                OrderFilterType.All => RestConstants.AllOrders,
                OrderFilterType.MyOwn => RestConstants.AllOrders,
                OrderFilterType.InProgress => RestConstants.InWork,
                OrderFilterType.New => RestConstants.NewOrders,
                OrderFilterType.MyOrdered => RestConstants.ProfileOwnOrders,
                OrderFilterType.MyCompletion => RestConstants.ProfileOwnOrdersInExecute,
                _ => throw new ArgumentException($"Uri path for {filterType} not found"),
            };
        }

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