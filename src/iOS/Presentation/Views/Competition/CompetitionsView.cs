using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Infrastructure.Helpers;
using PrankChat.Mobile.iOS.Presentation.Converters;
using PrankChat.Mobile.iOS.Presentation.SourcesAndDelegates;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;
using Xamarin.Essentials;

namespace PrankChat.Mobile.iOS.Presentation.Views.Competition
{
    [MvxTabPresentation(TabName = "Competitions", TabIconName = "unselected", TabSelectedIconName = "selected", WrapInNavigationController = true)]
    public partial class CompetitionsView : BaseRefreshableTabbedView<CompetitionsViewModel>, IScrollableView
    {
        private TableViewSource _source;
        private MvxUIRefreshControl _refreshControl;
        private UIBarButtonItem _notificationBarItem;

        public UIScrollView ScrollView => tableView;

        protected override void SetupControls()
        {
            DefinesPresentationContext = true;

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

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = this.CreateBindingSet<CompetitionsView, CompetitionsViewModel>();

            bindingSet.Bind(_source).For(v => v.ItemsSource).To(vm => vm.Items);
            bindingSet.Bind(_refreshControl).For(v => v.IsRefreshing).To(vm => vm.IsBusy);
            bindingSet.Bind(_refreshControl).For(v => v.RefreshCommand).To(vm => vm.LoadDataCommand);
            bindingSet.Bind(_notificationBarItem).For(v => v.Image).To(vm => vm.NotificationBadgeViewModel.HasUnreadNotifications)
                      .WithConversion<BoolToNotificationImageConverter>();
        }

        protected override void RefreshData()
        {
            ViewModel?.LoadDataCommand.Execute();
            MainThread.BeginInvokeOnMainThread(() =>
                ViewModel.SafeExecutionWrapper.Wrap(() =>
                tableView.SetContentOffset(new CGPoint(0, -_refreshControl.Frame.Height), true)));
        }

        private void InitializeNavigationBar()
        {
            NavigationController.NavigationBar.SetNavigationBarStyle();

            _notificationBarItem = NavigationItemHelper.CreateBarButton("ic_notification", ViewModel.ShowNotificationCommand);
            NavigationItem?.SetRightBarButtonItems(new UIBarButtonItem[]
            {
               _notificationBarItem,
                NavigationItemHelper.CreateBarButton("ic_info", ViewModel.ShowWalkthrouthCommand),
                // TODO: This feature will be implemented.
                //NavigationItemHelper.CreateBarButton("ic_search", ViewModel.ShowSearchCommand)
            }, true);

            var logoButton = NavigationItemHelper.CreateBarButton("ic_logo", null);
            logoButton.Enabled = false;
            NavigationItem.LeftBarButtonItem = logoButton;
        }
    }
}
