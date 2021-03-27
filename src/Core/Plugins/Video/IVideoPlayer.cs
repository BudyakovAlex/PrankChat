using PrankChat.Mobile.Core.Data.Enums;
using System;

namespace PrankChat.Mobile.Core.BusinessServices
{
    public interface IVideoPlayer : IDisposable
    {
        bool IsPlaying { get; }

        bool IsMuted { get; set; }

        void EnableRepeat(int repeatDelayInSeconds);

        void SetVideoUrl(string url);

        void Play();

        void Pause();

        void Stop();

        event EventHandler<VideoPlayingStatus> VideoPlayingStatusChanged;
    }
}
