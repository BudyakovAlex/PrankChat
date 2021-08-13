using System;
using System.Threading.Tasks;
using MvvmCross.Plugin.Messenger;
using System.Timers;

namespace PrankChat.Mobile.Core.Plugins.Timer
{
    public class SystemTimer : ISystemTimer, IDisposable
    {
        private const int TimerDelayInMilliseconds = 3000;
        private System.Timers.Timer _timer;
        public event EventHandler TimerElapsed;

        public SystemTimer()
        {
            _timer = new System.Timers.Timer(TimerDelayInMilliseconds);
            _timer.Elapsed += OnTimerElapsed;
            _timer.AutoReset = true;
            _timer.Enabled = false;
            _timer.Enabled = false;
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            TimerElapsed.Invoke(this, EventArgs.Empty);
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}
