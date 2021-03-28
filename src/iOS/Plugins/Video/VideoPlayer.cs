using AVFoundation;
using CoreMedia;
using Foundation;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Data.Enums;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using System;
using System.Reactive.Disposables;

namespace PrankChat.Mobile.iOS.Plugins.Video
{
    public class VideoPlayer : IVideoPlayer
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
                AutomaticallyWaitsToMinimizeStalling = false,
                Muted = true,
            }.DisposeWith(_disposables);

            AVPlayerItem.Notifications.ObserveDidPlayToEndTime(_player, OnVideoEnded)
                .DisposeWith(_disposables);

            _player.AddBoundaryTimeObserver(
                times: new[] { NSValue.FromCMTime(new CMTime(Constants.Delays.VideoPartiallyPlayedDelay, 1)) },
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
            var playerItem = new AVPlayerItem(new NSUrl(url))
            {
                PreferredForwardBufferDuration = 1
            };

            _player.ReplaceCurrentItemWithPlayerItem(playerItem);
        }

        public void Play()
        {
            _player.Play();
            IsPlaying = true;
            VideoPlayingStatusChanged?.Invoke(this, VideoPlayingStatus.Started);
        }

        public void Pause()
        {
            _player.Pause();
            IsPlaying = false;

            VideoPlayingStatusChanged?.Invoke(this, VideoPlayingStatus.Paused);
        }

        public void Stop()
        {
            _player.Pause();
            _ = _player.SeekAsync(CMTime.Zero);

            IsPlaying = false;
            VideoPlayingStatusChanged?.Invoke(this, VideoPlayingStatus.Stopped);
        }

        public object GetNativePlayer() => _player;

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (!isDisposing || _isDisposed)
            {
                return;
            }

            _isDisposed = true;
            _disposables.Dispose();
        }

        private void OnVideoEnded(object sender, NSNotificationEventArgs e)
        {
            VideoPlayingStatusChanged?.Invoke(this, VideoPlayingStatus.Played);
            if (!CanRepeat)
            {
                IsPlaying = false;
                return;
            }

            _player.Seek(new CMTime(0, 1));
        }
    }
}