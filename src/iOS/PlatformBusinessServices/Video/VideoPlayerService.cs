using System;
using System.Diagnostics;
using PrankChat.Mobile.Core.BusinessServices;

namespace PrankChat.Mobile.iOS.PlatformBusinessServices.Video
{
    public class VideoPlayerService : IVideoPlayerService, IDisposable
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

        public bool Muted
        {
            get => _player.Muted;
            set => _player.Muted = value;
        }

        public void Play(string uri)
        {
            Player.SetSourceUri(uri);
            Debug.WriteLine("Playing next source: " + uri);
            Player.Play();
        }

        public void Play()
        {
            Player.Play();
        }

        public void Pause()
        {
            Player.Pause();
        }

        public void Stop()
        {
            Player.Stop();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && _player != null)
            {
                _player.Dispose();
            }
        }
    }
}
