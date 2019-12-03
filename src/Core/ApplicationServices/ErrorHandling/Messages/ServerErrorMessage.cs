using System.Collections.Generic;

namespace PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages
{
    internal class ServerErrorMessage : BaseErrorMessage
    {
        public ServerErrorMessage(object sender, IReadOnlyList<string> errorMessages = null) : base(sender, errorMessages)
        {
        }
    }
}
