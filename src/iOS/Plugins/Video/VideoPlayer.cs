﻿using Foundation;
using LibVLCSharp.Shared;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Data.Enums;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.iOS.Plugins.Video
{
    public class VideoPlayer : NSObject, IVideoPlayer
    {
        private CompositeDisposable _disposables;
        private LibVLCSharp.Platforms.iOS.VideoView _player;

        private LibVLC _libVLC;

        private bool _isDisposed;
        private bool _shouldNotifyPartiallyPlayed;

        public VideoPlayer()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                _disposables = new CompositeDisposable();
                _player = new LibVLCSharp.Platforms.iOS.VideoView
                {
                    TranslatesAutoresizingMaskIntoConstraints = false,
                    UserInteractionEnabled = true,
                };

                _libVLC = new LibVLC();
                _player.MediaPlayer = new LibVLCSharp.Shared.MediaPlayer(_libVLC);
                _player.MediaPlayer.SubscribeToEvent<LibVLCSharp.Shared.MediaPlayer, MediaPlayerTimeChangedEventArgs>(OnTimeChanged,
                    (wrapper, handler) => wrapper.TimeChanged += handler,
                    (wrapper, handler) => wrapper.TimeChanged -= handler).DisposeWith(_disposables);

                _player.MediaPlayer.SubscribeToEvent<LibVLCSharp.Shared.MediaPlayer, EventArgs>(OnPlaying,
                      (wrapper, handler) => wrapper.Playing += handler,
                      (wrapper, handler) => wrapper.Playing -= handler).DisposeWith(_disposables);

                _player.MediaPlayer.SubscribeToEvent<LibVLCSharp.Shared.MediaPlayer, EventArgs>(OnEndReached,
                      (wrapper, handler) => wrapper.EndReached += handler,
                      (wrapper, handler) => wrapper.EndReached -= handler).DisposeWith(_disposables);
            });
        }

        public event EventHandler<VideoPlayingStatus> VideoPlayingStatusChanged;

        public bool IsPlaying { get; private set; }

        public bool IsMuted
        {
            get => _player.MediaPlayer.Mute;
            set => MainThread.BeginInvokeOnMainThread(() => _player.MediaPlayer.Mute = value);
        }

        public bool CanRepeat { get; set; }

        public Action ReadyToPlayAction { get; set; }

        public void SetVideoUrl(string url)
        {
            if (url.IsNullOrEmpty())
            {
                return;
            }

            MainThread.BeginInvokeOnMainThread(() =>
            {
                var mediaOptions = new string[] { "input-repeat=-1" };
                _player.MediaPlayer.Media = new Media(_libVLC, url, FromType.FromLocation, mediaOptions);
            });
        }

        public void Play()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                _shouldNotifyPartiallyPlayed = true;
                _player.MediaPlayer.Play();
                IsPlaying = true;
            });
        }

        public void Pause()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                _player.MediaPlayer.Pause();
                IsPlaying = false;
                VideoPlayingStatusChanged?.Invoke(this, VideoPlayingStatus.Paused);
            });
        }

        public void Stop()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                _player.MediaPlayer.Stop();
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
        }


        private void OnEndReached(object sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                VideoPlayingStatusChanged?.Invoke(this, VideoPlayingStatus.Played);
                if (!CanRepeat)
                {
                    return;
                }

                _shouldNotifyPartiallyPlayed = true;
                await Task.Run(() => _player.MediaPlayer.Stop());
                _player.MediaPlayer.Play();
            });
        }

        private async void OnPlaying(object sender, EventArgs e)
        {
            await Task.Delay(1000);
            if (ReadyToPlayAction is null)
            {
                return;
            }

            MainThread.BeginInvokeOnMainThread(ReadyToPlayAction);
        }

        private void OnTimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            if (e.Time >= Constants.Delays.VideoPartiallyPlayedDelay && _shouldNotifyPartiallyPlayed)
            {
                _shouldNotifyPartiallyPlayed = false;
                VideoPlayingStatusChanged?.Invoke(this, VideoPlayingStatus.PartiallyPlayed);
            }
        }
    }
}