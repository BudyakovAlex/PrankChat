using MvvmCross.Plugin.Messenger;

namespace PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages
{
    public class RefreshTokenExpiredMessage : MvxMessage
    {
        public RefreshTokenExpiredMessage(object sender)
            : base(sender)
        {
        }
    }
}
