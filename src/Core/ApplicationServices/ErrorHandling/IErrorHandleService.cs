using System;

namespace PrankChat.Mobile.Core.ApplicationServices.ErrorHandling
{
    public interface IErrorHandleService
    {
        void HandleException(Exception exception);
    }
}
