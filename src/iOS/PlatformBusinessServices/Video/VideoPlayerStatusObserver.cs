using System;
using AVFoundation;
using CoreMedia;
using Foundation;

namespace PrankChat.Mobile.iOS.PlatformBusinessServices.Video
{
    public class VideoPlayerStatusObserver : NSObject
    {
        private readonly AVPlayer _player;
        private readonly int _repeatDelayInSeconds;

        public VideoPlayerStatusObserver(AVPlayer player, int repeatDelayInSeconds)
        {
            _player = player;
            _repeatDelayInSeconds = repeatDelayInSeconds;
        }

        public override void ObserveValue(NSString keyPath, NSObject ofObject, NSDictionary change, IntPtr context)
        {
            if (Equals(ofObject, _player) && keyPath.Equals((NSString)"status"))
            {
                if (_player.Status == AVPlayerStatus.ReadyToPlay)
                    RepeatVideo();
            }
        }

        private void RepeatVideo()
        {
            var timeValue = _player.CurrentItem.CurrentTime;
            var currentTimePosition = timeValue.Seconds;

            if (currentTimePosition >= _repeatDelayInSeconds)
            {
                _player.Seek(new CMTime(0, 1));
            }
        }
    }
}
