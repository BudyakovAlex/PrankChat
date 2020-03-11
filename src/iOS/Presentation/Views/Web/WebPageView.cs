using System;
using Foundation;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.iOS.Presentation.Views.Base;

namespace PrankChat.Mobile.iOS.Presentation.Views.Web
{
    public partial class WebPageView : BaseView<WebViewModel>
    {
        protected override void SetupControls()
        {
            base.SetupControls();

            var nsurl = NSUrl.FromString(ViewModel.Url);
            webView.LoadRequest(new NSUrlRequest(nsurl));
        }
    }
}

