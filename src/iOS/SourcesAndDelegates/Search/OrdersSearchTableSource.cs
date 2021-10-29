using Foundation;
using PrankChat.Mobile.iOS.Infrastructure;
using PrankChat.Mobile.iOS.Views.Search;
using System;
using UIKit;

namespace PrankChat.Mobile.iOS.SourcesAndDelegates.Search
{
    public class OrdersSearchTableSource : SearchTableSource
    {
        public OrdersSearchTableSource(UITableView tableView) : base(tableView)
        {
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return Constants.CellHeights.OrderItemCellHeight;
        }
    }
}