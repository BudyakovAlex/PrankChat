using Android.App;
using Android.Content.PM;
using Android.OS;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.ViewModels.Common;
using PrankChat.Mobile.Droid.Controls.WebView;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views
{
    [MvxActivityPresentation]
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class WebPageView : BaseView<WebViewModel>
    {
        
        private BindableWebView _bindableWebView;

        protected override bool HasBackButton => true;

        protected override void OnCreate(Bundle bundle)
        {
            Window.SetBackgroundDrawableResource(Resource.Drawable.gradient_action_bar_background);
            base.OnCreate(bundle, Resource.Layout.activity_web_page);
        }

        protected override void SetViewProperties()
        {
            base.SetViewProperties();

            _bindableWebView = FindViewById<BindableWebView>(Resource.Id.webView);
        }

        protected override void Bind()
        {
            base.Bind();
            using var bindingSet = CreateBindingSet();

            bindingSet.Bind(_bindableWebView).For(v => v.WebViewUrlContent).To(vm => vm.Url);
        }
    }
}
