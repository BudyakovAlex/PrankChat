using MvvmCross;
using PrankChat.Mobile.Core.BusinessServices.Logger;
using PrankChat.Mobile.Core.BusinessServices.TaskSchedulers.BackgroundTasks.Abstract;
using PrankChat.Mobile.Core.Managers.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.BusinessServices.TaskSchedulers.BackgroundTasks.SendLogs
{
    public class SendLogsBackgroundTask : BackgroundTask, ISendLogsBackgroundTask
    {
        private readonly Lazy<ILogsManager> _lazyLogsManager = new Lazy<ILogsManager>(() => Mvx.IoCProvider.Resolve<ILogsManager>());

        private readonly ILogger _logger;

        public SendLogsBackgroundTask(ILogger logger)
        {
            _logger = logger;
        }

        protected override TimeSpan Interval => TimeSpan.FromMinutes(10);

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var logsManager = _lazyLogsManager.Value;
            await logsManager.SendLogsAsync(_logger.LogFilePath);
            await _logger.ClearLogAsync();
        }
    }
}