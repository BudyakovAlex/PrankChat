using System;
using System.Diagnostics;
using AVFoundation;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.BusinessServices.Logger;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Managers.Video;

namespace PrankChat.Mobile.iOS.PlatformBusinessServices.Video
{
    public class VideoPlayerService : BaseVideoPlayerService
    {
        private IVideoPlayer _player;

        private readonly IVideoManager _videoManager;
        private readonly ILogger _logger;
        private readonly IMvxMessenger _mvxMessenger;

        public VideoPlayerService(IVideoManager videoManager, ILogger logger, IMvxMessenger mvxMessenger)
        {
            _videoManager = videoManager;
            _logger = logger;
            _mvxMessenger = mvxMessenger;

            AVAudioSession.SharedInstance().SetCategory(AVAudioSessionCategory.Playback);
        }

        public override IVideoPlayer Player
        {
            get
            {
                if (_player == null)
                {
                    _player = new VideoPlayer(_videoManager, _logger, _mvxMessenger);
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

            Player.SetSourceUri(uri, id);
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
