using MvvmCross.Binding.BindingContext;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items;
using PrankChat.Mobile.iOS.Presentation.SourcesAndDelegates;
using PrankChat.Mobile.iOS.Presentation.Views.Base;

namespace PrankChat.Mobile.iOS.Presentation.Views.Competition
{
    public partial class CompetitionPrizePoolView : BaseView<CompetitionPrizePoolViewModel>
    {
        private TableViewSource _source;

        protected override void SetupControls()
        {
            base.SetupControls();

            Title = Resources.Competition_Results;

            titleLabel.Text = Resources.Competition_Prize_Pool;
            participantLabel.Text = Resources.Competition_Prize_Pool_Participant;
            ratingLabel.Text = Resources.Competition_Prize_Pool_Rating;

            _source = new TableViewSource(tableView)
                .Register<CompetitionPrizePoolItemViewModel>(CompetitionPrizePoolItemCell.Nib, CompetitionPrizePoolItemCell.CellId);

            tableView.Source = _source;
            tableView.RowHeight = CompetitionPrizePoolItemCell.Height;
        }

        protected override void SetupBinding()
        {
            base.SetupBinding();

            var bindingSet = this.CreateBindingSet<CompetitionPrizePoolView, CompetitionPrizePoolViewModel>();

            bindingSet.Bind(prizePoolLabel)
                      .For(v => v.Text)
                      .To(vm => vm.PrizePool);

            bindingSet.Bind(_source)
                      .For(v => v.ItemsSource)
                      .To(vm => vm.Items);

            bindingSet.Apply();
        }
    }
}
