﻿using System;
using System.Threading.Tasks;
using MvvmCross.Plugin.Messenger;

namespace PrankChat.Mobile.Core.Plugins.Timer
{
    public class Timer : ITimer
    {
        private const int TimerDelayInMilliseconds = 3000;

        private readonly IMvxMessenger _messenger;

        public Timer(IMvxMessenger messenger)
        {
            _messenger = messenger;

            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(TimerDelayInMilliseconds);
                    OnTimerCallback();
                }
            });
        }

        private void OnTimerCallback()
        {
            //_messenger.Publish(new TimerTickMessage(this));
        }
    }
}
