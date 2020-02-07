﻿using Android.Media;

namespace PrankChat.Mobile.Droid.PlatformBusinessServices.Video.Listeners
{
    internal class VideoPlayerOnPreparedListener : Java.Lang.Object, MediaPlayer.IOnPreparedListener
    {
        private readonly string _uri;
        private readonly bool _isLooping;

        //public VideoPlayerOnPreparedListener(string uri, bool isLooping = false)
        //{
        //    _uri = uri;
        //    _isLooping = isLooping;
        //}

        public void OnPrepared(MediaPlayer mediaPlayer)
        {
            mediaPlayer.Looping = _isLooping;
            //mediaPlayer.SetDataSource(_uri);
            //mediaPlayer.Start();
        }
    }
}
