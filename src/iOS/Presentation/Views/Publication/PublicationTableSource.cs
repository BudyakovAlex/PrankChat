using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using MvvmCross.Binding.Extensions;
using MvvmCross.Platforms.Ios.Binding.Views;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Publication
{
    public class PublicationTableSource : MvxTableViewSource
    {
        private const int FirstInitDelayInMiliseconds = 200;

        private PublicationItemCell _previousVideoCell;

        public PublicationTableSource(UITableView tableView) : base(tableView)
        {
            UseAnimations = true;
        }

        protected override void CollectionChangedOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            base.CollectionChangedOnCollectionChanged(sender, args);
            StartVideoIfNeedAsync(ItemsSource.Count()).FireAndForget();
        }

        private async Task StartVideoIfNeedAsync(int itemsCount)
        {
            await Task.Delay(FirstInitDelayInMiliseconds);

            if (itemsCount != ItemsSource.Count())
            {
                return;
            }

            PrepareCellsForPlayingVideoAsync().FireAndForget();
        }

        public override void DecelerationEnded(UIScrollView scrollView)
        {
            StopVideo(_previousVideoCell);
            PrepareCellsForPlayingVideoAsync().FireAndForget();
        }

        public override void DraggingEnded(UIScrollView scrollView, bool willDecelerate)
        {
            if (willDecelerate)
            {
                return;
            }

            PrepareCellsForPlayingVideoAsync().FireAndForget();
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return UITableView.AutomaticDimension;
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            return tableView.DequeueReusableCell(PublicationItemCell.CellId);
        }

        private Task PrepareCellsForPlayingVideoAsync()
        {
            var publicalitionCells = TableView.VisibleCells.OfType<PublicationItemCell>().ToList();
            if (publicalitionCells.Count == 0)
            {
                return Task.CompletedTask;
            }

            var cellToPlay = publicalitionCells.FirstOrDefault(cell => IsCompletelyVisible(cell));
            var lastCompletelyVisibleCell = publicalitionCells.LastOrDefault(cell => IsCompletelyVisible(cell));

            var indexPath = TableView.IndexPathForCell(lastCompletelyVisibleCell);
            if (indexPath.Row == ItemsSource.Count() - 1)
            {
                cellToPlay = lastCompletelyVisibleCell;
            }

            PlayVideo(cellToPlay);
            return Task.CompletedTask;
        }

        private void PlayVideo(PublicationItemCell cell)
        {
            StopVideo(_previousVideoCell);

            var viewModel = cell?.ViewModel;
            if (viewModel is null)
            {
                return;
            }

            var service = viewModel.VideoPlayerService;
            if (service.Player.IsPlaying)
            {
                return;
            }

            service.Play(viewModel.VideoUrl, viewModel.VideoId);
            service.Player.SetPlatformVideoPlayerContainer(cell.AVPlayerViewControllerInstance);
            _previousVideoCell = cell;
        }

        private void StopVideo(PublicationItemCell cell)
        {
            var viewModel = cell?.ViewModel;
            if (viewModel is null ||
                !viewModel.VideoPlayerService.Player.IsPlaying)
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