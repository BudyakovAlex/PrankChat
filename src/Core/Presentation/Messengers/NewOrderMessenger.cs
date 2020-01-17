using System;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Presentation.Messengers
{
    public class NewOrderMessenger : MvxMessage
    {
        public OrderDataModel NewOrder { get; }

        public NewOrderMessenger(object sender, OrderDataModel newOrder) : base(sender)
        {
            NewOrder = newOrder;
        }
    }
}
