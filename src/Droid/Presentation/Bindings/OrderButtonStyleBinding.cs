using System;
using AndroidX.Core.Content;
using Google.Android.Material.Button;
using MvvmCross.Binding;
using MvvmCross.Platforms.Android.Binding.Target;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Droid.Presentation.Bindings
{
    internal class OrderButtonStyleBinding : MvxAndroidTargetBinding
    {
        public static string TargetBinding = "OrderButtonStyle";

        public override Type TargetType => typeof(MaterialButton);

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public OrderButtonStyleBinding(object target) : base(target)
        {
        }

        protected override void SetValueImpl(object target, object value)
        {
            if (target is MaterialButton view && value is OrderType orderType)
            {
                switch (orderType)
                {
                    case OrderType.MyOrder:
                    case OrderType.NotMyOrder:
                        view.Background = null;
                        view.SetBackgroundResource(Resource.Drawable.button_accent_background);
                        view.SetTextColor(ContextCompat.GetColorStateList(view.Context, Resource.Color.applicationWhite));
                        break;

                    case OrderType.MyOrderInModeration:
                        view.Background = null;
                        view.SetBackgroundResource(Resource.Drawable.button_white_secondary_background);
                        view.SetTextColor(ContextCompat.GetColorStateList(view.Context, Resource.Color.applicationBlack));
                        break;
                }
            }
        }
    }
}
