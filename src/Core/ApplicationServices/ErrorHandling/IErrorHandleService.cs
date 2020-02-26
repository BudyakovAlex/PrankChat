using System;

namespace PrankChat.Mobile.Core.ApplicationServices.ErrorHandling
{
    public interface IErrorHandleService
    {
        void HandleException(Exception exception);

        void LogError(object sender, string message, Exception exception = null);
    }
}
