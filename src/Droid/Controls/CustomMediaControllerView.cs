using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;

namespace PrankChat.Mobile.Droid.Controls
{
    public class CustomMediaControllerView : FrameLayout, MediaController.IMediaPlayerControl
    {
        public int AudioSessionId => throw new NotImplementedException();

        public int BufferPercentage => throw new NotImplementedException();

        public int CurrentPosition => throw new NotImplementedException();

        public int Duration => throw new NotImplementedException();

        public bool IsPlaying => throw new NotImplementedException();

        public CustomMediaControllerView(Context context) : base(context)
        {
        }

        public CustomMediaControllerView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public CustomMediaControllerView(Context context, bool useFastForward) : base(context, useFastForward)
        {
        }

        protected CustomMediaControllerView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        override public bool CanPause()
        {
            throw new NotImplementedException();
        }

        public bool CanSeekBackward()
        {
            throw new NotImplementedException();
        }

        public bool CanSeekForward()
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void SeekTo(int pos)
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }
    }
}
