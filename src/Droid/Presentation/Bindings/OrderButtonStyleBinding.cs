using AndroidX.AppCompat.Widget;
using AndroidX.Core.Content;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Droid.Presentation.Bindings
{
    internal class OrderButtonStyleBinding : MvxTargetBinding<AppCompatButton, OrderType>
    {
        public static string TargetBinding = "OrderButtonStyle";

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public OrderButtonStyleBinding(AppCompatButton target) : base(target)
        {
        }

        protected override void SetValue(OrderType value)
        {
            switch (value)
            {
                case OrderType.MyOrder:
                case OrderType.NotMyOrder:
                    Target.SetBackgroundResource(Resource.Drawable.button_accent_background);
                    Target.SetTextColor(ContextCompat.GetColorStateList(Target.Context, Resource.Color.applicationWhite));
                    break;

                case OrderType.MyOrderInModeration:
                    Target.SetBackgroundResource(Resource.Drawable.button_white_secondary_background);
                    Target.SetTextColor(ContextCompat.GetColorStateList(Target.Context, Resource.Color.applicationBlack));
                    break;
            }
        }
    }
}