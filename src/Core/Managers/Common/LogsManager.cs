using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Common;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Managers.Common
{
    public class LogsManager : ILogsManager
    {
        private readonly ILogsService _logsService;

        public LogsManager(ILogsService logsService)
        {
            _logsService = logsService;
        }

        public Task<bool> SendLogsAsync(string filePath)
        {
            return _logsService.SendLogsAsync(filePath);
        }
    }
}