using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Common
{
    public interface ILogsService
    {
        Task<bool> SendLogsAsync(string filePath);
    }
}