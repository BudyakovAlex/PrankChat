using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views.Gestures;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Infrastructure;
using PrankChat.Mobile.iOS.Infrastructure.Helpers;
using PrankChat.Mobile.iOS.Presentation.Converters;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;
using Xamarin.Essentials;

namespace PrankChat.Mobile.iOS.Presentation.Views.Order
{
    [MvxTabPresentation(TabName = "Orders", TabIconName = "unselected", TabSelectedIconName = "selected", WrapInNavigationController = true)]
    public partial class OrdersView : BaseRefreshableTabbedView<OrdersViewModel>, IScrollableView
    {
        private MvxUIRefreshControl _refreshControl;
        private UIBarButtonItem _notificationBarItem;

        public OrdersTableSource OrdersTableSource { get; private set; }

        public UITableView TableView => tableView;

        protected override void SetupBinding()
		{
			var bindingSet = this.CreateBindingSet<OrdersView, OrdersViewModel>();

            bindingSet.Bind(OrdersTableSource).To(vm => vm.Items);
            bindingSet.Bind(filterContainerView.Tap()).For(v => v.Command).To(vm => vm.OpenFilterCommand);
            bindingSet.Bind(filterTitleLabel).To(vm => vm.ActiveFilterName);
            bindingSet.Bind(_refreshControl).For(v => v.IsRefreshing).To(vm => vm.IsBusy);
            bindingSet.Bind(_refreshControl).For(v => v.RefreshCommand).To(vm => vm.ReloadItemsCommand);
            bindingSet.Bind(OrdersTableSource).For(v => v.LoadMoreItemsCommand).To(vm => vm.LoadMoreItemsCommand);
            bindingSet.Bind(_notificationBarItem).For(v => v.Image).To(vm => vm.NotificationBageViewModel.HasUnreadNotifications).WithConversion<BoolToNotificationImageConverter>();

            bindingSet.Apply();
		}

		protected override void SetupControls()
		{
            DefinesPresentationContext = true;

            InitializeTableView();
            InitializeNavigationBar();

            NavigationController.NavigationBar.SetNavigationBarStyle();

            filterArrowImageView.Image = UIImage.FromBundle("ic_filter_arrow");
            filterTitleLabel.Font = Theme.Font.RegularFontOfSize(14);

            orderTabLabel.UserInteractionEnabled = true;
            orderTabLabel.AddGestureRecognizer(new UITapGestureRecognizer(_ => SetSelectedTab(0)));
            orderTabLabel.Text = Resources.Orders_Tab;

            ratingTabLabel.UserInteractionEnabled = true;
            ratingTabLabel.AddGestureRecognizer(new UITapGestureRecognizer(_ => SetSelectedTab(1)));
            ratingTabLabel.Text = Resources.Orders_In_Dispute;

            ApplySelectedTabStyle(0);
        }

        protected override void RefreshData()
        {
            ViewModel?.ReloadItemsCommand.Execute();
            MainThread.BeginInvokeOnMainThread(() => TableView.SetContentOffset(new CGPoint(0, -_refreshControl.Frame.Height), true));
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
            _notificationBarItem = NavigationItemHelper.CreateBarButton("ic_notification", ViewModel.ShowNotificationCommand);
            NavigationItem?.SetRightBarButtonItems(new UIBarButtonItem[]
            {
                _notificationBarItem,
                NavigationItemHelper.CreateBarButton("ic_info", ViewModel.ShowWalkthrouthCommand)
                // TODO: This feature will be implemented.
                //NavigationItemHelper.CreateBarButton("ic_search", ViewModel.ShowSearchCommand)
            }, true);

            var logoButton = NavigationItemHelper.CreateBarButton("ic_logo", null);
            logoButton.Enabled = false;
            NavigationItem.LeftBarButtonItem = logoButton;
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