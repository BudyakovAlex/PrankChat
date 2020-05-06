using System;
using Android.Media;
using Android.Runtime;

namespace PrankChat.Mobile.Droid.Presentation.Listeners
{
    public class MediaPlayerOnErrorListener : Java.Lang.Object, MediaPlayer.IOnErrorListener
    {
        private readonly Func<MediaPlayer, MediaError, int, bool> _errrorAction;

        public MediaPlayerOnErrorListener(Func<MediaPlayer, MediaError, int, bool> errrorAction)
        {
            _errrorAction = errrorAction;
        }

        public bool OnError(MediaPlayer mp, [GeneratedEnum] MediaError what, int extra)
        {
            return _errrorAction?.Invoke(mp, what, extra) ?? true;
        }
    }
}