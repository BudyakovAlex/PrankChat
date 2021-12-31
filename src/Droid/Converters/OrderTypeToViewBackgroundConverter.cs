using System;
using System.Globalization;
using MvvmCross.Converters;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Droid.Converters
{
    public class OrderTypeToViewBackgroundConverter : MvxValueConverter<OrderType, string>
    {
        protected override string Convert(OrderType value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                OrderType.MyOrder => "my_order_background",
                OrderType.MyOrderInModeration => "not_moderated_order_background",
                OrderType.NotMyOrder => "not_my_order_background",
                OrderType.MyOrderCompleted => "my_order_completed_background",
                OrderType.NotMyOrderCompleted => "my_order_completed_background",
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
    }
}
