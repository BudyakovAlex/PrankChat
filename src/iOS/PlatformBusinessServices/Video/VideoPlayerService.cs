using System;
using System.Diagnostics;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Infrastructure;

namespace PrankChat.Mobile.iOS.PlatformBusinessServices.Video
{
    public class VideoPlayerService : BaseVideoPlayerService
    {
        private IVideoPlayer _player;
        private readonly IApiService _apiService;
        private readonly IMvxMessenger _mvxMessenger;

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

            Player.SetSourceUri(uri);
            Debug.WriteLine("Playing next source: " + uri);
            Player.Play();
            Player.TryRegisterViewedFact(id, Constants.Delays.ViewedFactRegistrationDelayInMilliseconds);
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
