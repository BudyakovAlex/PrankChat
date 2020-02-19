using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AVFoundation;
using AVKit;
using CoreMedia;
using Foundation;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Infrastructure.Extensions;

namespace PrankChat.Mobile.iOS.PlatformBusinessServices.Video
{
    public class VideoPlayer : IVideoPlayer
    {
        private AVPlayer _player;
        private int _repeatDelayInSeconds;
        private AVPlayerViewController _currentContainer;
        private NSObject _repeatObserver;
        private NSObject _viewedFactRegistrationObserver;
        private NSObject _videoEndHandler;
        private readonly IApiService _apiService;
        private readonly IMvxMessenger _mvxMessenger;

        public VideoPlayer(IApiService apiService, IMvxMessenger mvxMessenger)
        {
            _player = new AVQueuePlayer
            {
                AutomaticallyWaitsToMinimizeStalling = true,
                Muted = true,
                ActionAtItemEnd = AVPlayerActionAtItemEnd.None
            };

            _apiService = apiService;
            _mvxMessenger = mvxMessenger;
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

            _videoEndHandler = NSNotificationCenter.DefaultCenter.AddObserver(AVPlayerItem.DidPlayToEndTimeNotification, RepeatEndedItem);
        }

        public void TryRegisterViewedFact(int id, int registrationDelayInMilliseconds)
        {
            var registrationDelayInSeconds = registrationDelayInMilliseconds / 1000;
            _repeatObserver = _player.AddBoundaryTimeObserver(
                times: new[] { NSValue.FromCMTime(new CMTime(registrationDelayInSeconds, 1)) },
                queue: null,
                handler: () => RegisterViewedVideoFactAsync(id, registrationDelayInSeconds).FireAndForget());
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
            if (disposing)
            {
                if (_repeatObserver != null)
                {
                    _player.RemoveTimeObserver(_repeatObserver);
                    _repeatObserver = null;
                }

                if (_viewedFactRegistrationObserver != null)
                {
                    _player.RemoveTimeObserver(_viewedFactRegistrationObserver);
                    _viewedFactRegistrationObserver.Dispose();
                    _viewedFactRegistrationObserver = null;
                }

                if (_videoEndHandler != null)
                {
                    NSNotificationCenter.DefaultCenter.RemoveObserver(_videoEndHandler);
                    _videoEndHandler.Dispose();
                    _videoEndHandler = null;
                }

                if(_player != null)
                {
                    _player.Dispose();
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

        private async Task RegisterViewedVideoFactAsync(int id, int registrationDelayInSeconds)
        {
            if (_player.CurrentItem.CurrentTime.Value >= registrationDelayInSeconds)
            {
                var views = await  _apiService.RegisterVideoViewedFactAsync(id);

                if (views.HasValue)
                {
                    _mvxMessenger.Publish(new ViewCountMessage(this, id, views.Value));
                }
            }
        }

        private void RepeatEndedItem(NSNotification obj)
        {
            _player.Seek(new CMTime(0, 1));
        }
    }
}
