using System;
using Android.Views;
using MvvmCross.Binding;
using MvvmCross.Platforms.Android.Binding.Target;

namespace PrankChat.Mobile.Droid.Presentation.Bindings
{
    internal class BackgroundBinding : MvxAndroidTargetBinding
    {
        public static string PropertyName = "Background";

        public BackgroundBinding(object target) : base(target)
        {
        }

        public override Type TargetType => typeof(View);

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        protected override void SetValueImpl(object target, object value)
        {
            if (target is View view && value is string drawableName)
            {
                var res = (int)typeof(Resource.Drawable).GetField(drawableName).GetValue(null);
                view.SetBackgroundResource(res);
            }
        }
    }
}
