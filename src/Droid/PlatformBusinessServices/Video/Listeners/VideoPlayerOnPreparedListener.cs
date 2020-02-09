using Android.Media;

namespace PrankChat.Mobile.Droid.PlatformBusinessServices.Video.Listeners
{
    internal class VideoPlayerOnPreparedListener : Java.Lang.Object, MediaPlayer.IOnPreparedListener
    {
        private readonly bool _isLooping;

        public VideoPlayerOnPreparedListener(bool isLooping)
        {
            _isLooping = isLooping;
        }

        public void OnPrepared(MediaPlayer mediaPlayer)
        {
            mediaPlayer.Looping = _isLooping;
        }
    }
}
