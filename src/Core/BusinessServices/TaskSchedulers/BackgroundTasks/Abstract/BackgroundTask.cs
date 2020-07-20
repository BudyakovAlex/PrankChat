using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace PrankChat.Mobile.Core.BusinessServices.TaskSchedulers.BackgroundTasks.Abstract
{
    public abstract class BackgroundTask : IBackgroundTask
    {
        private static readonly TimeSpan OneSecond = new TimeSpan(0, 0, 1);
        private static readonly TimeSpan OneMilliSecond = new TimeSpan(0, 0, 0, 0, 1);

        private System.Timers.Timer _timer;
        private CancellationTokenSource _cancellationTokenSource;

        public bool IsEnabled
        {
            get => _timer?.Enabled ?? false;
            set
            {
                if (_timer is null)
                {
                    System.Diagnostics.Debug.WriteLine("Fail to change IsEnabled state, time is null");
                    return;
                }

                _timer.Enabled = value;
            }
        }

        protected abstract TimeSpan Interval { get; }

        protected virtual bool NeedToStartImmediately => false;

        protected virtual string TaskName => GetType().Name;

        protected virtual bool CanExecute() => true;

        protected abstract Task ExecuteAsync(CancellationToken cancellationToken);

        protected virtual async Task HandleExceptionAsync(Exception ex)
        {
            await Task.Yield();

            System.Diagnostics.Debug.WriteLine(ex, $"The execution of BackgroundTask [{TaskName}] is failed");
        }

        public void Start()
        {
            if (_timer != null)
            {
                return;
            }

            if (Interval <= OneMilliSecond)
            {
                System.Diagnostics.Debug.WriteLine($"Cannot start BackgroundTask [{TaskName}]. Interval value must be strictly greater than 1 millisecond.");
                return;
            }

            _timer = new System.Timers.Timer(Interval.TotalMilliseconds)
            {
                AutoReset = true
            };

            if (NeedToStartImmediately)
            {
                Run();
            }

            _timer.Elapsed += OnTimedElapsed;
            _timer.Start();

            System.Diagnostics.Debug.WriteLine($"BackgroundTask [{TaskName}] has started");
        }

        public void Stop()
        {
            if (_timer is null)
            {
                return;
            }
            _timer.Stop();
            _timer.Dispose();
            _timer = null;

            _cancellationTokenSource?.Cancel();

            System.Diagnostics.Debug.WriteLine($"BackgroundTask [{TaskName}] has stopped");
        }

        protected virtual void OnTimedElapsed(object source, ElapsedEventArgs e)
        {
            if (!IsEnabled)
            {
                return;
            }

            Run();
        }

        protected virtual void Run()
        {
            _cancellationTokenSource?.Cancel();

            var timeout = GetTaskExecutionTimeout();
            _cancellationTokenSource = new CancellationTokenSource(timeout);
            var cancellationToken = _cancellationTokenSource.Token;

            Task.Factory.StartNew(() => ExecuteTaskAsync(cancellationToken), cancellationToken);
        }

        protected async Task ExecuteTaskAsync(CancellationToken cancellationToken)
        {
            if (!CanExecute())
            {
                return;
            }

            try
            {
                _cancellationTokenSource.Token.ThrowIfCancellationRequested();

                await ExecuteAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _cancellationTokenSource?.Cancel();

                await HandleExceptionAsync(ex);
            }
        }

        private TimeSpan GetTaskExecutionTimeout()
        {
            var timeToSubtract = Interval.TotalSeconds > 0 ? OneSecond : OneMilliSecond;
            return Interval.Subtract(timeToSubtract);
        }
    }
}
