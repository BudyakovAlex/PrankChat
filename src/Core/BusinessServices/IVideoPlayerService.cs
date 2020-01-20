using System;
namespace PrankChat.Mobile.Core.BusinessServices
{
    public interface IVideoPlayerService
    {
        IVideoPlayer Player { get; }

        void Play(string uri);

        void Play();

        void Pause();

        void Stop();
    }
}
