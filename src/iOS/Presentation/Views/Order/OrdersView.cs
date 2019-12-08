using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Order
{
    [MvxTabPresentation(TabName = "Orders", TabIconName = "unselected", TabSelectedIconName = "selected", WrapInNavigationController = true)]
    public partial class OrdersView : BaseTabbedView<OrdersViewModel>
    {
        public OrdersTableSource OrdersTableSource { get; private set; }

        protected override void SetupBinding()
		{
			var set = this.CreateBindingSet<OrdersView, OrdersViewModel>();

            set.Bind(OrdersTableSource)
                .To(vm => vm.Items);

            set.Apply();
		}

		protected override void SetupControls()
		{
            InitializeTableView();

            NavigationController.NavigationBar.SetNavigationBarStyle();
		}

        private void InitializeTableView()
        {
            OrdersTableSource = new OrdersTableSource(tableView);
            tableView.Source = OrdersTableSource;
            tableView.RegisterNibForCellReuse(OrderItemCell.Nib, OrderItemCell.CellId);
            tableView.SetStyle();
            tableView.RowHeight = OrderItemCell.EstimatedHeight;
            tableView.UserInteractionEnabled = true;
            tableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;

            tableView.SeparatorColor = Theme.Color.Separator;
            tableView.SeparatorStyle = UITableViewCellSeparatorStyle.DoubleLineEtched;
        }
    }
}