using MvvmCross.Plugin.Messenger;

namespace PrankChat.Mobile.Core.Presentation.Messengers
{
    public class UpdateUserProfileMessenger : MvxMessage
    {
        public UpdateUserProfileMessenger(object sender) : base(sender)
        {
        }
    }
}
