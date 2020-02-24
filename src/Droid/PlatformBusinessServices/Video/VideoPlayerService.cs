using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Infrastructure;

namespace PrankChat.Mobile.Droid.PlatformBusinessServices.Video
{
    public class VideoPlayerService : BaseVideoPlayerService 
    {
        private readonly IApiService _apiService;
        private readonly IMvxMessenger _mvxMessenger;
        private IVideoPlayer _player;
        private int _currentVideoId;

        public VideoPlayerService(IApiService apiService, IMvxMessenger mvxMessenger)
        {
            _apiService = apiService;
            _mvxMessenger = mvxMessenger;
        }

        public override IVideoPlayer Player
        {
            get
            {
                if (_player == null)
                {
                    _player = new VideoPlayer(_apiService, _mvxMessenger);
                    _player.EnableRepeat(Constants.Delays.RepeatDelayInSeconds);
                }
                return _player;
            }
        }

        public override bool Muted
        {
            get => _player.Muted;
            set => _player.Muted = value;
        }

        public override void Play(string uri, int id)
        {
            if (_player.IsPlaying)
                return;

            if (_currentVideoId != id)
            {
                Player.SetSourceUri(uri);
                _currentVideoId = id;
                Player.Play();
                Player.TryRegisterViewedFact(id, Constants.Delays.ViewedFactRegistrationDelayInMilliseconds);
                Debug.WriteLine("Playing next source: " + uri);
            }
        }

        public override void Play()
        {
            Player.Play();
        }

        public override void Pause()
        {
            Player.Pause();
        }

        public override void Stop()
        {
            Player.Stop();
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _player?.Dispose();
            }
        }
    }
}
