using System;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.iOS.AppTheme;
using UIKit;

namespace PrankChat.Mobile.iOS.Binding
{
    public class UIButtonOrderTypeTargetBinding : MvxTargetBinding<UIButton, OrderType>
    {
        public static string TargetBinding = "ButtonOrderType";

        public UIButtonOrderTypeTargetBinding(UIButton target)
            : base(target)
        {
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        protected override void SetValue(OrderType value)
        {
            switch (value)
            {
                case OrderType.MyOrder:
                case OrderType.MyOrderInModeration:
                case OrderType.MyOrderCompleted:
                case OrderType.NotMyOrderCompleted:
                    Target.BackgroundColor = Theme.Color.White;
                    Target.SetTitleColor(Theme.Color.Black, UIControlState.Normal);
                    Target.Layer.CornerRadius = 4f;
                    Target.Layer.BorderWidth = 1f;
                    Target.Layer.BorderColor = Theme.Color.DarkOrange.CGColor;
                    break;

                case OrderType.NotMyOrder:
                    Target.BackgroundColor = Theme.Color.Accent;
                    Target.SetTitleColor(Theme.Color.White, UIControlState.Normal);
                    Target.Layer.CornerRadius = 4f;
                    Target.Layer.BorderWidth = 1f;
                    Target.Layer.BorderColor = Theme.Color.AccentDark.CGColor;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
