using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.FilterTypes;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using System;

namespace PrankChat.Mobile.Core.Infrastructure.Extensions
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
                OrderFilterType.All => Constants.Rest.AllOrders,
                OrderFilterType.MyOwn => Constants.Rest.AllOrders,
                OrderFilterType.InProgress => Constants.Rest.InWork,
                OrderFilterType.New => Constants.Rest.NewOrders,
                OrderFilterType.MyOrdered => Constants.Rest.ProfileOwnOrders,
                OrderFilterType.MyCompletion => Constants.Rest.ProfileOwnOrdersInExecute,
                _ => throw new ArgumentException($"Uri path for {filterType} not found"),
            };
        }

        public static FullScreenVideo ToFullScreenVideo(this Order order)
        {
            if (order is null)
            {
                return null;
            }

            return new FullScreenVideo(
                order.Customer.Id,
                order.Customer.IsSubscribed,
                order.Video.Id,
                order.Video.StreamUri,
                order.Title,
                order.Description,
                order.Video.ShareUri,
                order.Customer.Avatar,
                order.Customer.Login.ToShortenName(),
                order.Video.LikesCount,
                order.Video.DislikesCount,
                order.Video.CommentsCount,
                order.Video.IsLiked,
                order.Video.IsDisliked,
                order.Video.Poster);
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