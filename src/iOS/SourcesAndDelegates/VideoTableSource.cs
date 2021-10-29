using FFImageLoading;
using Foundation;
using MvvmCross.Binding.Extensions;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ViewModels.Common.Abstract;
using PrankChat.Mobile.iOS.Views.Base;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UIKit;
using PrankChat.Mobile.Core.Extensions;

namespace PrankChat.Mobile.iOS.SourcesAndDelegates
{
    public class VideoTableSource : TableViewSource
    {
        private const int DelayMiliseconds = 200;

        private BaseVideoTableCell _previousVideoCell;

        public VideoTableSource(UITableView tableView)
            : base(tableView)
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

        public override void DecelerationEnded(UIScrollView scrollView)
        {
            base.DecelerationEnded(scrollView);

            PrepareCellsForPlayingVideoAsync().FireAndForget();
        }

        public override void DraggingEnded(UIScrollView scrollView, bool willDecelerate)
        {
            base.DraggingEnded(scrollView, willDecelerate);

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
            var cell = base.GetOrCreateCellFor(tableView, indexPath, item);
            if (cell is BaseVideoTableCell videoCell &&
                item is BaseVideoItemViewModel itemViewModel)
            {
                _ = ImageService.Instance.LoadUrl(itemViewModel.StubImageUrl).IntoAsync(videoCell.StubImageView);
            }

            return cell;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _itemsChangedInteraction != null)
            {
                _itemsChangedInteraction.Requested -= OnDataSetChanged;
            }

            base.Dispose(disposing);
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

        private Task PrepareCellsForPlayingVideoAsync()
        {
            var publicalitionCells = TableView.VisibleCells.OfType<BaseVideoTableCell>().ToList();
            if (publicalitionCells.Count == 0)
            {
                return Task.CompletedTask;
            }

            var cellToPlay = publicalitionCells.FirstOrDefault(cell => IsCompletelyVisible(cell));
            var lastCompletelyVisibleCell = publicalitionCells.LastOrDefault(cell => IsCompletelyVisible(cell));

            var index = lastCompletelyVisibleCell is null ? -1 : TableView.IndexPathForCell(lastCompletelyVisibleCell)?.Row;
            if (index == ItemsSource.Count() - 1)
            {
                cellToPlay = lastCompletelyVisibleCell;
            }

            if (cellToPlay?.VideoPlayer is null)
            {
                return Task.CompletedTask;
            }

            PlayVideo(cellToPlay);
            return Task.CompletedTask;
        }

        private void PlayVideo(BaseVideoTableCell cell)
        {
            StopVideo(_previousVideoCell);

            cell.LoadingActivityIndicator.Hidden = false;
            cell.LoadingActivityIndicator.StartAnimating();
            cell.VideoPlayer.Play();
            _previousVideoCell = cell;
        }

        private void StopVideo(BaseVideoTableCell cell)
        {
            if (cell is null)
            {
                return;
            }

            cell.ShowStub();
            cell.VideoPlayer.Stop();
        }

        private bool IsCompletelyVisible(BaseVideoTableCell publicationCell)
        {
            var videoRect = publicationCell.GetVideoBounds(TableView);
            Debug.WriteLine($"Video rect Y: {videoRect.Y}, Table view Y: {TableView.Bounds.Y}");
            return TableView.Bounds.Contains(videoRect);
        }
    }
}