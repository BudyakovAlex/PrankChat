using Android.App;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Microsoft.AppCenter.Crashes;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Data.Enums;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Wrappers;
using PrankChat.Mobile.Droid.Controls;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Droid.PlatformBusinessServices.Video
{
    public class VideoPlayer : Java.Lang.Object,
        IVideoPlayer,
        MediaPlayer.IOnPreparedListener,
        MediaPlayer.IOnVideoSizeChangedListener,
        MediaPlayer.IOnErrorListener,
        MediaPlayer.IOnInfoListener,
        MediaPlayer.IOnCompletionListener,
        TextureView.ISurfaceTextureListener
    {
        private AutoFitTextureView _textureView;
        private Surface _surface;
        private MediaPlayer _mediaPlayer;
        private CancellationTokenSource _registrationCancellationTokenSource;

        private readonly SafeExecutionWrapper _safeExecutionWrapper;

        private bool _isPrepared;
        private bool _isPlayNeeded;
        private string _url;

        public VideoPlayer()
        {
            _safeExecutionWrapper = new SafeExecutionWrapper((ex) =>
            {
                Crashes.TrackError(ex);
                return Task.CompletedTask;
            });

            SetupNewMediaPlayerIfNeeded();
        }

        public event EventHandler<VideoPlayingStatus> VideoPlayingStatusChanged;

        public bool CanRepeat { get; set; }

        private bool _isMuted;
        public bool IsMuted
        {
            get => _isMuted;
            set
            {
                _isMuted = value;
                RefreshMutedState();
            }
        }

        public bool IsPlaying => _mediaPlayer.IsPlaying;

        public Action ReadyToPlayAction { get; set; }

        public object GetNativePlayer() => _mediaPlayer;

        public void OnPrepared(MediaPlayer mp)
        {
            _isPrepared = true;

            if (_isPlayNeeded)
            {
                Play();
            }
        }

        public void Pause()
        {
            if (_isPrepared)
            {
                _mediaPlayer.Pause();
            }

            _isPlayNeeded = false;

            VideoPlayingStatusChanged?.Invoke(this, VideoPlayingStatus.Paused);
        }

        public void Play()
        {
            _registrationCancellationTokenSource?.Cancel();
            _registrationCancellationTokenSource = null;

            if (_isPrepared && !IsPlaying)
            {
                _mediaPlayer.Start();
                _isPlayNeeded = false;

                VideoPlayingStatusChanged?.Invoke(this, VideoPlayingStatus.Started);
                return;
            }

            _isPlayNeeded = true;
        }

        public void Stop()
        {
            if (_isPrepared && IsPlaying)
            {
                _mediaPlayer.Stop();
            }

            VideoPlayingStatusChanged?.Invoke(this, VideoPlayingStatus.Stopped);

            _isPlayNeeded = false;
        }

        public void SetPlatformVideoPlayerContainer(object container)
        {
            if (container is AutoFitTextureView textureView)
            {
                _isPrepared = false;

                _textureView = textureView;
                _ = ResetPlayerAsync();
                return;
            }

            _textureView = null;
        }

        public void SetVideoUrl(string url)
        {
            _url = url;
        }

        public void OnVideoSizeChanged(MediaPlayer mp, int width, int height)
        {
            _textureView?.SetAspectRatio(width, height);
        }

        public bool OnError(MediaPlayer mp, [GeneratedEnum] MediaError what, int extra)
        {
            System.Diagnostics.Debug.WriteLine("Attempt to restore media player");

            _ = ResetPlayerAsync();
            return true;
        }

        public bool OnInfo(MediaPlayer mp, [GeneratedEnum] MediaInfo what, int extra)
        {
            if (what == MediaInfo.VideoRenderingStart)
            {
                ReadyToPlayAction?.Invoke();
                _ = _safeExecutionWrapper.WrapAsync(() => RegisterVideoPlayingPartiallyCallbackAsync(_url));
            }

            return true;
        }

        public void OnCompletion(MediaPlayer mp) =>
            VideoPlayingStatusChanged?.Invoke(this, VideoPlayingStatus.Played);

        public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
        {
        }

        public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
        {
            Stop();

            _textureView.SurfaceTextureListener = null;
            _surface?.Release();
            _surface?.Dispose();
            _textureView = null;

            return true;
        }

        public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
        {
        }

        public void OnSurfaceTextureUpdated(SurfaceTexture surface)
        {
        }

        private Task ResetPlayerAsync()
        {
            return _safeExecutionWrapper.WrapAsync(async () =>
            {
                if (string.IsNullOrWhiteSpace(_url))
                {
                    return;
                }

                if (IsPlaying)
                {
                    _mediaPlayer.Stop();
                }

                if (_textureView?.SurfaceTexture is null)
                {
                    return;
                }

                _textureView.SurfaceTextureListener = this;
                _surface = new Surface(_textureView.SurfaceTexture);

                SetupNewMediaPlayerIfNeeded();

                _mediaPlayer.SetSurface(_surface);
                await _mediaPlayer.SetDataSourceAsync(_url);
                _mediaPlayer.PrepareAsync();
            });
        }

        private void SetupNewMediaPlayerIfNeeded()
        {
            if (_mediaPlayer != null)
            {
                _mediaPlayer.Reset();
                _mediaPlayer.Release();
                _mediaPlayer.Dispose();
            }

            _isPrepared = false;

            _mediaPlayer = new MediaPlayer
            {
                Looping = CanRepeat
            };

            _mediaPlayer.SetAudioAttributes(new AudioAttributes.Builder()
                .SetLegacyStreamType(Stream.Music)
                .Build());

            _mediaPlayer.SetWakeMode(Application.Context, WakeLockFlags.Partial);
            _mediaPlayer.SetOnInfoListener(this);
            _mediaPlayer.SetOnVideoSizeChangedListener(this);
            _mediaPlayer.SetOnPreparedListener(this);
            _mediaPlayer.SetOnErrorListener(this);
            _mediaPlayer.SetOnCompletionListener(this);

            RefreshMutedState();
        }

        private async Task RegisterVideoPlayingPartiallyCallbackAsync(string videoUrl)
        {
            _registrationCancellationTokenSource = new CancellationTokenSource();
            await Task.Delay(TimeSpan.FromSeconds(Constants.Delays.VideoPartiallyPlayedDelay), _registrationCancellationTokenSource.Token);
            _registrationCancellationTokenSource = null;

            if (videoUrl == _url && IsPlaying)
            {
                VideoPlayingStatusChanged?.Invoke(this, VideoPlayingStatus.PartiallyPlayed);
            }
        }

        private void RefreshMutedState()
        {
            var volume = IsMuted ? 0 : 1;
           _mediaPlayer?.SetVolume(volume, volume);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                _mediaPlayer.Release();
                _mediaPlayer.Dispose();

                _textureView = null;
                _surface?.Release();
                _surface?.Dispose();
            }
        }
    }
}