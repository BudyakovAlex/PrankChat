using System;

namespace PrankChat.Mobile.Core.Exceptions.UserVisible
{
    public abstract class BaseUserVisibleException : Exception
    {
        public BaseUserVisibleException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public BaseUserVisibleException(string message) : base(message)
        {
        }
    }
}
