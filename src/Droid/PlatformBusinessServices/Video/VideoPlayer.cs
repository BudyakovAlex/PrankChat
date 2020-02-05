using Android.Net;
using Android.Widget;
using PrankChat.Mobile.Core.BusinessServices;

namespace PrankChat.Mobile.Droid.PlatformBusinessServices.Video
{
    public class VideoPlayer : IVideoPlayer
    {
        private VideoView _videoView;

        public bool IsPlaying { get; private set; }

        public bool Muted { get; set; }

        public void Dispose()
        {
        }

        public void EnableRepeat(int repeatDelayInSeconds)
        {
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
            if (container is VideoView videoView)
                _videoView = videoView;
            else
                _videoView = null;
        }

        public void SetSourceUri(string uri)
        {
            _videoView.SetVideoURI(Uri.Parse(uri));
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
