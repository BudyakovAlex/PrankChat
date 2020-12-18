using System;
using Android.App;
using Android.Graphics;
using Android.Views;
using AndroidX.Core.Content.Resources;
using MvvmCross.Binding;
using MvvmCross.Platforms.Android.Binding.Target;

namespace PrankChat.Mobile.Droid.Presentation.Bindings
{
    public class BackgroundColorBinding : MvxAndroidTargetBinding
    {
        public static string TargetBinding = "BackgroundColor";

        public override Type TargetType => typeof(View);

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public BackgroundColorBinding(object target) : base(target)
        {
        }

        protected override void SetValueImpl(object target, object value)
        {
            if (target is View view && value is int colorResource)
            {
                var colorValue = ResourcesCompat.GetColor(Application.Context.Resources, colorResource, null);
                var color = new Color(colorValue);
                view.SetBackgroundColor(color);
            }
        }
    }
}