using System;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using PrankChat.Mobile.Core.Models.Enums;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Binding
{
    public class UIImageViewOrderTypeTargetBinding : MvxTargetBinding<UIImageView, OrderType>
    {
        public static string TargetBinding = "ImageViewOrderType";

        public UIImageViewOrderTypeTargetBinding(UIImageView target)
            : base(target)
        {
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        protected override void SetValue(OrderType value)
        {
            var imageName = GetImageName(value);
            Target.Image = UIImage.FromBundle(imageName);
        }

        private string GetImageName(OrderType orderType)
        {
            switch (orderType)
            {
                case OrderType.MyOrder:             return "bg_order_type_my";
                case OrderType.MyOrderInModeration: return "bg_order_type_my_in_moderation";
                case OrderType.NotMyOrder:          return "bg_order_type_not_my";
                case OrderType.MyOrderCompleted:    return "bg_order_type_my_completed";
                case OrderType.NotMyOrderCompleted: return "bg_order_type_my_completed";
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}
