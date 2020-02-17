using System;
using System.ComponentModel;
using CoreGraphics;
using Foundation;
using WebKit;

namespace PrankChat.Mobile.iOS.Controls
{
    [Register("CustomWKWebView"), DesignTimeVisible(true)]
    public class CustomWKWebView : WKWebView, IWKNavigationDelegate
    {
        public CustomWKWebView(NSCoder coder) : base(coder)
        {
        }

        public CustomWKWebView(CGRect frame, WKWebViewConfiguration configuration) : base(frame, configuration)
        {
        }

        protected CustomWKWebView(NSObjectFlag t) : base(t)
        {
        }

        protected internal CustomWKWebView(IntPtr handle) : base(handle)
        {
        }

        [Export("webView:didReceiveServerRedirectForProvisionalNavigation:")]
        public virtual void DidReceiveServerRedirectForProvisionalNavigation(WebKit.WKWebView webView, WebKit.WKNavigation navigation)
        {

        }

		[Export("webView:decidePolicyForNavigationAction:decisionHandler:")]
		public void DecidePolicy(WKWebView webView, WKNavigationAction navigationAction, System.Action<WKNavigationActionPolicy> decisionHandler)
		{
		}
	}
}
