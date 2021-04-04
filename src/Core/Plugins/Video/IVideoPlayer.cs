using PrankChat.Mobile.Core.Data.Enums;
using System;

namespace PrankChat.Mobile.Core.BusinessServices
{
    public interface IVideoPlayer : IDisposable
    {
        bool IsPlaying { get; }

        bool IsMuted { get; set; }

        bool CanRepeat { get; set; }

        void SetVideoUrl(string url);

        void Play();

        void Pause();

        void Stop();

        object GetNativePlayer();

        Action ReadyToPlayAction { get; set; }

        event EventHandler<VideoPlayingStatus> VideoPlayingStatusChanged;
    }
}
