using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;

namespace PrankChat.Mobile.Droid.Controls
{
    public class ExtendedVideoView : VideoView
    {
        private IOnVideoViewStateChangedListener _onVideoViewStateChangedListener;

        public ExtendedVideoView(Context context) : base(context)
        {
        }

        public ExtendedVideoView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public ExtendedVideoView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public ExtendedVideoView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        protected ExtendedVideoView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public void SetOnVideoViewStateChangedListener(IOnVideoViewStateChangedListener listener)
        {
            _onVideoViewStateChangedListener = listener;
        }

        public override void Pause()
        {
            base.Pause();
            _onVideoViewStateChangedListener?.Pause();
        }

        public override void Start()
        {
            base.Start();
            _onVideoViewStateChangedListener?.Start();
        }

        public override void StopPlayback()
        {
            base.StopPlayback();
            _onVideoViewStateChangedListener?.StopPlayback();
        }

        public interface IOnVideoViewStateChangedListener
        {
            void Pause();

            void Start();

            void StopPlayback();
        }
    }
}