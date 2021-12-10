using Foundation;
using LibVLCSharp.Shared;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Data.Enums;
using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Xamarin.Essentials;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Common;

namespace PrankChat.Mobile.iOS.Plugins.Video
{
    public static class Vlc
    {
        private static LibVLC _library;

        public static LibVLC GetOrCreateLibraryRecursively()
        {
            try
            {
                if (_library is null ||
                    _library.NativeReference == IntPtr.Zero)
                {
                    _library?.Dispose();
                    _library = null;
                    _library = new LibVLC();
                }

                return _library;
            }
            catch
            {
                return GetOrCreateLibraryRecursively();
            }
        }

        public static LibVLCSharp.Platforms.iOS.VideoView CreateAndConfigureVideoViewRecursively(Func<LibVLC> getOrCreateVlc)
        {
            try
            {
                var videoView = new LibVLCSharp.Platforms.iOS.VideoView
                {
                    TranslatesAutoresizingMaskIntoConstraints = false,
                    UserInteractionEnabled = true,
                };

                var libVlc = getOrCreateVlc.Invoke();
                videoView.MediaPlayer = new LibVLCSharp.Shared.MediaPlayer(libVlc);
                return videoView;
            }
            catch
            {
                return CreateAndConfigureVideoViewRecursively(getOrCreateVlc);
            }
        }
    }

    public class VideoPlayer : NSObject, IVideoPlayer
    {
        private CompositeDisposable _disposables;
        private LibVLCSharp.Platforms.iOS.VideoView _player;

        private LibVLC _libVlc;

        private bool _isDisposed;
        private bool _shouldNotifyPartiallyPlayed;

        public VideoPlayer()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                _libVlc = Vlc.GetOrCreateLibraryRecursively();
                _disposables = new CompositeDisposable();
                _player = Vlc.CreateAndConfigureVideoViewRecursively(() =>
                {
                    _libVlc = Vlc.GetOrCreateLibraryRecursively();
                    return _libVlc;
                });

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
                _player.MediaPlayer.Media = new Media(_libVlc, url, FromType.FromLocation, mediaOptions);
            });
        }

        public void Play()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                System.Diagnostics.Debug.WriteLine($"Play before {_player?.MediaPlayer?.State}");
                _shouldNotifyPartiallyPlayed = true;
                _player?.MediaPlayer?.Play();
                IsPlaying = true;
                System.Diagnostics.Debug.WriteLine($"Play after {_player?.MediaPlayer?.State}");
            });
        }

        public void Pause()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                System.Diagnostics.Debug.WriteLine($"Pause before {_player?.MediaPlayer?.State}");
                _player?.MediaPlayer?.Pause();
                IsPlaying = false;
                VideoPlayingStatusChanged?.Invoke(this, VideoPlayingStatus.Paused);
                System.Diagnostics.Debug.WriteLine($"Pause after {_player?.MediaPlayer?.State}");
            });
        }

        public void Stop()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                System.Diagnostics.Debug.WriteLine($"Stop before {_player?.MediaPlayer?.State}");
                var playerState = _player?.MediaPlayer?.State;
                if (playerState != null && playerState == VLCState.Stopped)
                {
                    System.Diagnostics.Debug.WriteLine($"Stop return {_player?.MediaPlayer?.State}");
                    return;
                }

                _player?.MediaPlayer?.Stop();
                IsPlaying = false;
                System.Diagnostics.Debug.WriteLine($"Stop after {_player?.MediaPlayer?.State}");
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
            _player?.MediaPlayer?.Dispose();
            _player?.Dispose();
            _player = null;
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