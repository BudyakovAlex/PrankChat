namespace PrankChat.Mobile.Core.BusinessServices.TaskSchedulers.BackgroundTasks.Abstract
{
    public interface IBackgroundTask
    {
        bool IsEnabled { get; set; }

        void Start();

        void Stop();
    }
}
