using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Managers.Common
{
    public interface ILogsManager
    {
        Task<bool> SendLogsAsync(string filePath);
    }
}