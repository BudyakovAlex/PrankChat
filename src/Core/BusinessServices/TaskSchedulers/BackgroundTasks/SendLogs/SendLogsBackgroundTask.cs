using MvvmCross;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.BusinessServices.Logger;
using PrankChat.Mobile.Core.BusinessServices.TaskSchedulers.BackgroundTasks.Abstract;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.BusinessServices.TaskSchedulers.BackgroundTasks.SendLogs
{
    public class SendLogsBackgroundTask : BackgroundTask, ISendLogsBackgroundTask
    {
        private readonly Lazy<IApiService> _lazyApiService = new Lazy<IApiService>(() => Mvx.IoCProvider.Resolve<IApiService>());
        private readonly ILogger _logger;

        public SendLogsBackgroundTask(ILogger logger)
        {
            _logger = logger;
        }

        protected override TimeSpan Interval => TimeSpan.FromMinutes(10);

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var apiService = _lazyApiService.Value;
            await apiService.SendLogsAsync(_logger.LogFilePath);
            await _logger.ClearLogAsync();
        }
    }
}