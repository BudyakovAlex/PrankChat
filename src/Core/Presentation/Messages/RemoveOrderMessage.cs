using System;
using MvvmCross.Plugin.Messenger;

namespace PrankChat.Mobile.Core.Presentation.Messages
{
    public class RemoveOrderMessage : MvxMessage
    {
        public int OrderId { get; }

        public RemoveOrderMessage(object sender, int orderId) : base(sender)
        {
            OrderId = orderId;
        }
    }
}
