using AVFoundation;
using AVKit;
using CoreFoundation;
using CoreMedia;
using Foundation;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.BusinessServices.Logger;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PrankChat.Mobile.iOS.PlatformBusinessServices.Video
{
    public class VideoPlayer : BaseVideoPlayer
    {
        private readonly AVPlayer _player;

        private int _repeatDelayInSeconds;
        private AVPlayerViewController _currentContainer;
        private NSObject _repeatObserver;
        private NSObject _viewedFactRegistrationObserver;
        private NSObject _videoEndHandler;
        private NSObject _playerPerdiodicTimeObserver;

        private int _id;
        private string _uri;

        public VideoPlayer(IApiService apiService, ILogger logger, IMvxMessenger mvxMessenger) : base(apiService, logger, mvxMessenger)
        {
            _player = new AVQueuePlayer
            {
                AutomaticallyWaitsToMinimizeStalling = true,
                Muted = true,
                ActionAtItemEnd = AVPlayerActionAtItemEnd.None
            };
        }

        /// <inheritdoc />>
        public override bool IsPlaying { get; protected set; }

        /// <inheritdoc />>
        public override bool Muted
        {
            get => _player.Muted;
            set => _player.Muted = value;
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />>
        public override void EnableRepeat(int repeatDelayInSeconds)
        {
            _repeatDelayInSeconds = repeatDelayInSeconds;
            _repeatObserver = _player.AddBoundaryTimeObserver(
                times: new[] { NSValue.FromCMTime(new CMTime(repeatDelayInSeconds, 1)) },
                queue: null,
                handler: TryRepeatVideo);

            _videoEndHandler = NSNotificationCenter.DefaultCenter.AddObserver(AVPlayerItem.DidPlayToEndTimeNotification, RepeatEndedItem);
        }

        public override void TryRegisterViewedFact(int id, int registrationDelayInMilliseconds)
        {
            var registrationDelayInSeconds = registrationDelayInMilliseconds / 1000;

            if (_viewedFactRegistrationObserver != null)
                RemoveViewedFactRegistrationObserver();

            _viewedFactRegistrationObserver = _player.AddBoundaryTimeObserver(
                times: new[] { NSValue.FromCMTime(new CMTime(registrationDelayInSeconds, 1)) },
                queue: null,
                handler: () => RegisterViewedVideoFactAsync(id, registrationDelayInSeconds).FireAndForget());
        }

        /// <inheritdoc />>
        public override void Pause()
        {
            if (!IsPlaying)
                return;

            _player.Pause();
            IsPlaying = false;
        }

        public override void Play()
        {
            if (IsPlaying)
                return;

            if (_player.CurrentItem == null)
                return;

            Logger.LogEventAsync(DateTime.Now, "[Video_Buffering]", $"Video uri is {_uri}, video ID is {_id}").FireAndForget();

            _player.Play();
            IsPlaying = true;
        }

        public override void Stop()
        {
            Logger.LogEventAsync(DateTime.Now, "[Video_Stop]", $"Video uri is {_uri}").FireAndForget();

            Debug.WriteLine("Play stopped.");
            _player.Seek(new CMTime(0, 1));
            _player.Pause();
            IsPlaying = false;
        }

        public override void SetPlatformVideoPlayerContainer(object container)
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

        public override void SetSourceUri(string uri, int id )
        {
            if (string.IsNullOrEmpty(uri))
            {
                return;
            }

            _id = id;
            _uri = uri;
            _playerPerdiodicTimeObserver = _player.AddPeriodicTimeObserver(
                new CMTime(1, 2),
                DispatchQueue.MainQueue,
                PlayerTimeChanged);

            Logger.LogEventAsync(DateTime.Now, "[Video_Initialization]", $"Video uri is {_uri}, video ID is {_id}").FireAndForget();
            _player.ReplaceCurrentItemWithPlayerItem(new AVPlayerItem(new NSUrl(uri)));
        }

        private void PlayerTimeChanged(CMTime obj)
        {
            if (_playerPerdiodicTimeObserver is null)
            {
                return;
            }

            if (obj.Value > 0)
            {
                if (_playerPerdiodicTimeObserver != null)
                {
                    Logger.LogEventAsync(DateTime.Now, "[Video_Play]", $"Video uri is {_uri}, video ID is {_id}").FireAndForget();
                    _player.RemoveTimeObserver(_playerPerdiodicTimeObserver);
                    _playerPerdiodicTimeObserver = null;
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_repeatObserver != null)
                {
                    _player?.RemoveTimeObserver(_repeatObserver);
                    _repeatObserver = null;
                }

                RemoveViewedFactRegistrationObserver();

                if (_videoEndHandler != null)
                {
                    NSNotificationCenter.DefaultCenter.RemoveObserver(_videoEndHandler);
                    _videoEndHandler?.Dispose();
                    _videoEndHandler = null;
                }

                _player?.Dispose();
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

        private async Task RegisterViewedVideoFactAsync(int id, int registrationDelayInMilliseconds)
        {
            var currentTimeInMilliseconds = (int)_player.CurrentItem.CurrentTime.Seconds * 1000;
            if (currentTimeInMilliseconds >= registrationDelayInMilliseconds)
            {
                await SendRegisterViewedFactAsync(id, registrationDelayInMilliseconds, currentTimeInMilliseconds);
                RemoveViewedFactRegistrationObserver();
            }
        }

        private void RepeatEndedItem(NSNotification obj)
        {
            _player.Seek(new CMTime(0, 1));
        }

        private void RemoveViewedFactRegistrationObserver()
        {
            if (_viewedFactRegistrationObserver == null)
            {
                return;
            }

            _player?.RemoveTimeObserver(_viewedFactRegistrationObserver);
            _viewedFactRegistrationObserver?.Dispose();
            _viewedFactRegistrationObserver = null;
        }
    }
}
