using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.ViewModels.Competition;
using PrankChat.Mobile.iOS.Views.Base;

namespace PrankChat.Mobile.iOS.Views.Competition
{
    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class CompetitonStatitsticsView : BaseViewController<CompetitonStatitsticsViewModel>
    {
        protected override void SetupControls()
        {
            base.SetupControls();

            Title = Resources.Statistics;
            ProfitTitleLabel.Text = Resources.Profit;
            ParticipantsTitleLabel.Text = Resources.Participants;
            PercentageFromContributionTitleLabel.Text = $"% {Resources.OfTheInstallment}";
            ContributionTitleLabel.Text = Resources.Fee;
        }

        protected override void Bind()
        {
            using var bindingSet = CreateBindingSet();

            bindingSet.Bind(ProfitLabel).For(v => v.Text).To(vm => vm.Profit);
            bindingSet.Bind(ParticipantsLabel).For(v => v.Text).To(vm => vm.Participants);
            bindingSet.Bind(PercentageFromContributionValueLabel).For(v => v.Text).To(vm => vm.PercentageFromContribution);
            bindingSet.Bind(ContributionValueLabel).For(v => v.Text).To(vm => vm.Contribution);
        }
    }
}

