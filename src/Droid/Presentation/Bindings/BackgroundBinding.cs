using System;
using Android.Views;
using MvvmCross.Binding;
using MvvmCross.Platforms.Android.Binding.Target;

namespace PrankChat.Mobile.Droid.Presentation.Bindings
{
    public class BackgroundBinding : MvxAndroidTargetBinding<View, int>
    {
        public BackgroundBinding(View target) : base(target)
        {
        }

        protected override void SetValueImpl(View target, int value)
            => target.SetBackgroundResource(value);
    }
}
