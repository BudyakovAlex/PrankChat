using System;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.BusinessServices.Logger
{
    public interface ILogger
    {
        Task WriteRequestInfoAsync(DateTime dateTime, string tag, string message, bool isEndOfRequest = false, string parameters = "");

        Task<string> ExtractAndClearLogContentAsync();
    }
}
