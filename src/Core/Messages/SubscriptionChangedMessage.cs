using MvvmCross.Plugin.Messenger;

namespace PrankChat.Mobile.Core.Messages
{
    public class SubscriptionChangedMessage : MvxMessage
    {
        public SubscriptionChangedMessage(object sender) : base(sender)
        {
        }
    }
}