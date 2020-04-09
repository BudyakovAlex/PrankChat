using System;
using Foundation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Arbitration.Items;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items;
using PrankChat.Mobile.iOS.Infrastructure;
using PrankChat.Mobile.iOS.Presentation.SourcesAndDelegates;
using PrankChat.Mobile.iOS.Presentation.Views.ArbitrationView;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Order
{
    public class OrdersTableSource : PagedTableViewSource
    {
        public OrdersTableSource(UITableView tableView)
            : base(tableView)
        {
            UseAnimations = true;

            tableView.RegisterNibForCellReuse(OrderItemCell.Nib, OrderItemCell.CellId);
            tableView.RegisterNibForCellReuse(ArbitrationItemCell.Nib, ArbitrationItemCell.CellId);
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            if (item is OrderItemViewModel)
            {
                return tableView.DequeueReusableCell(OrderItemCell.CellId);
            }

            if (item is ArbitrationItemViewModel)
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
