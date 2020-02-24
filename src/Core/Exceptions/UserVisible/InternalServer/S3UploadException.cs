using System;

namespace PrankChat.Mobile.Core.Exceptions.UserVisible.InternalServer
{
    public class S3UploadException : BaseUserVisibleException
    {
        public S3UploadException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public S3UploadException(string message) : base(message)
        {
        }
    }
}
