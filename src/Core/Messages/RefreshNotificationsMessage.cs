using MvvmCross.Plugin.Messenger;

namespace PrankChat.Mobile.Core.Messages
{
    public class RefreshNotificationsMessage : MvxMessage
    {
        public RefreshNotificationsMessage(object sender) : base(sender)
        {
        }
    }
}