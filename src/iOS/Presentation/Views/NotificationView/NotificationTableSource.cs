using System;
using Foundation;
using MvvmCross.Platforms.Ios.Binding.Views;
using PrankChat.Mobile.Core.Presentation.ViewModels.Notification.Items;
using PrankChat.Mobile.iOS.Presentation.Views.NotificationView;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Order
{
    public class NotificationTableSource : MvxTableViewSource
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

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return NotificationItemCell.EstimatedHeight;
        }
    }
}
