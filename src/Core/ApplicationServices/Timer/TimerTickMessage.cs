using MvvmCross.Plugin.Messenger;

namespace PrankChat.Mobile.Core.ApplicationServices.Timer
{
    public class TimerTickMessage : MvxMessage
    {
        public TimerTickMessage(object sender) : base(sender)
        {
        }
    }
}
