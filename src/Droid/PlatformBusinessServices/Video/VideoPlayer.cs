using System.Threading.Tasks;
using Android.Media;
using Android.OS;
using Android.Widget;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Droid.Presentation.Listeners;

namespace PrankChat.Mobile.Droid.PlatformBusinessServices.Video
{
    public class VideoPlayer : BaseVideoPlayer
    {
        private readonly IErrorHandleService _errorHandleService;

        private const int RecheckDelayInMilliseconds = 1000;
        private VideoView _videoView;
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

        public override void Dispose()
        {
            _videoView?.SetOnPreparedListener(null);
        }

        public override void EnableRepeat(int repeatDelayInSeconds)
        {
            _isRepeatEnabled = true;
        }

        public override void Pause()
        {
            if (!IsPlaying || _videoView == null)
                return;

            _videoView.Pause();
            IsPlaying = false;
        }

        public override void Play()
        {
            if (IsPlaying || _videoView == null)
                return;

            _videoView.Start();
            IsPlaying = true;
        }

        public override void SetPlatformVideoPlayerContainer(object container)
        {
            if (_videoView == container)
                return;

            Stop();

            if (container is VideoView videoView)
            {
                _videoView = videoView;
            }
            else
            {
                _videoView = null;
            }
        }

        public override void TryRegisterViewedFact(int id, int registrationDelayInMilliseconds)
        {
            var handler = new Handler();
            handler.PostDelayed(async () => await RegisterAction(id, registrationDelayInMilliseconds), registrationDelayInMilliseconds);
        }

        public override void SetSourceUri(string uri)
        {
            _videoView.SetVideoPath(uri);
            _videoView.SetOnPreparedListener(new MediaPlayerOnPreparedListener(OnMediaPlayerPrepeared));
        }

        private void OnMediaPlayerPrepeared(MediaPlayer mediaPlayer)
        {
            _mediaPlayer = mediaPlayer;
            mediaPlayer.Looping = _isRepeatEnabled;
            mediaPlayer.SetVolume(0, 0);
        }

        public override void Stop()
        {
            if (!IsPlaying || _videoView == null)
                return;

            _videoView?.StopPlayback();
            IsPlaying = false;
        }

        private async Task RegisterAction(int id, int registrationDelayInMilliseconds)
        {
            if (_videoView == null)
            {
                _errorHandleService.LogError(nameof(VideoPlayer), $"{nameof(_videoView)} is null.");
                return;
            }

            var sent = await SendRegisterViewedFactAsync(id, registrationDelayInMilliseconds, _videoView.CurrentPosition);

            if (!sent)
            {
                var handler = new Handler();
                handler.PostDelayed(async () => await RegisterAction(id, registrationDelayInMilliseconds), RecheckDelayInMilliseconds);
            }
        }
    }
}
