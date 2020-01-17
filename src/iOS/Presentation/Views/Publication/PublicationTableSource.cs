using System;
using System.Diagnostics;
using System.Linq;
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

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return PublicationItemCell.EstimatedHeight;
        }

        public override void DecelerationEnded(UIScrollView scrollView)
        {
            PlayFirstCompletelyVisibleVideo();
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

        private void PlayFirstCompletelyVisibleVideo()
        {
            var indexPaths = TableView.IndexPathsForVisibleRows;
            PublicationItemCell cellToPlay = null;
            var centralCellToPlay = TableView.CellAt(indexPaths[indexPaths.Length / 2]) as PublicationItemCell;
            foreach (var indexPath in TableView.IndexPathsForVisibleRows)
            {
                var cell = TableView.CellAt(indexPath);
                var publicationCell = cell as PublicationItemCell;
                if (publicationCell == null)
                    continue;

                if (IsCompletelyVisible(publicationCell))
                {
                    cellToPlay = publicationCell;
                    break;
                }
            }

            if (cellToPlay == null)
                if (centralCellToPlay != null)
                    cellToPlay = centralCellToPlay;
                else
                    return;

            Debug.WriteLine("Play activated:" + TableView.IndexPathForCell(cellToPlay).Row);
            cellToPlay.PlayVideo();
        }

        private bool IsCompletelyVisible(PublicationItemCell publicationCell)
        {
            var videoRect = publicationCell.GetVideoBounds(TableView);
            Debug.WriteLine($"Video rect Y: {videoRect.Y}, Table view Y: {TableView.Bounds.Y}");
            return TableView.Bounds.Contains(videoRect);
        }
    }
}
