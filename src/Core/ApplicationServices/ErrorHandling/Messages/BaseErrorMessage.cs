using System.Collections.Generic;
using MvvmCross.Plugin.Messenger;

namespace PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages
{
    internal abstract class BaseErrorMessage : MvxMessage
    {
        public IReadOnlyList<string> ErrorMessages { get; }

        public BaseErrorMessage(object sender, IReadOnlyList<string> errorMessages = null) : base(sender)
        {
            ErrorMessages = errorMessages;
        }
    }
}
