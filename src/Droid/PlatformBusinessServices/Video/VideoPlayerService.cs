using System;
using System.Diagnostics;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Infrastructure;

namespace PrankChat.Mobile.Droid.PlatformBusinessServices.Video
{
    public class VideoPlayerService : BaseVideoPlayerService 
    {
        private readonly IErrorHandleService _errorHandleService;
        private readonly IApiService _apiService;
        private readonly IMvxMessenger _mvxMessenger;
        private IVideoPlayer _player;
        private int _currentVideoId;

        public VideoPlayerService(IApiService apiService, IMvxMessenger mvxMessenger, IErrorHandleService errorHandleService)
        {
            _apiService = apiService;
            _mvxMessenger = mvxMessenger;
            _errorHandleService = errorHandleService;
        }

        public override IVideoPlayer Player
        {
            get
            {
                if (_player == null)
                {
                    _player = new VideoPlayer(_apiService, _mvxMessenger, _errorHandleService);
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
            if (_currentVideoId == id)
                return;

            Player.SetSourceUri(uri);
            _currentVideoId = id;
            Player.Play();
            Player.TryRegisterViewedFact(id, Constants.Delays.ViewedFactRegistrationDelayInMilliseconds);
            Debug.WriteLine("Playing next source: " + uri);
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
            _currentVideoId = 0;
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
