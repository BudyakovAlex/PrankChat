using System;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.iOS.Common;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Binding
{
    public class UIImageViewOrderTypeTargetBinding : MvxTargetBinding<UIImageView, OrderType>
    {
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

        private string GetImageName(OrderType orderType) => orderType switch
        {
            OrderType.MyOrder => ImageNames.BackgroundOrderTypeMy,
            OrderType.MyOrderInModeration => ImageNames.BackgroundOrderTypeMyInModeration,
            OrderType.NotMyOrder => ImageNames.BackgroundOrderTypeNotMy,
            OrderType.MyOrderCompleted => ImageNames.BackgroundOrderTypeMyCompleted,
            OrderType.NotMyOrderCompleted => ImageNames.BackgroundOrderTypeMyCompleted,
            _ => throw new ArgumentOutOfRangeException(),
        };
    }
}
