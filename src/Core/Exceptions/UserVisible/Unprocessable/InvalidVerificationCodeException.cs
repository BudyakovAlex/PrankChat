using System;

namespace PrankChat.Mobile.Core.Exceptions.UserVisible.Unprocessable
{
    public class InvalidVerificationCodeException : BaseUserVisibleException
    {
        public InvalidVerificationCodeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public InvalidVerificationCodeException(string message) : base(message)
        {
        }
    }
}
