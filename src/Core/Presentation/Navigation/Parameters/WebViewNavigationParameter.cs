namespace PrankChat.Mobile.Core.Presentation.Navigation.Parameters
{
    public class WebViewNavigationParameter
    {
        public WebViewNavigationParameter(string url)
        {
            Url = url;
        }

        public string Url { get; }
    }
}