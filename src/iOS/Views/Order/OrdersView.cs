using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views.Gestures;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.ViewModels.Order;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Infrastructure;
using PrankChat.Mobile.iOS.Infrastructure.Helpers;
using PrankChat.Mobile.iOS.Converters;
using PrankChat.Mobile.iOS.Views.Base;
using UIKit;
using Xamarin.Essentials;
using PrankChat.Mobile.iOS.Common;

namespace PrankChat.Mobile.iOS.Views.Order
{
    [MvxTabPresentation(TabName = "Orders", TabIconName = ImageNames.IconUnselected, TabSelectedIconName = ImageNames.IconSelected, WrapInNavigationController = true)]
    public partial class OrdersView : BaseRefreshableTabbedViewController<OrdersViewModel>, IScrollableView
    {
        private MvxUIRefreshControl _refreshControl;
        private UIBarButtonItem _notificationBarItem;

        public OrdersTableSource OrdersTableSource { get; private set; }

        public UIScrollView ScrollView => tableView;

        protected override void Bind()
		{
			using var bindingSet = this.CreateBindingSet<OrdersView, OrdersViewModel>();

            bindingSet.Bind(OrdersTableSource).To(vm => vm.Items);
            bindingSet.Bind(filterContainerView.Tap()).For(v => v.Command).To(vm => vm.OpenFilterCommand);
            bindingSet.Bind(filterTitleLabel).To(vm => vm.ActiveFilterName);
            bindingSet.Bind(_refreshControl).For(v => v.IsRefreshing).To(vm => vm.IsBusy);
            bindingSet.Bind(_refreshControl).For(v => v.RefreshCommand).To(vm => vm.ReloadItemsCommand);
            bindingSet.Bind(OrdersTableSource).For(v => v.LoadMoreItemsCommand).To(vm => vm.LoadMoreItemsCommand);
            bindingSet.Bind(_notificationBarItem).For(v => v.Image).To(vm => vm.NotificationBadgeViewModel.HasUnreadNotifications).WithConversion<BoolToNotificationImageConverter>();
		}

		protected override void SetupControls()
		{
            DefinesPresentationContext = true;

            InitializeTableView();
            InitializeNavigationBar();

            NavigationController.NavigationBar.SetNavigationBarStyle();

            filterArrowImageView.Image = UIImage.FromBundle(ImageNames.IconFilterArrow);
            filterTitleLabel.Font = Theme.Font.RegularFontOfSize(14);

            orderTabLabel.UserInteractionEnabled = true;
            orderTabLabel.AddGestureRecognizer(new UITapGestureRecognizer(_ => SetSelectedTab(0)));
            orderTabLabel.Text = Resources.Orders;

            ratingTabLabel.UserInteractionEnabled = true;
            ratingTabLabel.AddGestureRecognizer(new UITapGestureRecognizer(_ => SetSelectedTab(1)));
            ratingTabLabel.Text = Resources.InDispute;

            ApplySelectedTabStyle(0);
        }

        protected override void RefreshData()
        {
            ViewModel?.ReloadItemsCommand.Execute();
            MainThread.BeginInvokeOnMainThread(() =>
                ViewModel?.SafeExecutionWrapper.Wrap(() =>
                tableView.SetContentOffset(new CGPoint(0, -_refreshControl.Frame.Height), true)));
        }

        private void InitializeTableView()
        {
            OrdersTableSource = new OrdersTableSource(tableView);
            tableView.Source = OrdersTableSource;
            tableView.SetStyle();
            tableView.RowHeight = Constants.CellHeights.OrderItemCellHeight;

            tableView.UserInteractionEnabled = true;
            tableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;

            tableView.SeparatorColor = Theme.Color.Separator;
            tableView.SeparatorStyle = UITableViewCellSeparatorStyle.DoubleLineEtched;

            _refreshControl = new MvxUIRefreshControl();
            tableView.RefreshControl = _refreshControl;
        }

        private void InitializeNavigationBar()
        {
            _notificationBarItem = NavigationItemHelper.CreateBarButton(ImageNames.IconNotification, ViewModel.ShowNotificationCommand);
            NavigationItem?.SetRightBarButtonItems(new UIBarButtonItem[]
            {
                _notificationBarItem,
                NavigationItemHelper.CreateBarButton(ImageNames.IconInfo, ViewModel.ShowWalkthrouthCommand)
                // TODO: This feature will be implemented.
                //NavigationItemHelper.CreateBarButton("ic_search", ViewModel.ShowSearchCommand)
            }, true);

            NavigationItem.LeftBarButtonItem = NavigationItemHelper.CreateBarLogoButton();
        }

        private void SetSelectedTab(int index)
        {
            tableView.SetContentOffset(new CGPoint(0, 0), false);

            ApplySelectedTabStyle(index);

            switch (index)
            {
                case 0:
                    ViewModel.TabType = OrdersTabType.Order;
                    break;

                case 1:
                    ViewModel.TabType = OrdersTabType.Arbitration;
                    break;
            }
        }

        private void ApplySelectedTabStyle(int index)
        {
            var isFirstTabSelected = index == 0;

            orderTabIndicator.Hidden = !isFirstTabSelected;
            ratingTabIndicator.Hidden = isFirstTabSelected;

            if (isFirstTabSelected)
            {
                orderTabLabel.SetMainTitleStyle();
                ratingTabLabel.SetTitleStyle();
            }
            else
            {
                ratingTabLabel.SetMainTitleStyle();
                orderTabLabel.SetTitleStyle();
            }
        }
    }
}