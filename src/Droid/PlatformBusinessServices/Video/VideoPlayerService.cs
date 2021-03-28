using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Managers.Video;
using System;
using System.Diagnostics;

namespace PrankChat.Mobile.Droid.PlatformBusinessServices.Video
{
    public class VideoPlayerService : BaseVideoPlayerService 
    {
        private readonly IErrorHandleService _errorHandleService;
        private readonly IVideoManager _videoManager;
        private readonly IMvxMessenger _mvxMessenger;

        private IVideoPlayer _player;
        private int _currentVideoId;

        //TODO: fix gap with injection Manager to service layer
        public VideoPlayerService(IVideoManager videoManager,
                                  IMvxMessenger mvxMessenger,
                                  IErrorHandleService errorHandleService)
        {
            _videoManager = videoManager;

            _mvxMessenger = mvxMessenger;
            _errorHandleService = errorHandleService;
        }

        public override IVideoPlayer Player
        {
            get
            {
                if (_player == null)
                {
                    _player = new VideoPlayer(_videoManager, _mvxMessenger, _errorHandleService);
                    _player.EnableRepeat(Constants.Delays.RepeatDelayInSeconds);
                }

                return _player;
            }
        }

        public override bool Muted
        {
            get => _player.IsMuted;
            set => _player.IsMuted = value;
        }

        public override void Play(string uri, int id)
        {
            if (_currentVideoId == id)
            {
                return;
            }

            Player.SetSourceUri(uri, id);

            _currentVideoId = id;
            Player.Play();
            Player.TryRegisterViewedFact(id, Constants.Delays.VideoPartiallyPlayedDelay);
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
