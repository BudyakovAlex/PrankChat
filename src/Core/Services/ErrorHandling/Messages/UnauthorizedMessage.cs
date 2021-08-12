using System;
using MvvmCross.Plugin.Messenger;

namespace PrankChat.Mobile.Core.Services.ErrorHandling.Messages
{
    public class UnauthorizedMessage : MvxMessage
    {
        public UnauthorizedMessage(object sender) : base(sender)
        {
        }
    }
}
