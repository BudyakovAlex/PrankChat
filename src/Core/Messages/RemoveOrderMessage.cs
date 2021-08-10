using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Messages
{
    public class RemoveOrderMessage : MvxMessage
    {
        public RemoveOrderMessage(object sender, int orderId, OrderStatusType status) : base(sender)
        {
            OrderId = orderId;
            Status = status;
        }

        public int OrderId { get; }

        public OrderStatusType Status { get; }
    }
}
