using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Services.Common
{
    public interface ILogsService
    {
        Task<bool> SendLogsAsync(string filePath);
    }
}