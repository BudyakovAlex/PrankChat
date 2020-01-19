using System;
using System.Diagnostics;
using System.Linq;
using AVFoundation;
using AVKit;
using CoreMedia;
using Foundation;
using PrankChat.Mobile.Core.BusinessServices;

namespace PrankChat.Mobile.iOS.PlatformBusinessServices
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
            _repeatDelayInSeconds = repeatDelayInSeconds;
            _repeadEnabled = true;
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

            //// Initialize looper for player that will repeat first 10 seconds of video in a loop.
            //if ((_looper == null && _repeadEnabled) ||
            //    (_repeadEnabled && !_looper.LoopingPlayerItems.Contains(_player.CurrentItem)))
            //    _looper =
            //        new AVPlayerLooper(_player, _player.CurrentItem,
            //            new CMTimeRange
            //            {
            //                Start = new CMTime(0, 1),
            //                Duration = new CMTime(_repeatDelayInSeconds, 1)
            //            });
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
    }
}
