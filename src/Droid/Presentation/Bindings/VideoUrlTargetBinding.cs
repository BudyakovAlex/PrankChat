using System;
using Android.Widget;
using MvvmCross.Binding;
using MvvmCross.Platforms.Android.Binding.Target;

namespace PrankChat.Mobile.Droid.Presentation.Bindings
{
    public class VideoUrlTargetBinding : MvxAndroidTargetBinding
    {
        public static string TargetBinding { get; } = "VideoUrl";

        public VideoUrlTargetBinding(object target) : base(target)
        {
        }

        public override Type TargetType => typeof(VideoView);

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        protected override void SetValueImpl(object target, object value)
        {
            if (target is VideoView videoView &&
                value is string url)
            {
                videoView.SetVideoPath(url);
                videoView.Start();
            }
        }
    }
}
