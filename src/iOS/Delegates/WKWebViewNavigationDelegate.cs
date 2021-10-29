using System;
using WebKit;

namespace PrankChat.Mobile.iOS.Delegates
{
    public class WKWebViewNavigationDelegate : WKNavigationDelegate
    {
        private Action<string> _navigationAction;

        public WKWebViewNavigationDelegate(Action<string> navigaitonAction)
        {
            _navigationAction = navigaitonAction;
        }

        public override void DidStartProvisionalNavigation(WKWebView webView, WKNavigation navigation)
        {
            _navigationAction?.Invoke(webView.Url.ToString());
        }
    }
}
