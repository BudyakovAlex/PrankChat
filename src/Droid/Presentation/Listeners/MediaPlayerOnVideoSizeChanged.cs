using System;
using Android.Media;

namespace PrankChat.Mobile.Droid.Presentation.Listeners
{
    public class MediaPlayerOnVideoSizeChanged : Java.Lang.Object, MediaPlayer.IOnVideoSizeChangedListener
    {
        private readonly Action<MediaPlayer, int, int> _onSizeChnagedAction;

        public MediaPlayerOnVideoSizeChanged(Action<MediaPlayer, int, int> onSizeChnagedAction)
        {
            _onSizeChnagedAction = onSizeChnagedAction;
        }

        public void OnVideoSizeChanged(MediaPlayer mp, int width, int height)
        {
            _onSizeChnagedAction?.Invoke(mp, width, height);
        }
    }
}
