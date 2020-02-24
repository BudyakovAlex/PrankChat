using System;

namespace PrankChat.Mobile.Core.Exceptions.UserVisible.Conflict
{
    public class YouAreSubscribedException : BaseUserVisibleException
    {
        public YouAreSubscribedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public YouAreSubscribedException(string message) : base(message)
        {
        }
    }
}
