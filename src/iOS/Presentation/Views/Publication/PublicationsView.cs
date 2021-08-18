using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Binding.Views.Gestures;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Infrastructure;
using PrankChat.Mobile.iOS.Infrastructure.Helpers;
using PrankChat.Mobile.iOS.Presentation.Converters;
using PrankChat.Mobile.iOS.Presentation.SourcesAndDelegates;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;
using Xamarin.Essentials;

namespace PrankChat.Mobile.iOS.Presentation.Views.Publication
{
    [MvxTabPresentation(TabName = "Publications", TabIconName = "unselected", TabSelectedIconName = "selected")]
    public partial class PublicationsView : BaseRefreshableTabbedView<PublicationsViewModel>, IScrollableView
    {
        private MvxUIRefreshControl _refreshControl;
        private UIBarButtonItem _notificationBarItem;

        public VideoTableSource PublicationTableSource { get; private set; }

        public UIScrollView ScrollView => tableView;

        protected override void Bind()
		{
			using var bindingSet = this.CreateBindingSet<PublicationsView, PublicationsViewModel>();

            bindingSet.Bind(filterContainerView.Tap()).For(v => v.Command).To(vm => vm.OpenFilterCommand);
            bindingSet.Bind(filterTitleLabel).To(vm => vm.ActiveFilterName);

            bindingSet.Bind(PublicationTableSource).To(vm => vm.Items);
            bindingSet.Bind(PublicationTableSource).For(v => v.ItemsChangedInteraction).To(vm => vm.ItemsChangedInteraction);
            bindingSet.Bind(PublicationTableSource).For(v => v.LoadMoreItemsCommand).To(vm => vm.LoadMoreItemsCommand);

            bindingSet.Bind(loadingOverlayView).For(v => v.BindVisible()).To(vm => vm.IsRefreshingData);
            bindingSet.Bind(_refreshControl).For(v => v.IsRefreshing).To(vm => vm.IsBusy);
            bindingSet.Bind(_refreshControl).For(v => v.RefreshCommand).To(vm => vm.ReloadItemsCommand);

            bindingSet.Bind(_notificationBarItem).For(v => v.Image).To(vm => vm.NotificationBadgeViewModel.HasUnreadNotifications).WithConversion<BoolToNotificationImageConverter>();
		}

		protected override void SetupControls()
		{
			InitializeNavigationBar();
            InitializeTableView();

            publicationTypeStackView.SetTabsStyle(new string[] {
                Resources.Popular_Publication_Tab,
                Resources.Actual_Publication_Tab,
				Resources.MyFeed_Publication_Tab,
			}, OnTabSelected);

            topSeparatorView.BackgroundColor = Theme.Color.Separator;
            filterArrowImageView.Image = UIImage.FromBundle("ic_filter_arrow");
            filterTitleLabel.Font = Theme.Font.RegularFontOfSize(14);

            lottieAnimationView.SetAnimationNamed("Animations/ripple_animation");
            lottieAnimationView.LoopAnimation = true;
            lottieAnimationView.Play();
        }

        protected override void RefreshData()
        {
            ViewModel?.ReloadItemsCommand.Execute();
            MainThread.BeginInvokeOnMainThread(() =>
                ViewModel.SafeExecutionWrapper.Wrap(() =>
                tableView.SetContentOffset(new CGPoint(0, -_refreshControl.Frame.Height), true)));
        }

        private void OnTabSelected(int position)
        {
            publicationTypeStackView.SetSelectedTabStyle(position);
            tableView.SetContentOffset(new CGPoint(0, 0), false);

            filterView.Hidden = position != 0;
            bottomSeparatorView.Hidden = filterView.Hidden;

            ViewModel.SelectedPublicationType = (PublicationType)position;
        }

        private void InitializeNavigationBar()
		{
			NavigationController.NavigationBar.SetNavigationBarStyle();

            _notificationBarItem = NavigationItemHelper.CreateBarButton("ic_notification", ViewModel.ShowNotificationCommand);
            NavigationItem?.SetRightBarButtonItems(new UIBarButtonItem[]
            {
                _notificationBarItem,
                NavigationItemHelper.CreateBarButton("ic_search", ViewModel.ShowSearchCommand)
            }, true);

            var logoButton = NavigationItemHelper.CreateBarButton("ic_logo", null);
            logoButton.Enabled = false;
            NavigationItem.LeftBarButtonItem = logoButton;
        }

        private void InitializeTableView()
        {
            PublicationTableSource = new VideoTableSource(tableView);
            PublicationTableSource.Register<PublicationItemViewModel>(PublicationItemCell.Nib, PublicationItemCell.CellId);

            tableView.Source = PublicationTableSource;
            tableView.SetVideoListStyle(Constants.CellHeights.PublicationItemCellHeight);

            _refreshControl = new MvxUIRefreshControl();
            tableView.RefreshControl = _refreshControl;
        }
    }
}