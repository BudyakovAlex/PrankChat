using System;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Presentation.Messages
{
    public class OrderChangedMessage : MvxMessage
    {
        public OrderChangedMessage(object sender, Order newOrder) : base(sender)
        {
            NewOrder = newOrder ?? throw new ArgumentNullException(nameof(newOrder));
        }

        public Order NewOrder { get; }
    }
}