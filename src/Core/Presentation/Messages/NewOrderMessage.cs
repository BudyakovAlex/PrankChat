using System;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Presentation.Messages
{
    public class OrderChangedMessage : MvxMessage
    {
        public OrderDataModel NewOrder { get; }

        public OrderChangedMessage(object sender, OrderDataModel newOrder) : base(sender)
        {
            if (newOrder == null)
                throw new ArgumentNullException(nameof(newOrder));

            NewOrder = newOrder;
        }
    }
}
