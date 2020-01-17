using System;
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
            PlayAllVisibleVideos();
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            if (item is PublicationItemViewModel viewModel)
            {
                var cell = (PublicationItemCell)tableView.DequeueReusableCell(PublicationItemCell.CellId);
                cell.PrerollVideo(viewModel.VideoUrl);
                if (TableView.IndexPathsForVisibleRows.Contains(indexPath))
                    cell.PlayVideo();

                return cell;
            }

            return null;
        }

        private void PlayAllVisibleVideos()
        {
            foreach (var indexPath in TableView.IndexPathsForVisibleRows)
            {
                var cell = TableView.CellAt(indexPath);
                var publicationCell = cell as PublicationItemCell;
                if (publicationCell == null)
                    continue;

                publicationCell.PlayVideo();
            }
        }
    }
}
