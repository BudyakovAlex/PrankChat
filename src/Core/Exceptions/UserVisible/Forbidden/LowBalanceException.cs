using System;

namespace PrankChat.Mobile.Core.Exceptions.UserVisible.Forbidden
{
    public class LowBalanceException : BaseUserVisibleException
    {
        public LowBalanceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public LowBalanceException(string message) : base(message)
        {
        }
    }
}
