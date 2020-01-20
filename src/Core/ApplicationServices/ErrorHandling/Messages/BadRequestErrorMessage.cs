using System.Collections.Generic;

namespace PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages
{
    internal class BadRequestErrorMessage : BaseErrorMessage
    {
        public BadRequestErrorMessage(object sender, IReadOnlyList<string> errorMessages = null) : base(sender, errorMessages)
        {
        }
    }
}
