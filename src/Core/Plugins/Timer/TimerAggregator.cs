using System;
using System.Collections.Generic;
using System.Text;

namespace PrankChat.Mobile.Core.Plugins.Timer
{
    class TimerAggregator : IObservable<SystemTimer>
    {
        private readonly List<IObserver<SystemTimer>> _observers;

        public TimerAggregator()
        {
            _observers = new List<IObserver<SystemTimer>>();
        }

        public IDisposable Subscribe(IObserver<SystemTimer> observer)
        {
            _observers.Add(observer);
            return null;
        }

        public void Unsubscribe(IObserver<SystemTimer> observer)
        {
            _observers.Remove(observer);
        }

        public void Notify(SystemTimer timer)
        {

        }
    }
}
