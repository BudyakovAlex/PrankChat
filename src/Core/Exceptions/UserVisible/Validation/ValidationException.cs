using System;

namespace PrankChat.Mobile.Core.Exceptions.UserVisible.Validation
{
    public class ValidationException : BaseUserVisibleException
    {
        public ValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ValidationException(string localizedFieldName) : base(localizedFieldName)
        {
        }
    }
}
