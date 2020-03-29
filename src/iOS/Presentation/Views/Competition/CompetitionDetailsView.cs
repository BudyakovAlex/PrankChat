using MvvmCross.Binding.BindingContext;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items;
using PrankChat.Mobile.iOS.Presentation.SourcesAndDelegates;
using PrankChat.Mobile.iOS.Presentation.Views.Base;

namespace PrankChat.Mobile.iOS.Presentation.Views.Competition
{
    public partial class CompetitionDetailsView : BaseView<CompetitionDetailsViewModel>
    {
        private TableViewSource _source;

        protected override void SetupControls()
        {
            base.SetupControls();

            _source = new TableViewSource(tableView)
                .Register<CompetitionDetailsHeaderViewModel>(CompetitionDetailsHeaderCell.Nib, CompetitionDetailsHeaderCell.CellId)
                .Register<CompetitionVideoViewModel>(CompetitionVideoCell.Nib, CompetitionVideoCell.CellId);

            tableView.Source = _source;
        }

        protected override void SetupBinding()
        {
            base.SetupBinding();

            var bindingSet = this.CreateBindingSet<CompetitionDetailsView, CompetitionDetailsViewModel>();

            bindingSet.Bind(_source)
                      .For(v => v.ItemsSource)
                      .To(vm => vm.Items);

            bindingSet.Bind(_source)
                      .For(v => v.LoadMoreItemsCommand)
                      .To(vm => vm.LoadMoreItemsCommand);

            bindingSet.Apply();
        }
    }
}
