using System;

namespace PrankChat.Mobile.Core.BusinessServices
{
    public interface IVideoPlayer : IDisposable
    {
        bool IsPlaying { get; }

        bool Muted { get; set; }

        void EnableRepeat(int repeatDelayInSeconds);

        void SetSourceUri(string uri, int id);

        void Play();

        void Pause();

        void Stop();

        Action VideoRenderingStartedAction { get; set; }

        void SetPlatformVideoPlayerContainer(object container);

        void TryRegisterViewedFact(int id, int registrationDelayInMilliseconds);
    }
}
