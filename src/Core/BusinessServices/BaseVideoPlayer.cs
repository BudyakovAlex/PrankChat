using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.BusinessServices.Logger;
using PrankChat.Mobile.Core.Presentation.Messages;

namespace PrankChat.Mobile.Core.BusinessServices
{
    public abstract class BaseVideoPlayer : IVideoPlayer
    {
        private readonly IApiService _apiService;
        private readonly IMvxMessenger _mvxMessenger;

        protected BaseVideoPlayer(IApiService apiService, ILogger logger, IMvxMessenger mvxMessenger)
        {
            Logger = logger;
            _apiService = apiService;
            _mvxMessenger = mvxMessenger;
        }

        public virtual void Dispose()
        {
        }

        protected ILogger Logger { get; }

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
                return false;

            var views = await _apiService.RegisterVideoViewedFactAsync(id);

            if (views.HasValue)
                _mvxMessenger.Publish(new ViewCountMessage(this, id, views.Value));
            
            Debug.WriteLine($"Views {views}");

            return true;
        }
    }
}
