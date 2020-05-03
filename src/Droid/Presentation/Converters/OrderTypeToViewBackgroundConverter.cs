﻿using System;
using System.Globalization;
using MvvmCross.Converters;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Droid.Presentation.Converters
{
    public class OrderTypeToViewBackgroundConverter : MvxValueConverter<OrderType, string>
    {
        protected override string Convert(OrderType value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case OrderType.MyOrder:             return "my_order_background";
                case OrderType.MyOrderInModeration: return "not_moderated_order_background";
                case OrderType.NotMyOrder:          return "not_my_order_background";
                case OrderType.MyOrderCompleted:    return "my_order_completed_background";
                case OrderType.NotMyOrderCompleted: return "not_moderated_order_card";
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}
