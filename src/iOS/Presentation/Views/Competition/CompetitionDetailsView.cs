using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items;
using PrankChat.Mobile.iOS.Presentation.SourcesAndDelegates;
using PrankChat.Mobile.iOS.Presentation.Views.Base;

namespace PrankChat.Mobile.iOS.Presentation.Views.Competition
{
    public partial class CompetitionDetailsView : BaseView<CompetitionDetailsViewModel>
    {
        private MvxUIRefreshControl _refreshControl;
        private VideoTableSource _source;

        protected override void SetupControls()
        {
            base.SetupControls();

            _source = new VideoTableSource(tableView);
            _source.Register<CompetitionDetailsHeaderViewModel>(CompetitionDetailsHeaderCell.Nib, CompetitionDetailsHeaderCell.CellId);
            _source.Register<CompetitionVideoViewModel>(CompetitionVideoCell.Nib, CompetitionVideoCell.CellId);

            lottieAnimationView.SetAnimationNamed("Animations/ripple_animation");
            lottieAnimationView.LoopAnimation = true;
            lottieAnimationView.Play();

            tableView.Source = _source;

            _refreshControl = new MvxUIRefreshControl();
            tableView.RefreshControl = _refreshControl;
        }

        protected override void SetupBinding()
        {
            base.SetupBinding();

            var bindingSet = this.CreateBindingSet<CompetitionDetailsView, CompetitionDetailsViewModel>();

            bindingSet.Bind(_source)
                      .For(v => v.ItemsSource)
                      .To(vm => vm.Items);

            bindingSet.Bind(loadingView)
                      .For(v => v.BindVisible())
                      .To(vm => vm.IsBusy);

            bindingSet.Bind(_source)
                      .For(v => v.LoadMoreItemsCommand)
                      .To(vm => vm.LoadMoreItemsCommand);

            bindingSet.Bind(_refreshControl)
                      .For(v => v.IsRefreshing)
                      .To(vm => vm.IsRefreshing);

            bindingSet.Bind(_refreshControl)
                      .For(v => v.RefreshCommand)
                      .To(vm => vm.RefreshDataCommand);

            bindingSet.Apply();
        }
    }
}
