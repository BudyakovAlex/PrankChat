using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Webkit;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views.Competitions
{
    [MvxActivityPresentation]
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class CompetitionRulesView : BaseView<CompetitionRulesViewModel>
    {
        private WebView _webView;

        protected override bool HasBackButton => true;

        protected override string TitleActionBar => Core.Presentation.Localization.Resources.Competition_Rules;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.activity_competition_rules);
        }

        protected override void SetViewProperties()
        {
            _webView = FindViewById<WebView>(Resource.Id.web_view);
        }

        protected override void DoBind()
        {
            base.DoBind();

            var bindingSet = this.CreateBindingSet<CompetitionRulesView, CompetitionRulesViewModel>();

            bindingSet.Bind(_webView).For(v => v.BindWebViewHtml()).To(vm => vm.HtmlContent);

            bindingSet.Apply();
        }
    }
}