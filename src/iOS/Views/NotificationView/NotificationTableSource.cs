using Foundation;
using PrankChat.Mobile.Core.ViewModels.Notification.Items;
using PrankChat.Mobile.iOS.SourcesAndDelegates;
using PrankChat.Mobile.iOS.Views.NotificationView;
using UIKit;

namespace PrankChat.Mobile.iOS.Views.Order
{
    public class NotificationTableSource : PagedTableViewSource
    {
        public NotificationTableSource(UITableView tableView) : base(tableView)
        {
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            if (item is NotificationItemViewModel)
            {
                return tableView.DequeueReusableCell(NotificationItemCell.CellId);
            }

            return null;
        }
    }
}
