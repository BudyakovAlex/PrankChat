using System;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Presentation.Messages
{
    public class RemoveOrderMessage : MvxMessage
    {
        public int OrderId { get; }

        public OrderStatusType Status { get; }

        public RemoveOrderMessage(object sender, int orderId, OrderStatusType status) : base(sender)
        {
            OrderId = orderId;
            Status = status;
        }
    }
}
