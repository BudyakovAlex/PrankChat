using System;
using System.Diagnostics;
using System.Linq;
using AVFoundation;
using AVKit;
using CoreMedia;
using Foundation;
using PrankChat.Mobile.Core.BusinessServices;

namespace PrankChat.Mobile.iOS.PlatformBusinessServices.Video
{
    public class VideoPlayer : IVideoPlayer
    {
        private AVPlayer _player;
        private AVPlayerLooper _looper;
        private int _repeatDelayInSeconds;
        private bool _repeadEnabled;
        private string _currentUri;
        private AVPlayerViewController _currentContainer;

        public VideoPlayer()
        {
            _player = new AVQueuePlayer();
            _player.AutomaticallyWaitsToMinimizeStalling = true;
        }

        public bool IsPlaying { get; private set; }

        public object PlatformPlayerInstance => _player;

        public void EnableRepeat(int repeatDelayInSeconds)
        {
            var observer = new VideoPlayerStatusObserver(_player, repeatDelayInSeconds);
            _player.AddBoundaryTimeObserver(
                times: new[] { NSValue.FromCMTime(new CMTime(repeatDelayInSeconds, 1)) },
                queue: null,
                handler: TryRepeatVideo);
        }

        public void Play()
        {
            if (IsPlaying)
                return;

            _player.Play();
            IsPlaying = true;
        }

        public void SetPlatformVideoPlayerContainer(object container)
        {
            if (container is AVPlayerViewController viewController)
            {
                if (_currentContainer != null)
                {
                    _currentContainer.Player = null;
                    _currentContainer = null;
                }

                _currentContainer = viewController;
                _currentContainer.Player = _player;
            }
        }

        public void SetSourceUri(string uri)
        {
            _player.ReplaceCurrentItemWithPlayerItem(new AVPlayerItem(new NSUrl(uri))); 
        }

        public void Stop()
        {
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
            {
                Debug.WriteLine("Play stopped.");
                _player.Seek(new CMTime(0, 1));
                _player.Pause();
                IsPlaying = false;
            });
        }

        private void TryRepeatVideo()
        {
            var timeValue = _player.CurrentItem.CurrentTime;
            var currentTimePosition = timeValue.Seconds;

            if (currentTimePosition >= _repeatDelayInSeconds)
            {
                _player.Seek(new CMTime(0, 1));
            }
        }
    }
}
