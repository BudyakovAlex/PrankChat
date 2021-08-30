using System;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.iOS.Providers;
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
                case OrderType.MyOrder:             return ImageNames.BackgroundOrderTypeMy;
                case OrderType.MyOrderInModeration: return ImageNames.BackgroundOrderTypeMyInModeration;
                case OrderType.NotMyOrder:          return ImageNames.BackgroundOrderTypeNotMy;
                case OrderType.MyOrderCompleted:    return ImageNames.BackgroundOrderTypeMyCompleted;
                case OrderType.NotMyOrderCompleted: return ImageNames.BackgroundOrderTypeMyCompleted;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}
