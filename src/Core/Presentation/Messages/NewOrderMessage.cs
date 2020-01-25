using System;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Presentation.Messages
{
    public class NewOrderMessage : MvxMessage
    {
        public OrderDataModel NewOrder { get; }

        public NewOrderMessage(object sender, OrderDataModel newOrder) : base(sender)
        {
            if (newOrder == null)
                throw new ArgumentNullException(nameof(newOrder));

            NewOrder = newOrder;
        }
    }
}
