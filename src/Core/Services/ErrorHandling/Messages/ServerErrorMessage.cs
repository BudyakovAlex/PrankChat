using System;
using MvvmCross.Plugin.Messenger;

namespace PrankChat.Mobile.Core.Services.ErrorHandling.Messages
{
    internal class ServerErrorMessage : MvxMessage
    {
        public Exception Error { get; }

        public ServerErrorMessage(object sender, Exception problemError) : base(sender)
        {
            Error = problemError;
        }
    }
}
