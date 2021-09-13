using System;
using Foundation;
using MvvmCross.Platforms.Ios.Binding.Views;
using PrankChat.Mobile.Core.ViewModels.Comment.Items;
using PrankChat.Mobile.iOS.Views.Comment;
using UIKit;

namespace PrankChat.Mobile.iOS.SourcesAndDelegates
{
    public class CommentsTableSource : MvxTableViewSource
    {
        public CommentsTableSource(UITableView tableView) : base(tableView)
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
            return UITableView.AutomaticDimension;
        }
    }
}
