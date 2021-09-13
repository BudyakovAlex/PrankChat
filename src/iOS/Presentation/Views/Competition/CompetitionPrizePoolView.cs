using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.ViewModels.Competition;
using PrankChat.Mobile.Core.ViewModels.Competition.Items;
using PrankChat.Mobile.iOS.Presentation.SourcesAndDelegates;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Competition
{
    public partial class CompetitionPrizePoolView : BaseView<CompetitionPrizePoolViewModel>
    {
        private TableViewSource _source;
        private MvxUIRefreshControl _refreshControl;

        protected override void SetupControls()
        {
            base.SetupControls();

            Title = Resources.Results;

            titleLabel.Text = Resources.PrizePool;
            participantLabel.Text = Resources.Participant;
            votingLabel.Text = Resources.Vote;
            prizeLabel.Text = Resources.Prize;

            _source = new TableViewSource(tableView)
                .Register<CompetitionPrizePoolItemViewModel>(CompetitionPrizePoolItemCell.Nib, CompetitionPrizePoolItemCell.CellId);

            tableView.Source = _source;
            tableView.RowHeight = CompetitionPrizePoolItemCell.Height;

            _refreshControl = new MvxUIRefreshControl
            {
                TintColor = UIColor.White
            };

            tableView.RefreshControl = _refreshControl;
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = this.CreateBindingSet<CompetitionPrizePoolView, CompetitionPrizePoolViewModel>();

            bindingSet.Bind(prizePoolLabel).For(v => v.Text).To(vm => vm.PrizePool);
            bindingSet.Bind(_source).For(v => v.ItemsSource).To(vm => vm.Items);
            bindingSet.Bind(_refreshControl).For(v => v.IsRefreshing).To(vm => vm.IsBusy);
            bindingSet.Bind(_refreshControl).For(v => v.RefreshCommand).To(vm => vm.RefreshCommand);
        }
    }
}
