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
    public class ImageViewTintColorTargetBinding : MvxAndroidTargetBinding<ImageView, int>
    {
        public ImageViewTintColorTargetBinding(ImageView target) : base(target)
        {
        }

        protected override void SetValueImpl(ImageView target, int value)
        {
            var colorValue = ResourcesCompat.GetColor(Application.Context.Resources, value, null);
            var color = new Color(colorValue);
            target.SetColorFilter(color);
        }
    }
}