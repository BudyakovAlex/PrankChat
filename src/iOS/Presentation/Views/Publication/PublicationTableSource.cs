using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using MvvmCross.Binding.Extensions;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.iOS.Presentation.SourcesAndDelegates;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Publication
{
    public class PublicationTableSource : PagedTableViewSource
    {
        private const int DelayMiliseconds = 200;

        private PublicationItemCell _previousVideoCell;

        public PublicationTableSource(UITableView tableView) : base(tableView)
        {
            UseAnimations = true;
        }

        private MvxInteraction _itemsChangedInteraction;
        public MvxInteraction ItemsChangedInteraction
        {
            get => _itemsChangedInteraction;
            set
            {
                if (_itemsChangedInteraction != null)
                {
                    _itemsChangedInteraction.Requested -= OnDataSetChanged;
                }

                _itemsChangedInteraction = value;
                _itemsChangedInteraction.Requested += OnDataSetChanged;
            }
        }

        private void OnDataSetChanged(object sender, EventArgs e)
        {
            PlayVideoAfterReloadDataAsync().FireAndForget();
        }

        private async Task PlayVideoAfterReloadDataAsync()
        {
            await Task.Delay(DelayMiliseconds);
            await PrepareCellsForPlayingVideoAsync();
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

        protected override void Dispose(bool disposing)
        {
            if (disposing && _itemsChangedInteraction != null)
            {
                _itemsChangedInteraction.Requested -= OnDataSetChanged;
            }

            base.Dispose(disposing);
        }
    }
}