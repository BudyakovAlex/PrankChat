using Foundation;
using PrankChat.Mobile.Core.ViewModels.Common;
using PrankChat.Mobile.iOS.Delegates;
using PrankChat.Mobile.iOS.Views.Base;
using WebKit;

namespace PrankChat.Mobile.iOS.Views.Web
{
    public partial class WebPageView : BaseViewController<WebViewModel>, IWKNavigationDelegate
    {
        protected override void SetupControls()
        {
            base.SetupControls();

            var nsurl = NSUrl.FromString(ViewModel.Url);
            webView.LoadRequest(new NSUrlRequest(nsurl));
            webView.NavigationDelegate = new WKWebViewNavigationDelegate(url => ViewModel.Url = url);
        }
    }
}

