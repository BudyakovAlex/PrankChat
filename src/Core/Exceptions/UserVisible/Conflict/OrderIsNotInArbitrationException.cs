using System;

namespace PrankChat.Mobile.Core.Exceptions.UserVisible.Conflict
{
    public class OrderIsNotInArbitrationException : BaseUserVisibleException
    {
        public OrderIsNotInArbitrationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public OrderIsNotInArbitrationException(string message) : base(message)
        {
        }
    }
}
