using System;
using Android.App;
using Android.Graphics;
using Android.Views;
using AndroidX.Core.Content.Resources;
using MvvmCross.Binding;
using MvvmCross.Platforms.Android.Binding.Target;

namespace PrankChat.Mobile.Droid.Bindings
{
    public class BackgroundColorBinding : MvxAndroidTargetBinding<View, int>
    {
        public BackgroundColorBinding(View target) : base(target)
        {
        }

        protected override void SetValueImpl(View target, int value)
        {
            var colorValue = ResourcesCompat.GetColor(Application.Context.Resources, value, null);
            var color = new Color(colorValue);
            target.SetBackgroundColor(color);
        }
    }
}