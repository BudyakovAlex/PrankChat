using System;

namespace PrankChat.Mobile.Core.Services.ErrorHandling
{
    public interface IErrorHandleService
    {
        void SuspendServerErrorsHandling();

        void ResumeServerErrorsHandling();

        void HandleException(Exception exception);

        void LogError(object sender, string message, Exception exception = null);
    }
}
