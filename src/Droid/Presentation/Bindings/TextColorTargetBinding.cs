using System;
using Android.App;
using Android.Graphics;
using AndroidX.Core.Content.Resources;
using Android.Widget;
using MvvmCross.Binding;
using MvvmCross.Platforms.Android.Binding.Target;

namespace PrankChat.Mobile.Droid.Presentation.Bindings
{
    public class TextColorTargetBinding : MvxAndroidTargetBinding
    {
        public static string TargetBinding { get; } = "TextColor";

        public override Type TargetType => typeof(TextView);

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public TextColorTargetBinding(object target) : base(target)
        {
        }

        protected override void SetValueImpl(object target, object value)
        {
            if (target is TextView textView && value is int colorResource)
            {
                var colorValue = ResourcesCompat.GetColor(Application.Context.Resources, colorResource, null);
                var color = new Color(colorValue);
                textView.SetTextColor(color);
                return;
            }
        }
    }
}