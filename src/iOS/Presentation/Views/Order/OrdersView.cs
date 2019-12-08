using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views.Gestures;
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

            set.Bind(filterContainerView.Tap())
                .For(v => v.Command)
                .To(vm => vm.OpenFilterCommand);

            set.Bind(filterTitleLabel)
                .To(vm => vm.ActiveFilterName);

            set.Apply();
		}

		protected override void SetupControls()
		{
            InitializeTableView();

            NavigationController.NavigationBar.SetNavigationBarStyle();

            filterArrowImageView.Image = UIImage.FromBundle("ic_filter_arrow");
            filterTitleLabel.Font = Theme.Font.RegularFontOfSize(14);
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