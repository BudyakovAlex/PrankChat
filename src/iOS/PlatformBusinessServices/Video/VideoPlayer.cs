using System;
using System.Diagnostics;
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
        private int _repeatDelayInSeconds;
        private AVPlayerViewController _currentContainer;
        private NSObject _repeatObserver;
        private NSObject _itemEndHanler;

        public VideoPlayer()
        {
            _player = new AVQueuePlayer();
            _player.AutomaticallyWaitsToMinimizeStalling = true;
            _player.Muted = true;
            _player.ActionAtItemEnd = AVPlayerActionAtItemEnd.None;
        }

        /// <inheritdoc />>
        public bool IsPlaying { get; private set; }

        /// <inheritdoc />>
        public object PlatformPlayerInstance => _player;

        /// <inheritdoc />>
        public bool Muted
        {
            get => _player.Muted;
            set => _player.Muted = value;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />>
        public void EnableRepeat(int repeatDelayInSeconds)
        {
            _repeatDelayInSeconds = repeatDelayInSeconds;
            _repeatObserver = _player.AddBoundaryTimeObserver(
                times: new[] { NSValue.FromCMTime(new CMTime(repeatDelayInSeconds, 1)) },
                queue: null,
                handler: TryRepeatVideo);

            _itemEndHanler = NSNotificationCenter.DefaultCenter.AddObserver(AVPlayerItem.DidPlayToEndTimeNotification, RepeatEndedItem);
        }

        /// <inheritdoc />>
        public void Pause()
        {
            if (!IsPlaying)
                return;

            _player.Pause();
            IsPlaying = false;
        }

        public void Play()
        {
            if (IsPlaying)
                return;

            if (_player.CurrentItem == null)
                return;

            _player.Play();
            IsPlaying = true;
        }

        public void Stop()
        {
            Debug.WriteLine("Play stopped.");
            _player.Seek(new CMTime(0, 1));
            _player.Pause();
            IsPlaying = false;
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

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && _player != null)
            {
                _player.Dispose();

                if (_repeatObserver != null)
                {
                    _player.RemoveTimeObserver(_repeatObserver);
                    _repeatObserver = null;
                }

                if (_itemEndHanler != null)
                {
                    NSNotificationCenter.DefaultCenter.RemoveObserver(_itemEndHanler);
                    _itemEndHanler = null;
                }
            }
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

        private void RepeatEndedItem(NSNotification obj)
        {
            _player.Seek(new CMTime(0, 1));
        }
    }
}
