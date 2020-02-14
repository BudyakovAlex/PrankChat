using System;
using Android.Media;

namespace PrankChat.Mobile.Droid.Presentation.Listeners
{
    public class MediaPlayerOnPreparedListener : Java.Lang.Object, MediaPlayer.IOnPreparedListener
    {
        private readonly Action<MediaPlayer> mediaPlayerPreparedAction;

        public MediaPlayerOnPreparedListener(Action<MediaPlayer> mediaPlayerPreparedAction)
        {
            this.mediaPlayerPreparedAction = mediaPlayerPreparedAction;
        }

        public void OnPrepared(MediaPlayer mp)
        {
            mediaPlayerPreparedAction?.Invoke(mp);
        }
    }
}
