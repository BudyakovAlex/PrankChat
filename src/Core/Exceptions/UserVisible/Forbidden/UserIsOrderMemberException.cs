using System;

namespace PrankChat.Mobile.Core.Exceptions.UserVisible.Forbidden
{
    public class UserIsOrderMemberException : BaseUserVisibleException
    {
        public UserIsOrderMemberException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public UserIsOrderMemberException(string message) : base(message)
        {
        }
    }
}
