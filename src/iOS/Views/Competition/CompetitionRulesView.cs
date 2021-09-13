using MvvmCross.Binding.BindingContext;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.ViewModels.Competition;
using PrankChat.Mobile.iOS.Binding;
using PrankChat.Mobile.iOS.Views.Base;

namespace PrankChat.Mobile.iOS.Views.Competition
{
    public partial class CompetitionRulesView : BaseView<CompetitionRulesViewModel>
    {
        protected override void SetupControls()
        {
            base.SetupControls();

            Title = Resources.CompetitionRules;
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = this.CreateBindingSet<CompetitionRulesView, CompetitionRulesViewModel>();

            bindingSet.Bind(webView).For(v => v.BindHtmlString()).To(vm => vm.HtmlContent);
        }
    }
}
