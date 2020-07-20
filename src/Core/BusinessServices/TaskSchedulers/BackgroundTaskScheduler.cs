using MvvmCross;
using PrankChat.Mobile.Core.BusinessServices.TaskSchedulers.BackgroundTasks.Abstract;
using PrankChat.Mobile.Core.BusinessServices.TaskSchedulers.BackgroundTasks.SendLogs;

namespace PrankChat.Mobile.Core.BusinessServices.TaskSchedulers
{
    public class BackgroundTaskScheduler : IBackgroundTaskScheduler
    {
        private readonly IBackgroundTask[] _tasks;

        public BackgroundTaskScheduler()
        {
            _tasks = new IBackgroundTask[]
            {
                Mvx.IoCProvider.Resolve<ISendLogsBackgroundTask>()
            };
        }

        public void Start()
        {
            foreach (var task in _tasks)
            {
                task.Start();
            }
        }

        public void Stop()
        {
            foreach (var task in _tasks)
            {
                task.Stop();
            }
        }
    }
}
