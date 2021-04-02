using Android.Media;
using Android.Views;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Data.Enums;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.Presentation.Listeners;
using System;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Droid.PlatformBusinessServices.Video
{
    public class VideoPlayer : Java.Lang.Object, IVideoPlayer
    {
        private AutoFitTextureView _textureView;
        private Surface _surface;
        private MediaPlayer _mediaPlayer;

        private bool _isPrepared;
        private bool _isPlayNeeded;
        private string _url;

        public VideoPlayer()
        {
            SetupNewMediaPlayerIfNeeded();
        }

        public event EventHandler<VideoPlayingStatus> VideoPlayingStatusChanged;

        public bool CanRepeat
        {
            get => _mediaPlayer.Looping;
            set => _mediaPlayer.Looping = value;
        }

        private bool _isMuted;
        public bool IsMuted
        {
            get => _isMuted;
            set
            {
                _isMuted = value;
                if (_isMuted)
                {
                    _mediaPlayer?.SetVolume(0, 0);
                    return;
                }

                _mediaPlayer?.SetVolume(1, 1);
            }
        }

        public bool IsPlaying => _mediaPlayer.IsPlaying;

        public object GetNativePlayer() => _mediaPlayer;

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
                _mediaPlayer.Pause();
                _mediaPlayer.SeekTo(0);
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

        private async Task ResetPlayerAsync()
        {
            await Task.Delay(100);

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

            if (_surface != null)
            {
                _surface.Release();
                _surface.Dispose();
            }

            _surface = new Surface(_textureView.SurfaceTexture);

            SetupNewMediaPlayerIfNeeded();

            _mediaPlayer.SetSurface(_surface);
            await _mediaPlayer.SetDataSourceAsync(_url);
            _mediaPlayer.PrepareAsync();
        }

        private void OnPlayerSizeChanged(MediaPlayer mp, int width, int height)
        {
            _textureView?.SetAspectRatio(width, height);
        }

        private bool OnError(MediaPlayer mp, MediaError error, int arg3)
        {
            System.Diagnostics.Debug.WriteLine("Attempt to restore media player");

            _ = ResetPlayerAsync();
            return true;
        }

        private void OnMediaPlayerPrepeared(MediaPlayer mediaPlayer)
        {
            _isPrepared = true;

            if (_isPlayNeeded)
            {
                Play();
            }
        }

        private void SetupNewMediaPlayerIfNeeded()
        {
            if (_isPrepared)
            {
                _mediaPlayer.Reset();
                _mediaPlayer.Release();
                _mediaPlayer.Dispose();
                _mediaPlayer = null;

                _isPrepared = false;
            }

            if (_mediaPlayer is null)
            {
                _mediaPlayer = new MediaPlayer();
                _mediaPlayer.SetAudioStreamType(Stream.Music);
                _mediaPlayer.SetOnVideoSizeChangedListener(new MediaPlayerOnVideoSizeChanged(OnPlayerSizeChanged));
                _mediaPlayer.SetOnPreparedListener(new MediaPlayerOnPreparedListener(OnMediaPlayerPrepeared));
                _mediaPlayer.SetOnErrorListener(new MediaPlayerOnErrorListener(OnError));
            }
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