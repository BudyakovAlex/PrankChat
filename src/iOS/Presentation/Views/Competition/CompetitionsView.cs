using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Infrastructure.Helpers;
using PrankChat.Mobile.iOS.Presentation.SourcesAndDelegates;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Competition
{
    [MvxTabPresentation(TabName = "Competitions", TabIconName = "unselected", TabSelectedIconName = "selected", WrapInNavigationController = true)]
    public partial class CompetitionsView : BaseTabbedView<CompetitionsViewModel>
    {
        private TableViewSource _source;
        private MvxUIRefreshControl _refreshControl;

        protected override void SetupControls()
        {
            base.SetupControls();
            InitializeNavigationBar();

            _source = new TableViewSource(tableView)
                .Register<CompetitionsSectionViewModel>(CompetitionsSectionCell.Nib, CompetitionsSectionCell.CellId);

            tableView.Source = _source;
            tableView.ContentInset = new UIEdgeInsets(16f, 0, 0f, 0f);

            _refreshControl = new MvxUIRefreshControl
            {
                TintColor = UIColor.White
            };

            tableView.RefreshControl = _refreshControl;
        }

        private void InitializeNavigationBar()
        {
            NavigationController.NavigationBar.SetNavigationBarStyle();
            NavigationItem?.SetRightBarButtonItems(new UIBarButtonItem[]
            {
                NavigationItemHelper.CreateBarButton("ic_notification", ViewModel.ShowNotificationCommand),
                // TODO: This feature will be implemented.
                //NavigationItemHelper.CreateBarButton("ic_search", ViewModel.ShowSearchCommand)
            }, true);

            var logoButton = NavigationItemHelper.CreateBarButton("ic_logo", null);
            logoButton.Enabled = false;
            NavigationItem.LeftBarButtonItem = logoButton;
        }

        protected override void SetupBinding()
        {
            base.SetupBinding();

            var bindingSet = this.CreateBindingSet<CompetitionsView, CompetitionsViewModel>();

            bindingSet.Bind(_source)
                      .For(v => v.ItemsSource)
                      .To(vm => vm.Items);

            bindingSet.Bind(_refreshControl)
                      .For(v => v.IsRefreshing)
                      .To(vm => vm.IsBusy);

            bindingSet.Bind(_refreshControl)
                      .For(v => v.RefreshCommand)
                      .To(vm => vm.LoadDataCommand);

            bindingSet.Apply();
        }
    }
}
