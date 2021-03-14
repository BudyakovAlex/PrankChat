using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.Managers.Video;
using PrankChat.Mobile.Core.Presentation.Messages;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.BusinessServices
{
    public abstract class BaseVideoPlayer : IVideoPlayer
    {
        private readonly IVideoManager _videoManager;
        private readonly IMvxMessenger _mvxMessenger;

        protected BaseVideoPlayer(IVideoManager videoManager, IMvxMessenger mvxMessenger)
        {
            _videoManager = videoManager;
            _mvxMessenger = mvxMessenger;
        }

        public virtual void Dispose()
        {
        }

        public abstract bool IsPlaying { get; protected set; }

        public abstract bool Muted { get; set; }

        public Action VideoRenderingStartedAction { get; set; }

        public abstract void EnableRepeat(int repeatDelayInSeconds);

        public abstract void SetSourceUri(string uri, int id);

        public abstract void Play();

        public abstract void Pause();

        public abstract void Stop();

        public abstract void SetPlatformVideoPlayerContainer(object container);

        public abstract void TryRegisterViewedFact(int id, int registrationDelayInMilliseconds);

        protected async Task<bool> SendRegisterViewedFactAsync(int id, int registrationDelayInMilliseconds, int currentTimeInMilliseconds)
        {
            if (currentTimeInMilliseconds < registrationDelayInMilliseconds)
            {
                return false;
            }

            var views = await _videoManager.RegisterVideoViewedFactAsync(id);
            if (views.HasValue)
            {
                _mvxMessenger.Publish(new ViewCountMessage(this, id, views.Value));
            }
            
            Debug.WriteLine($"Views {views}");
            return true;
        }
    }
}
