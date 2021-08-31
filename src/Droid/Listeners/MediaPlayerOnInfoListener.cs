using System;
using Android.Media;
using Android.Runtime;

namespace PrankChat.Mobile.Droid.Listeners
{
    public class MediaPlayerOnInfoListener : Java.Lang.Object, MediaPlayer.IOnInfoListener
    {
        private readonly Func<MediaPlayer, MediaInfo, int, bool> _onInfoFunc;

        public MediaPlayerOnInfoListener(Func<MediaPlayer, MediaInfo, int, bool> onInfoFunc)
        {
            _onInfoFunc = onInfoFunc;
        }

        public bool OnInfo(MediaPlayer mp, [GeneratedEnum] MediaInfo what, int extra)
        {
            return _onInfoFunc?.Invoke(mp, what, extra) ?? true;
        }
    }
}