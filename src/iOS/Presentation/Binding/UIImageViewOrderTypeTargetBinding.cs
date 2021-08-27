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
                case OrderType.MyOrder:             return ImagePathProvider.BackgroundOrderTypeMy;
                case OrderType.MyOrderInModeration: return ImagePathProvider.BackgroundOrderTypeMyInModeration;
                case OrderType.NotMyOrder:          return ImagePathProvider.BackgroundOrderTypeNotMy;
                case OrderType.MyOrderCompleted:    return ImagePathProvider.BackgroundOrderTypeMyCompleted;
                case OrderType.NotMyOrderCompleted: return ImagePathProvider.BackgroundOrderTypeMyCompleted;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}
