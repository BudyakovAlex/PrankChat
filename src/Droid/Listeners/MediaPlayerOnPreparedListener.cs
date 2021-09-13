using System;
using Android.Media;

namespace PrankChat.Mobile.Droid.Listeners
{
    public class MediaPlayerOnPreparedListener : Java.Lang.Object, MediaPlayer.IOnPreparedListener
    {
        private readonly Action<MediaPlayer> _mediaPlayerPreparedAction;

        public MediaPlayerOnPreparedListener(Action<MediaPlayer> mediaPlayerPreparedAction)
        {
            _mediaPlayerPreparedAction = mediaPlayerPreparedAction;
        }

        public void OnPrepared(MediaPlayer mp)
        {
            _mediaPlayerPreparedAction?.Invoke(mp);
        }
    }
}
