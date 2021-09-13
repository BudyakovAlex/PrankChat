using System;
using Android.Views;
using MvvmCross.Binding;
using MvvmCross.Platforms.Android.Binding.Target;

namespace PrankChat.Mobile.Droid.Presentation.Bindings
{
    public class BackgroundDrawableBinding : MvxAndroidTargetBinding<View, string>
    {
        public BackgroundDrawableBinding(View target) : base(target)
        {
        }

        protected override void SetValueImpl(View target, string value)
        {
            var res = (int)typeof(Resource.Drawable).GetField(value).GetValue(null);
            target.SetBackgroundResource(res);
        }   
    }
}
