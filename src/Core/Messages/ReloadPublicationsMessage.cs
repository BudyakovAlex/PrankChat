using System.Collections.Generic;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ViewModels.Results;

namespace PrankChat.Mobile.Core.Messages
{
    public class ReloadPublicationsMessage : MvxMessage
    {
        public Dictionary<int, FullScreenVideoResult> UpdatedItemsDictionary { get; }

        public ReloadPublicationsMessage(object sender)
            : base(sender)
        {
        }

        public ReloadPublicationsMessage(object sender, Dictionary<int, FullScreenVideoResult> updatedItems)
            : this(sender)
        {
            UpdatedItemsDictionary = updatedItems;
        }
    }
}