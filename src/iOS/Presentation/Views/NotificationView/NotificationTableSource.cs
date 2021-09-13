using Foundation;
using PrankChat.Mobile.Core.ViewModels.Notification.Items;
using PrankChat.Mobile.iOS.Presentation.SourcesAndDelegates;
using PrankChat.Mobile.iOS.Presentation.Views.NotificationView;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Order
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
