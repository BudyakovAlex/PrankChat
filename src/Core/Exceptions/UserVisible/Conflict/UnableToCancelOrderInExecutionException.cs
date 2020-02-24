using System;

namespace PrankChat.Mobile.Core.Exceptions.UserVisible.Conflict
{
    public class UnableToCancelOrderInExecutionException : BaseUserVisibleException
    {
        public UnableToCancelOrderInExecutionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public UnableToCancelOrderInExecutionException(string message) : base(message)
        {
        }
    }
}
