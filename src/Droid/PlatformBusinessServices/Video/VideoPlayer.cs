using System.Threading.Tasks;
using Android.OS;
using Android.Widget;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Droid.PlatformBusinessServices.Video.Listeners;
using Debug = System.Diagnostics.Debug;

namespace PrankChat.Mobile.Droid.PlatformBusinessServices.Video
{
    public class VideoPlayer : IVideoPlayer
    {
        private const int RecheckDelayInMilliseconds = 1000;
        private readonly IApiService _apiService;
        private readonly IMvxMessenger _mvxMessenger;
        private VideoView _videoView;
        private bool _isRepeatEnabled;

        public VideoPlayer(IApiService apiService, IMvxMessenger mvxMessenger)
        {
            _apiService = apiService;
            _mvxMessenger = mvxMessenger;
        }

        public bool IsPlaying { get; private set; }

        public bool Muted { get; set; }

        public void Dispose()
        {
            _videoView?.SetOnPreparedListener(null);
        }

        public void EnableRepeat(int repeatDelayInSeconds)
        {
            _isRepeatEnabled = true;
        }

        public void Pause()
        {
            if (!IsPlaying || _videoView == null)
                return;

            _videoView.Pause();
            IsPlaying = false;
        }

        public void Play()
        {
            if (IsPlaying || _videoView == null)
                return;

            _videoView.Start();
            IsPlaying = true;
        }

        public void SetPlatformVideoPlayerContainer(object container)
        {
            if (_videoView == container)
                return;

            if (container is VideoView videoView)
            {
                _videoView = videoView;
            }
            else
            {
                _videoView = null;
            }
        }

        public void TryRegisterViewedFact(int id, int registrationDelayInMilliseconds)
        {
            var handler = new Handler();
            handler.PostDelayed(async () => await RegisterAction(id, registrationDelayInMilliseconds), registrationDelayInMilliseconds);
        }

        private async Task RegisterAction(int id, int registrationDelayInMilliseconds)
        {
            if (_videoView.CurrentPosition >= registrationDelayInMilliseconds)
            {
                var views = await _apiService.RegisterVideoViewedFactAsync(id);
                if (views.HasValue)
                    _mvxMessenger.Publish(new ViewCountMessage(this, id, views.Value));

                Debug.WriteLine($"Views {views}");
            }
            else
            {
                var handler = new Handler();
                handler.PostDelayed(async () => await RegisterAction(id, registrationDelayInMilliseconds), RecheckDelayInMilliseconds);
            }
        }

        public void SetSourceUri(string uri)
        {
            _videoView.SetVideoPath(uri);
            _videoView.SetOnPreparedListener(new VideoPlayerOnPreparedListener(_isRepeatEnabled));
        }

        public void Stop()
        {
            if (!IsPlaying || _videoView == null)
                return;

            _videoView?.StopPlayback();
            IsPlaying = false;
        }
    }
}
