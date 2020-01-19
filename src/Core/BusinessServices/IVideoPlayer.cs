using System;
namespace PrankChat.Mobile.Core.BusinessServices
{
    public interface IVideoPlayer
    {
        bool IsPlaying { get; }

        void EnableRepeat(int repeatDelayInSeconds);

        void SetSourceUri(string uri);

        void Play();

        void Stop();

        void SetPlatformVideoPlayerContainer(object container);
    }
}
