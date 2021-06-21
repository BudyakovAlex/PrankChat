using System;

namespace PrankChat.Mobile.Core.Exceptions.UserVisible.Validation
{
    public class ValidationException : BaseUserVisibleException
    {
        public string LocalizedFieldName { get; }

        public string RelativeValue { get; }

        public ValidationErrorType ErrorType { get; }

        public ValidationException(string message) : base(message)
        {
            ErrorType = ValidationErrorType.Undefined;
        }

        public ValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ValidationException(string localizedFieldName, ValidationErrorType errorType) : base(localizedFieldName)
        {
            LocalizedFieldName = localizedFieldName;
            ErrorType = errorType;
        }

        public ValidationException(string localizedFieldName, ValidationErrorType errorType, string relativeValue) : base(localizedFieldName)
        {
            LocalizedFieldName = localizedFieldName;
            ErrorType = errorType;
            RelativeValue = relativeValue;
        }
    }
}
