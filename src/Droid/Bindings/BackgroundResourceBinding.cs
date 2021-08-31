using System;
using Android.Views;
using MvvmCross.Binding;
using MvvmCross.Platforms.Android.Binding.Target;

namespace PrankChat.Mobile.Droid.Bindings
{
    public class BackgroundResourceBinding : MvxAndroidTargetBinding
    {
        public static string TargetBinding = "BackgroundResource";

        public override Type TargetType => typeof(View);

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public BackgroundResourceBinding(object target) : base(target)
        {
        }

        protected override void SetValueImpl(object target, object value)
        {
            if (target is View view && value is int backgroundResource)
            {
                view.SetBackgroundResource(backgroundResource);
            }
        }
    }
}
