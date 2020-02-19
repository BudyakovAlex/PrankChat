using System;

namespace PrankChat.Mobile.Core.BusinessServices
{
    public interface IVideoPlayer : IDisposable
    {
        bool IsPlaying { get; }

        bool Muted { get; set; }

        void EnableRepeat(int repeatDelayInSeconds);

        void SetSourceUri(string uri);

        void Play();

        void Pause();

        void Stop();

        void SetPlatformVideoPlayerContainer(object container);

        void TryRegisterViewedFact(int id, int registrationDelayInMilliseconds);
    }
}
