using System;
using Android.App;
using Android.Graphics;
using AndroidX.Core.Content.Resources;
using Android.Widget;
using MvvmCross.Binding;
using MvvmCross.Platforms.Android.Binding.Target;

namespace PrankChat.Mobile.Droid.Presentation.Bindings
{
    public class TextColorTargetBinding : MvxAndroidTargetBinding<TextView, int>
    {
        public TextColorTargetBinding(TextView target) : base(target)
        {
        }

        protected override void SetValueImpl(TextView target, int value)
        {
            var colorValue = ResourcesCompat.GetColor(Application.Context.Resources, value, null);
            var color = new Color(colorValue);
            target.SetTextColor(color);
        }
    }
}