using System;
using Foundation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items;
using PrankChat.Mobile.Core.Presentation.ViewModels.Rating.Items;
using PrankChat.Mobile.iOS.Presentation.SourcesAndDelegates;
using PrankChat.Mobile.iOS.Presentation.Views.RatingView;
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
            tableView.RegisterNibForCellReuse(RatingItemCell.Nib, RatingItemCell.CellId);
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            if (item is OrderItemViewModel)
            {
                return tableView.DequeueReusableCell(OrderItemCell.CellId);
            }

            if (item is RatingItemViewModel)
            {
                return tableView.DequeueReusableCell(RatingItemCell.CellId);
            }

            return null;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return OrderItemCell.EstimatedHeight;
        }
    }
}
