using System;

namespace PrankChat.Mobile.Core.Exceptions.UserVisible.NotFound
{
    public class NotFoundHttpException : BaseUserVisibleException
    {
        public NotFoundHttpException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public NotFoundHttpException(string message) : base(message)
        {
        }
    }
}
