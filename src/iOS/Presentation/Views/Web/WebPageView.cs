using System;
using MvvmCross.Binding.BindingContext;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.iOS.Presentation.Binding;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Web
{
    public partial class WebPageView : BaseView<WebViewModel>
    {
        protected override void SetupBinding()
        {
            var set = this.CreateBindingSet<WebPageView, WebViewModel>();

            set.Bind(webView)
                .For(WebViewUrlTargetBinding.TargetBinding)
                .To(vm => vm.Url);

            set.Apply();
        }
    }
}

