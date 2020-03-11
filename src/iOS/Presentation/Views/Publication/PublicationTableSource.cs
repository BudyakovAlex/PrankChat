using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using MvvmCross.Platforms.Ios.Binding.Views;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Publication
{
    public class PublicationTableSource : MvxTableViewSource
    {
        private const int InitializeDelayInMilliseconds = 200;

        private PublicationItemCell _previousVideoCell;

        public PublicationTableSource(UITableView tableView) : base(tableView)
        {
            UseAnimations = true;
        }

        public override void ScrollAnimationEnded(UIScrollView scrollView)
        {
            StopVideo(_previousVideoCell);
            PrepareCellsFowPlayingVideoAsync().FireAndForget();
        }

        public override void DecelerationEnded(UIScrollView scrollView)
        {
            StopVideo(_previousVideoCell);
            PrepareCellsFowPlayingVideoAsync().FireAndForget();
        }

        public override void DraggingEnded(UIScrollView scrollView, bool willDecelerate)
        {
            StopVideo(_previousVideoCell);
            PrepareCellsFowPlayingVideoAsync().FireAndForget();
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return UITableView.AutomaticDimension;
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            return tableView.DequeueReusableCell(PublicationItemCell.CellId);
        }

        private async Task PrepareCellsFowPlayingVideoAsync()
        {
            var firstCompletelyVisibleCell = TableView.VisibleCells.OfType<PublicationItemCell>()
                                                                   .ToList()
                                                                   .FirstOrDefault(cell => IsCompletelyVisible(cell));
            await Task.Delay(InitializeDelayInMilliseconds);

            var newVisibleCell = TableView.VisibleCells.OfType<PublicationItemCell>()
                                                       .ToList()
                                                       .FirstOrDefault(cell => IsCompletelyVisible(cell));
            if (firstCompletelyVisibleCell == newVisibleCell)
            {
                PlayVideo(newVisibleCell);
            }
        }

        private void PlayVideo(PublicationItemCell cell)
        {
            var viewModel = cell?.ViewModel;
            if (viewModel is null)
            {
                return;
            }

            _previousVideoCell = cell;
            var service = viewModel.VideoPlayerService;
            if (service.Player.IsPlaying)
            {
                return;
            }

            service.Stop();
            service.Player.SetPlatformVideoPlayerContainer(null);
            service.Player.SetPlatformVideoPlayerContainer(cell.AVPlayerViewControllerInstance);
            service.Play(viewModel.VideoUrl, viewModel.VideoId);
        }

        private void StopVideo(PublicationItemCell cell)
        {
            var viewModel = cell?.ViewModel;
            if (viewModel is null)
            {
                return;
            }

            if (!viewModel.VideoPlayerService.Player.IsPlaying)
            {
                return;
            }

            viewModel.VideoPlayerService.Stop();
            cell.AVPlayerViewControllerInstance.Player = null;
        }

        private bool IsCompletelyVisible(PublicationItemCell publicationCell)
        {
            var videoRect = publicationCell.GetVideoBounds(TableView);
            Debug.WriteLine($"Video rect Y: {videoRect.Y}, Table view Y: {TableView.Bounds.Y}");
            return TableView.Bounds.Contains(videoRect);
        }
    }
}