using System;
namespace PrankChat.Mobile.Core.Exceptions
{
    public class UserVisibleException : Exception
    {
        public UserVisibleException(string message) : base(message)
        {
        }
    }
}
