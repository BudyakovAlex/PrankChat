using System;
using System.Threading.Tasks;
using Android.Media;
using Android.OS;
using Android.Views;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.Presentation.Listeners;

namespace PrankChat.Mobile.Droid.PlatformBusinessServices.Video
{
    public class VideoPlayer : BaseVideoPlayer
    {
        private const int RecheckDelayInMilliseconds = 1000;

        private readonly IErrorHandleService _errorHandleService;

        private AutoFitTextureView _textureView;
        private MediaPlayer _mediaPlayer;

        private bool _isRepeatEnabled;
        private bool _isPrepared;
        private bool _isPlayNeeded;

        private string _cachedUri;

        public VideoPlayer(IApiService apiService, IMvxMessenger mvxMessenger, IErrorHandleService errorHandleService) : base(apiService, mvxMessenger)
        {
            _errorHandleService = errorHandleService;
        }

        public override bool IsPlaying { get; protected set; }

        private bool _isMuted;
        private Surface _surface;

        public override bool Muted
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

        public override void EnableRepeat(int repeatDelayInSeconds)
        {
            _isRepeatEnabled = true;
        }

        public override void Pause()
        {
            if (!IsPlaying || _textureView == null)
            {
                return;
            }

            if (_isPrepared)
            {
                _mediaPlayer?.Pause();
            }

            IsPlaying = false;
            _isPlayNeeded = false;
        }

        public override void Play()
        {
            if (IsPlaying || _textureView == null)
            {
                return;
            }

            if (_isPrepared)
            {
                _mediaPlayer?.Start();
                IsPlaying = true;
                _isPlayNeeded = false;
                return;
            }

            _isPlayNeeded = true;
        }

        public override void SetPlatformVideoPlayerContainer(object container)
        {
            if (_textureView == container)
            {
                return;
            }

            Stop();

            if (container is AutoFitTextureView textureView)
            {
                _textureView = textureView;
                return;
            }

            _textureView = null;
        }

        public override void TryRegisterViewedFact(int id, int registrationDelayInMilliseconds)
        {
            var handler = new Handler();
            handler.PostDelayed(async () => await RegisterAction(id, registrationDelayInMilliseconds), registrationDelayInMilliseconds);
        }

        public override void SetSourceUri(string uri)
        {
            if (string.IsNullOrWhiteSpace(uri))
            {
                return;
            }

            _cachedUri = uri;
            CreateMediaPlayer();
        }

        private void CreateMediaPlayer()
        {
            if (string.IsNullOrWhiteSpace(_cachedUri))
            {
                return;
            }

            if (_mediaPlayer != null)
            {
                _mediaPlayer.Release();
            }

            _mediaPlayer = MediaPlayer.Create(Xamarin.Essentials.Platform.CurrentActivity, Android.Net.Uri.Parse(_cachedUri));

            if (_textureView?.SurfaceTexture is null)
            {
                return;
            }

            if (_surface != null)
            {
                _surface.Release();
            }

            _surface = new Surface(_textureView.SurfaceTexture);
            _mediaPlayer.SetSurface(_surface);

            _mediaPlayer.SetOnVideoSizeChangedListener(new MediaPlayerOnVideoSizeChanged(OnPlayerSizeChanged));
            _mediaPlayer.SetOnPreparedListener(new MediaPlayerOnPreparedListener(OnMediaPlayerPrepeared));
            _mediaPlayer.SetOnInfoListener(new MediaPlayerOnInfoListener(OnInfoChanged));
            _mediaPlayer.SetOnErrorListener(new MediaPlayerOnErrorListener(OnError));
        }

        private void OnPlayerSizeChanged(MediaPlayer mp, int width, int height)
        {
            _textureView?.SetAspectRatio(width, height);
        }

        private bool OnError(MediaPlayer mp, MediaError error, int arg3)
        {
            _errorHandleService.LogError(mp, $"Media error {error}");
            System.Diagnostics.Debug.WriteLine("Attempt to restore media player");

            CreateMediaPlayer();
            return true;
        }

        private bool OnInfoChanged(MediaPlayer mediaPlayer, MediaInfo mediaInfo, int extra)
        {
            if (mediaInfo == MediaInfo.VideoRenderingStart)
            {
                VideoRenderingStartedAction?.Invoke();
            }

            return true;
        }

        private void OnMediaPlayerPrepeared(MediaPlayer mediaPlayer)
        {
            _mediaPlayer.Looping = _isRepeatEnabled;
            _mediaPlayer.SetVolume(0, 0);

            _isPrepared = true;

            if (_isPlayNeeded)
            {
                Play();
            }
        }

        public override void Stop()
        {
            if (!IsPlaying ||
                _textureView == null)
            {
                return;
            }

            if (_isPrepared)
            {
                _mediaPlayer?.Stop();
            }

            _mediaPlayer?.SetOnPreparedListener(null);
            _mediaPlayer?.Release();
            _surface?.Release();

            _textureView = null;
            _textureView = null;
            _cachedUri = null;
            _surface = null;

            IsPlaying = false;
            _isPlayNeeded = false;
            _isPrepared = false;
        }

        private async Task RegisterAction(int id, int registrationDelayInMilliseconds)
        {
            if (_mediaPlayer == null)
            {
                _errorHandleService.LogError(nameof(VideoPlayer), $"{nameof(_mediaPlayer)} is null.");
                return;
            }

            var isSent = await SendRegisterViewedFactAsync(id, registrationDelayInMilliseconds, _mediaPlayer.CurrentPosition);

            if (!isSent)
            {
                var handler = new Handler();
                handler.PostDelayed(async () => await RegisterAction(id, registrationDelayInMilliseconds), RecheckDelayInMilliseconds);
            }
        }
    }
}
