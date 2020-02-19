using System;
namespace PrankChat.Mobile.Core.Presentation.Navigation.Parameters
{
    public class WebViewNavigationParameter
    {
        public string Url { get; }

        public WebViewNavigationParameter(string url)
        {
            Url = url;
        }
    }
}
