using MvvmCross.Plugin.Messenger;

namespace PrankChat.Mobile.Core.Services.Timer
{
    public class TimerTickMessage : MvxMessage
    {
        public TimerTickMessage(object sender) : base(sender)
        {
        }
    }
}
