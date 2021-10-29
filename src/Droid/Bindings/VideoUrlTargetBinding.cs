using System;
using Android.Widget;
using MvvmCross.Binding;
using MvvmCross.Platforms.Android.Binding.Target;

namespace PrankChat.Mobile.Droid.Bindings
{
    public class VideoUrlTargetBinding : MvxAndroidTargetBinding<VideoView, string>
    {
        public VideoUrlTargetBinding(VideoView target) : base(target)
        {
        }

        protected override void SetValueImpl(VideoView target, string value)
        {
            target.SetVideoPath(value);
            target.Start();
        }
    }
}
