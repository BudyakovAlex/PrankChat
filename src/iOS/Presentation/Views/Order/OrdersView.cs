using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views.Gestures;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Infrastructure.Helpers;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Order
{
    [MvxTabPresentation(TabName = "Orders", TabIconName = "unselected", TabSelectedIconName = "selected", WrapInNavigationController = true)]
    public partial class OrdersView : BaseTabbedView<OrdersViewModel>
    {
        private MvxUIRefreshControl _refreshControl;

        public OrdersTableSource OrdersTableSource { get; private set; }

        protected override void SetupBinding()
		{
			var set = this.CreateBindingSet<OrdersView, OrdersViewModel>();

            set.Bind(OrdersTableSource)
                .To(vm => vm.Items);

            set.Bind(filterContainerView.Tap())
                .For(v => v.Command)
                .To(vm => vm.OpenFilterCommand);

            set.Bind(filterTitleLabel)
                .To(vm => vm.ActiveFilterName);

            set.Bind(_refreshControl)
                .For(v => v.IsRefreshing)
                .To(vm => vm.IsBusy);

            set.Bind(_refreshControl)
                .For(v => v.IsRefreshing)
                .To(vm => vm.IsBusy);

            set.Bind(_refreshControl)
                .For(v => v.RefreshCommand)
                .To(vm => vm.ReloadItemsCommand);

            set.Bind(OrdersTableSource)
                .For(v => v.LoadMoreItemsCommand)
                .To(vm => vm.LoadMoreItemsCommand);

            set.Apply();
		}

		protected override void SetupControls()
		{
            Title = Resources.OrdersView_Title_Label;

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
            ratingTabLabel.Text = Resources.Rate_Tab;

            ApplySelectedTabStyle(0);
        }

        private void InitializeTableView()
        {
            OrdersTableSource = new OrdersTableSource(tableView);
            tableView.Source = OrdersTableSource;
            tableView.SetStyle();
            tableView.RowHeight = OrderItemCell.EstimatedHeight;
            tableView.UserInteractionEnabled = true;
            tableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;

            tableView.SeparatorColor = Theme.Color.Separator;
            tableView.SeparatorStyle = UITableViewCellSeparatorStyle.DoubleLineEtched;

            _refreshControl = new MvxUIRefreshControl();
            tableView.RefreshControl = _refreshControl;
        }

        private void InitializeNavigationBar()
        {
            NavigationItem?.SetRightBarButtonItems(new UIBarButtonItem[]
            {
                NavigationItemHelper.CreateBarButton("ic_notification", ViewModel.ShowNotificationCommand),
                // TODO: This feature will be implemented.
                //NavigationItemHelper.CreateBarButton("ic_search", ViewModel.ShowSearchCommand)
            }, true);
        }

        private void SetSelectedTab(int index)
        {
            ApplySelectedTabStyle(index);

            switch (index)
            {
                case 0:
                    ViewModel.TabType = OrdersTabType.Order;
                    break;

                case 1:
                    ViewModel.TabType = OrdersTabType.Rating;
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