using System;

namespace PrankChat.Mobile.Core.Plugins.Timer
{
    public interface ISystemTimer
    {
        event EventHandler TimerElapsed;
        void Start();
        void Stop();
    }
}
