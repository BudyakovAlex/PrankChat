using System;
using Android.App;
using Android.Graphics;
using AndroidX.Core.Content.Resources;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding;
using MvvmCross.Platforms.Android.Binding.Target;

namespace PrankChat.Mobile.Droid.Bindings
{
    public class ImageViewTintColorTargetBinding : MvxAndroidTargetBinding
    {
        public static string TargetBinding = "ImageViewTintColor";

        public override Type TargetType => typeof(ImageView);

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public ImageViewTintColorTargetBinding(object target) : base(target)
        {
        }

        protected override void SetValueImpl(object target, object value)
        {
            if (target is ImageView imageView && value is int colorResource)
            {
                var colorValue = ResourcesCompat.GetColor(Application.Context.Resources, colorResource, null);
                var color = new Color(colorValue);
                imageView.SetColorFilter(color);
                return;
            }
        }
    }
}