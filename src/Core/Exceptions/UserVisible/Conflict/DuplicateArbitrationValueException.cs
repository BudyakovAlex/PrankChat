using System;

namespace PrankChat.Mobile.Core.Exceptions.UserVisible.Conflict
{
    public class DuplicateArbitrationValueException : BaseUserVisibleException
    {
        public DuplicateArbitrationValueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public DuplicateArbitrationValueException(string message) : base(message)
        {
        }
    }
}
