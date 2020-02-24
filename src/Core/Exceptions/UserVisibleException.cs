using System;
namespace PrankChat.Mobile.Core.Exceptions
{
    public class UserVisibleException : Exception
    {
        public UserVisibleException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public UserVisibleException(string message) : base(message)
        {
        }
    }
}
