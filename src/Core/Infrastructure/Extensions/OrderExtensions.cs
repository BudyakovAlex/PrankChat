using System;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.FilterTypes;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Video;

namespace PrankChat.Mobile.Core.Infrastructure.Extensions
{
    public static class OrderExtensions
    {
        public static TimeSpan? GetActiveOrderTime(this OrderDataModel order)
        {
            switch (order?.Status)
            {
                case null:
                    return null;

                case OrderStatusType.VideoInProcess:
                case OrderStatusType.VideoWaitModeration:
                case OrderStatusType.VideoProcessError:
                    return order?.VideoUploadedIn < TimeSpan.Zero ? TimeSpan.Zero : order?.VideoUploadedIn;

                case OrderStatusType.WaitFinish:
                    return order?.CloseOrderIn < TimeSpan.Zero ? TimeSpan.Zero : order?.CloseOrderIn;

                case OrderStatusType.Finished:
                    return null;

                case OrderStatusType.InArbitration:
                    return order?.ArbitrationFinishIn < TimeSpan.Zero ? TimeSpan.Zero : order?.ArbitrationFinishIn;

                default:
                    return order?.FinishIn < TimeSpan.Zero ? TimeSpan.Zero : order?.FinishIn;
            }
        }

        public static string GetOrderStatusTitle(this OrderDataModel orderDataModel, UserDataModel currentUser)
        {
            switch (orderDataModel?.Status)
            {
                case OrderStatusType.New:
                    return Resources.OrderStatus_New;

                case OrderStatusType.Rejected:
                    return Resources.OrderStatus_Rejected;

                case OrderStatusType.Cancelled:
                    return Resources.OrderStatus_Cancelled;

                case OrderStatusType.Active:
                    return Resources.OrderStatus_Active;

                case OrderStatusType.InWork:
                case OrderStatusType.VideoWaitModeration:
                case OrderStatusType.VideoInProcess:
                case OrderStatusType.VideoProcessError:
                    return Resources.OrderStatus_InWork;

                case OrderStatusType.InArbitration:
                    return Resources.OrderStatus_InArbitration;

                case OrderStatusType.ProcessCloseArbitration:
                    return Resources.OrderStatus_ProcessCloseArbitration;

                case OrderStatusType.ClosedAfterArbitrationCustomerWin when orderDataModel?.Customer?.Id == currentUser?.Id:
                    return Resources.OrderStatus_ClosedAfterArbitrationCustomerWin;

                case OrderStatusType.ClosedAfterArbitrationExecutorWin when orderDataModel?.Customer?.Id == currentUser?.Id:
                    return Resources.OrderStatus_ClosedAfterArbitrationExecutorWin;

                case OrderStatusType.ClosedAfterArbitrationCustomerWin when orderDataModel?.Executor?.Id == currentUser?.Id:
                    return Resources.OrderStatus_ClosedAfterArbitrationExecutorWin;

                case OrderStatusType.ClosedAfterArbitrationExecutorWin when orderDataModel?.Executor?.Id == currentUser?.Id:
                    return Resources.OrderStatus_ClosedAfterArbitrationCustomerWin;

                case OrderStatusType.WaitFinish:
                    return Resources.OrderStatus_WaitFinish;

                case OrderStatusType.Finished:
                    return Resources.OrderStatus_Finished;

                default:
                    return string.Empty;
            }
        }

        public static string GetUrlResource(this OrderFilterType filterType)
        {
            switch (filterType)
            {
                case OrderFilterType.All:
                case OrderFilterType.MyOwn:
                    return Constants.Rest.AllOrders;
                case OrderFilterType.InProgress:
                    return  Constants.Rest.InWork;
                case OrderFilterType.New:
                    return Constants.Rest.NewOrders;
                case OrderFilterType.MyOrdered:
                    return Constants.Rest.ProfileOwnOrders;
                case OrderFilterType.MyCompletion:
                    return Constants.Rest.ProfileOwnOrdersInExecute;
                default:
                    throw new ArgumentException($"Uri path for {filterType} not found");
            }
        }

        public static FullScreenVideoDataModel ToFullScreenVideoDataModel(this OrderDataModel order)
        {
            if (order is null)
            {
                return null;
            }

            return new FullScreenVideoDataModel(order.Customer.Id,
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

        public static bool CheckIsTimeAvailable(this OrderDataModel Order)
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