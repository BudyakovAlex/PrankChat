namespace PrankChat.Mobile.Core.Presentation.ViewModels.Parameters
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