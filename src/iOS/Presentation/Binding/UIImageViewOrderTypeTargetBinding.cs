using System;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using PrankChat.Mobile.Core.Models.Enums;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Binding
{
    public class UIImageViewOrderTypeTargetBinding : MvxTargetBinding<UIImageView, OrderType>
    {
        public static string TargetBinding = "UIImageViewOrderType";

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
                case OrderType.MyOrder:             return "OrderTypeMyBackground";
                case OrderType.MyOrderInModeration: return "OrderTypeMyInModerationBackground";
                case OrderType.NotMyOrder:          return "OrderTypeNotMyBackground";
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}