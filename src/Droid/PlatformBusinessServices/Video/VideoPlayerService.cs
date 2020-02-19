using System;
using System.Diagnostics;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.BusinessServices;

namespace PrankChat.Mobile.Droid.PlatformBusinessServices.Video
{
    public class VideoPlayerService : IVideoPlayerService
    {
        private readonly IApiService _apiService;
        private readonly IMvxMessenger _mvxMessenger;
        private const int RepeatDelayInSeconds = 10;
        private const int ViewedFactRegistrationDelayInMilliseconds = 3000;
        private IVideoPlayer _player;

        public VideoPlayerService(IApiService apiService, IMvxMessenger mvxMessenger)
        {
            _apiService = apiService;
            _mvxMessenger = mvxMessenger;
        }

        public IVideoPlayer Player
        {
            get
            {
                if (_player == null)
                {
                    _player = new VideoPlayer(_apiService, _mvxMessenger);
                    _player.EnableRepeat(RepeatDelayInSeconds);
                }
                return _player;
            }
        }

        public bool Muted
        {
            get => _player.Muted;
            set => _player.Muted = value;
        }

        public void Play(string uri, int id)
        {
            if (_player.IsPlaying)
                return;

            Player.SetSourceUri(uri);
            Debug.WriteLine("Playing next source: " + uri);
            Player.Play();
            Player.TryRegisterViewedFact(id, ViewedFactRegistrationDelayInMilliseconds);
        }

        public void Play()
        {
            Player.Play();
        }

        public void Pause()
        {
            Player.Pause();
        }

        public void Stop()
        {
            Player.Stop();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && _player != null)
            {
                _player.Dispose();
            }
        }
    }
}
