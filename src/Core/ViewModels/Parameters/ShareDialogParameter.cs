namespace PrankChat.Mobile.Core.ViewModels.Parameters
{
    public class ShareDialogParameter
    {
        public ShareDialogParameter(string url)
        {
            Url = url;
        }

        public string Url { get; }
    }
}