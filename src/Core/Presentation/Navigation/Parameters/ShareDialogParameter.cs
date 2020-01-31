using System;

namespace PrankChat.Mobile.Core.Presentation.Navigation.Parameters
{
    public class ShareDialogParameter
    {
        public string Url { get; }

        public ShareDialogParameter(string url)
        {
            Url = url;
        }
    }
}
