using System.Threading.Tasks;
using Android.Media;
using Android.Net;
using Android.OS;
using Android.Views;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Droid.Presentation.Listeners;

namespace PrankChat.Mobile.Droid.PlatformBusinessServices.Video
{
    public class VideoPlayer : BaseVideoPlayer
    {
        private const int RecheckDelayInMilliseconds = 1000;

        private readonly IErrorHandleService _errorHandleService;

        private TextureView _textureView;
        private MediaPlayer _mediaPlayer;

        private bool _isRepeatEnabled;

        public VideoPlayer(IApiService apiService, IMvxMessenger mvxMessenger, IErrorHandleService errorHandleService) : base(apiService, mvxMessenger)
        {
            _errorHandleService = errorHandleService;
        }

        public override bool IsPlaying { get; protected set; }

        private bool _isMuted;
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

            _mediaPlayer?.Pause();
            IsPlaying = false;
        }

        public override void Play()
        {
            if (IsPlaying || _textureView == null)
            {
                return;
            }

            _mediaPlayer?.Start();
            IsPlaying = true;
        }

        public override void SetPlatformVideoPlayerContainer(object container)
        {
            if (_textureView == container)
            {
                return;
            }

            Stop();

            if (container is TextureView videoView)
            {
                _textureView = videoView;
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

            _mediaPlayer = MediaPlayer.Create(Xamarin.Essentials.Platform.CurrentActivity, Uri.Parse(uri));
            if (_textureView?.SurfaceTexture is null)
            {
                return;
            }

            var surface = new Surface(_textureView.SurfaceTexture);
            _mediaPlayer.SetSurface(surface);
            _mediaPlayer.SetOnPreparedListener(new MediaPlayerOnPreparedListener(OnMediaPlayerPrepeared));
            _mediaPlayer.SetOnInfoListener(new MediaPlayerOnInfoListener(OnInfoChanged));
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
            _mediaPlayer = mediaPlayer;
            mediaPlayer.Looping = _isRepeatEnabled;
            mediaPlayer.SetVolume(0, 0);
        }

        public override void Stop()
        {
            if (!IsPlaying || _textureView == null)
            {
                return;
            }

            _mediaPlayer?.Stop();
            _mediaPlayer?.SetOnPreparedListener(null);

            _textureView = null;
            _textureView = null;
            IsPlaying = false;
        }

        private async Task RegisterAction(int id, int registrationDelayInMilliseconds)
        {
            if (_mediaPlayer == null)
            {
                _errorHandleService.LogError(nameof(VideoPlayer), $"{nameof(_textureView)} is null.");
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
