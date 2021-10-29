using System;
using Foundation;
using PrankChat.Mobile.Core.ViewModels.Arbitration.Items;
using PrankChat.Mobile.Core.ViewModels.Order.Items;
using PrankChat.Mobile.iOS.Infrastructure;
using PrankChat.Mobile.iOS.SourcesAndDelegates;
using PrankChat.Mobile.iOS.Views.ArbitrationView;
using UIKit;

namespace PrankChat.Mobile.iOS.Views.Order
{
    public class OrdersTableSource : PagedTableViewSource
    {
        private readonly Action _scrollAction;

        public OrdersTableSource(UITableView tableView, Action scrollAction = null)
            : base(tableView)
        {
            UseAnimations = true;

            tableView.RegisterNibForCellReuse(OrderItemCell.Nib, OrderItemCell.CellId);
            tableView.RegisterNibForCellReuse(ArbitrationItemCell.Nib, ArbitrationItemCell.CellId);
            _scrollAction = scrollAction;
        }

        public override void Scrolled(UIScrollView scrollView) =>
            _scrollAction?.Invoke();

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            if (item is OrderItemViewModel)
            {
                return tableView.DequeueReusableCell(OrderItemCell.CellId);
            }

            if (item is ArbitrationOrderItemViewModel)
            {
                return tableView.DequeueReusableCell(ArbitrationItemCell.CellId);
            }

            return null;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return Constants.CellHeights.OrderItemCellHeight;
        }
    }
}
