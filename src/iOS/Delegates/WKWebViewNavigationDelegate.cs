using System;
using WebKit;

namespace PrankChat.Mobile.iOS.Delegates
{
    public class WKWebViewNavigationDelegate : WKNavigationDelegate
    {
        private Action<string> _action;

        public WKWebViewNavigationDelegate(Action<string> action)
        {
            _action = action;
        }

        public override void DidStartProvisionalNavigation(WKWebView webView, WKNavigation navigation)
        {
            _action?.Invoke(webView.Url.ToString());
        }
    }
}
