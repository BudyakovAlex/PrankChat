using System;

namespace PrankChat.Mobile.Core.Exceptions.UserVisible.Forbidden
{
    public class NegativeReplenishmentException : BaseUserVisibleException
    {
        public NegativeReplenishmentException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public NegativeReplenishmentException(string message) : base(message)
        {
        }
    }
}
