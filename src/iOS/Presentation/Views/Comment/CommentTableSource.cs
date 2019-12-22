using System;
using Foundation;
using MvvmCross.Platforms.Ios.Binding.Views;
using PrankChat.Mobile.Core.Presentation.ViewModels.Comment.Items;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Comment
{
    public class CommentTableSource : MvxTableViewSource
    {
        public CommentTableSource(UITableView tableView) : base(tableView)
        {
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            if (item is CommentItemViewModel)
            {
                return tableView.DequeueReusableCell(CommentItemCell.CellId);
            }

            return null;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return CommentItemCell.EstimatedHeight;
        }
    }
}
