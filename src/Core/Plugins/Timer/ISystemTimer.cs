using System;

namespace PrankChat.Mobile.Core.Plugins.Timer
{
    public interface ISystemTimer
    {
        event EventHandler TimerElapsed;
        void Start();
        public void Stop();
    }
}
