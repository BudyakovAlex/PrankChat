using System;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.FilterTypes;
using PrankChat.Mobile.Core.Models.Enums;

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
                case OrderStatusType.WaitFinish:
                    return order?.VideoUploadedIn < TimeSpan.Zero ? TimeSpan.Zero : order?.VideoUploadedIn;
                case OrderStatusType.Finished:
                    return null;
                case OrderStatusType.InArbitration:
                    return order?.ArbitrationFinishIn < TimeSpan.Zero ? TimeSpan.Zero : order?.ArbitrationFinishIn;
                default:
                    return order?.FinishIn < TimeSpan.Zero ? TimeSpan.Zero : order?.FinishIn;
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
    }
}