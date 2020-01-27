using System;
using PrankChat.Mobile.Core.BusinessServices;

namespace PrankChat.Mobile.Droid.PlatformBusinessServices.Video
{
    public class VideoPlayer : IVideoPlayer
    {
        public bool IsPlaying => false;

        public bool Muted { get => true; set { } }

        public void Dispose()
        {

        }

        public void EnableRepeat(int repeatDelayInSeconds)
        {
            
        }

        public void Pause()
        {
            
        }

        public void Play()
        {
            
        }

        public void SetPlatformVideoPlayerContainer(object container)
        {
            
        }

        public void SetSourceUri(string uri)
        {
            
        }

        public void Stop()
        {
            
        }
    }
}
