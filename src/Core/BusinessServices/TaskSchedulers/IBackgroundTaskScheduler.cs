namespace PrankChat.Mobile.Core.BusinessServices.TaskSchedulers
{
    public interface IBackgroundTaskScheduler
	{
		void Start();

		void Stop();
	}
}
