using System.Collections.Generic;

namespace PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages
{
    internal class NotFoundErrorMessage : BaseErrorMessage
    {
        public NotFoundErrorMessage(object sender, IReadOnlyList<string> errorMessages = null) : base(sender, errorMessages)
        {
        }
    }
}
