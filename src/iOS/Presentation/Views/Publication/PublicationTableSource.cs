using System;
using System.Linq;
using CoreGraphics;
using Foundation;
using MvvmCross.Platforms.Ios.Binding.Views;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Publication
{
    public class PublicationTableSource : MvxTableViewSource
    {
        public PublicationTableSource(UITableView tableView) : base(tableView)
        {
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            if (item is PublicationItemViewModel viewModel)
            {
                var cell = (PublicationItemCell)tableView.DequeueReusableCell(PublicationItemCell.CellId);
                cell.PrerollVideo(viewModel.VideoUrl);
                return cell;
            }

            return null;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return PublicationItemCell.EstimatedHeight;
        }

        public override void Scrolled(UIScrollView scrollView)
        {
            var firstCellIndexPath = TableView.IndexPathsForVisibleRows?.FirstOrDefault();
            if (firstCellIndexPath == null)
                return;

            try
            {
                foreach (var item in TableView.IndexPathsForVisibleRows)
                {
                    var cell = TableView.CellAt(item);
                    var publicationCell = cell as PublicationItemCell;
                    if (publicationCell == null)
                        continue;

                    if (GetIsFirstCompletelyVisibleCell(item, publicationCell.VideoBounds))
                    {
                        publicationCell.PlayVideo();
                    }
                    else
                    {
                        publicationCell.StopVideo();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private bool GetIsFirstCompletelyVisibleCell(NSIndexPath indexPath, CGRect videoBounds)
        {
            var cellRect = TableView.RectForRowAtIndexPath(indexPath);
            var isContains = TableView.Bounds.Contains(videoBounds);
            return isContains;
        }
    }
}
