using PrankChat.Mobile.Core.BusinessServices;

namespace PrankChat.Mobile.iOS.PlatformBusinessServices.Video
{
    public class VideoPlayerService : IVideoPlayerService
    {
        private const int RepeatDelayInSeconds = 10;
        private IVideoPlayer _player;

        public IVideoPlayer Player
        {
            get
            {
                if (_player == null)
                {
                    _player = new VideoPlayer();
                    _player.EnableRepeat(RepeatDelayInSeconds);
                }
                return _player;
            }
        }

        public void Play(string uri)
        {
            Player.SetSourceUri(uri);
            Player.Play();
        }

        public void Stop()
        {
            Player.Stop();
        }
    }
}
