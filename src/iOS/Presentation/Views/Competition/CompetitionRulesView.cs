using MvvmCross.Binding.BindingContext;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition;
using PrankChat.Mobile.iOS.Presentation.Binding;
using PrankChat.Mobile.iOS.Presentation.Views.Base;

namespace PrankChat.Mobile.iOS.Presentation.Views.Competition
{
    public partial class CompetitionRulesView : BaseView<CompetitionRulesViewModel>
    {
        protected override void SetupControls()
        {
            base.SetupControls();

            Title = Resources.Competition_Rules;
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = this.CreateBindingSet<CompetitionRulesView, CompetitionRulesViewModel>();

            bindingSet.Bind(webView).For(WKWebViewHtmlStringTargetBinding.TargetBinding).To(vm => vm.HtmlContent);
        }
    }
}
