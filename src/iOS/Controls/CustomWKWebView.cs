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
            Initilize();
        }

        public CustomWKWebView(CGRect frame, WKWebViewConfiguration configuration) : base(frame, configuration)
        {
            Initilize();
        }

        protected CustomWKWebView(NSObjectFlag t) : base(t)
        {
            Initilize();
        }

        protected internal CustomWKWebView(IntPtr handle) : base(handle)
        {
            Initilize();
        }

		[Export("webView:decidePolicyForNavigationAction:decisionHandler:")]
		public void DecidePolicy(WKWebView webView, WKNavigationAction navigationAction, System.Action<WKNavigationActionPolicy> decisionHandler)
		{
            decisionHandler(WKNavigationActionPolicy.Allow);
        }

        [Export("webView:didStartProvisionalNavigation:")]
        public void DidStartProvisionalNavigation(WKWebView webView, WKNavigation navigation)
        {
            // When navigation starts, this gets called
            Console.WriteLine("DidStartProvisionalNavigation");
        }

        private void Initilize()
        {
            var preferences = new WKPreferences()
            {
                JavaScriptEnabled = true,
                JavaScriptCanOpenWindowsAutomatically = true,
            };
     
            NavigationDelegate = this;
            Configuration.Preferences = preferences;
        }
    }
}
