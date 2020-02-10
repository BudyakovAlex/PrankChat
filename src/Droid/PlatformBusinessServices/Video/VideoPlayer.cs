using Android.Widget;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Droid.PlatformBusinessServices.Video.Listeners;

namespace PrankChat.Mobile.Droid.PlatformBusinessServices.Video
{
    public class VideoPlayer : IVideoPlayer
    {
        private VideoView _videoView;
        private bool _isRepeatEnabled;

        public bool IsPlaying { get; private set; }

        public bool Muted { get; set; }

        public void Dispose()
        {
            _videoView?.SetOnPreparedListener(null);
        }

        public void EnableRepeat(int repeatDelayInSeconds)
        {
            _isRepeatEnabled = true;
        }

        public void Pause()
        {
            if (!IsPlaying || _videoView == null)
                return;

            _videoView.Pause();
            IsPlaying = false;
        }

        public void Play()
        {
            if (IsPlaying || _videoView == null)
                return;

            _videoView.Start();
            IsPlaying = true;
        }

        public void SetPlatformVideoPlayerContainer(object container)
        {
            if (_videoView == container)
                return;

            if (container is VideoView videoView)
            {
                _videoView = videoView;
            }
            else
            {
                _videoView = null;
            }
        }

        public void SetSourceUri(string uri)
        {
            _videoView.SetVideoPath(uri);
            _videoView.SetOnPreparedListener(new VideoPlayerOnPreparedListener(_isRepeatEnabled));
        }

        public void Stop()
        {
            if (!IsPlaying || _videoView == null)
                return;

            _videoView?.StopPlayback();
            IsPlaying = false;
        }
    }
}
