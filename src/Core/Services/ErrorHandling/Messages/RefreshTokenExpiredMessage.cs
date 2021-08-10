using MvvmCross.Plugin.Messenger;

namespace PrankChat.Mobile.Core.Services.ErrorHandling.Messages
{
    public class RefreshTokenExpiredMessage : MvxMessage
    {
        public RefreshTokenExpiredMessage(object sender)
            : base(sender)
        {
        }
    }
}
