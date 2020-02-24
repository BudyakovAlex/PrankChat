using System;

namespace PrankChat.Mobile.Core.Exceptions.UserVisible.Conflict
{
    public class UserHasAlreadyLeftLikeException : BaseUserVisibleException
    {
        public UserHasAlreadyLeftLikeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public UserHasAlreadyLeftLikeException(string message) : base(message)
        {
        }
    }
}
