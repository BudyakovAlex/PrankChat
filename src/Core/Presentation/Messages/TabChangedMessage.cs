using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Presentation.Messages
{
    public class TabChangedMessage : MvxMessage
    {
        public TabChangedMessage(object sender, MainTabType tabType) : base(sender)
        {
            TabType = tabType;
        }

        public MainTabType TabType { get; }
    }
}
