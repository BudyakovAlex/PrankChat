using AVFoundation;
using CoreMedia;
using Foundation;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Data.Enums;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using System;
using System.Reactive.Disposables;
using Xamarin.Essentials;

namespace PrankChat.Mobile.iOS.Plugins.Video
{
    public class VideoPlayer : NSObject, IVideoPlayer
    {
        private readonly CompositeDisposable _disposables;
        private readonly AVPlayer _player;

        private bool _isDisposed;

        public VideoPlayer()
        {
            _disposables = new CompositeDisposable();
            _player = new AVPlayer
            {
                ActionAtItemEnd = AVPlayerActionAtItemEnd.None,
                Muted = true,
            }.DisposeWith(_disposables);

            _player.AddBoundaryTimeObserver(
                times: new[] { NSValue.FromCMTime(CMTime.FromSeconds(Constants.Delays.VideoPartiallyPlayedDelay, 1)) },
                queue: null,
                handler: () => VideoPlayingStatusChanged?.Invoke(this, VideoPlayingStatus.PartiallyPlayed))
                .DisposeWith(_disposables);
        }

        public event EventHandler<VideoPlayingStatus> VideoPlayingStatusChanged;

        public bool IsPlaying { get; private set; }

        public bool IsMuted
        {
            get => _player.Muted;
            set => _player.Muted = value;
        }

        public bool CanRepeat { get; set; }

        public void SetVideoUrl(string url)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var playerItem = AVPlayerItem.FromUrl(NSUrl.FromString(url));
                _player.ReplaceCurrentItemWithPlayerItem(playerItem);

                AVPlayerItem.Notifications.ObserveDidPlayToEndTime(playerItem, OnVideoEnded)
                    .DisposeWith(_disposables);
            });
        }

        public void Play()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (_player.CurrentItem is null)
                {
                    return;
                }

                _player.Play();
                IsPlaying = true;
                VideoPlayingStatusChanged?.Invoke(this, VideoPlayingStatus.Started);
            });
        }

        public void Pause()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (_player.CurrentItem is null)
                {
                    return;
                }

                _player.Pause();
                IsPlaying = false;

                VideoPlayingStatusChanged?.Invoke(this, VideoPlayingStatus.Paused);
            });
        }

        public void Stop()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (_player.CurrentItem is null)
                {
                    return;
                }

                _player.Pause();
                _player.Seek(new CMTime(0, 1));

                IsPlaying = false;
                VideoPlayingStatusChanged?.Invoke(this, VideoPlayingStatus.Stopped);
            });
        }

        public object GetNativePlayer() => _player;

        protected override void Dispose(bool isDisposing)
        {
            if (!isDisposing || _isDisposed)
            {
                return;
            }

            _isDisposed = true;
            _disposables.Dispose();
            _player?.ReplaceCurrentItemWithPlayerItem(null);
        }

        private void OnVideoEnded(object sender, NSNotificationEventArgs e)
        {
            VideoPlayingStatusChanged?.Invoke(this, VideoPlayingStatus.Stopped);
            if (!CanRepeat)
            {
                IsPlaying = false;
                return;
            }

            _player.Seek(new CMTime(0, 1));
        }
    }
}