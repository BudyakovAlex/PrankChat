using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using MvvmCross.Binding.Extensions;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.SourcesAndDelegates
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

            if (cellToPlay is null)
            {
                return Task.CompletedTask;
            }

            PlayVideo(cellToPlay);
            return Task.CompletedTask;
        }

        private void PlayVideo(BaseVideoTableCell cell)
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

            service.Player.SetPlatformVideoPlayerContainer(cell.AVPlayerViewControllerInstance);
            cell.AddObserverForPeriodicTime();
            service.Play(viewModel.PreviewUrl, viewModel.VideoId);
            viewModel.Logger.LogEventAsync(DateTime.Now, "[Publications_Start_Play]", $"Video id is {viewModel.VideoId}");

            _previousVideoCell = cell;
        }

        private void StopVideo(BaseVideoTableCell cell)
        {
            cell?.ShowStub();

            var viewModel = cell?.ViewModel;
            if (viewModel is null ||
                !viewModel.VideoPlayerService.Player.IsPlaying)
            {
                return;
            }

            viewModel.VideoPlayerService.Stop();
            viewModel.Logger.LogEventAsync(DateTime.Now, "[Publications_Stop_Play]", $"Video id is {viewModel.VideoId}");
            cell.AVPlayerViewControllerInstance.Player = null;
        }

        private bool IsCompletelyVisible(BaseVideoTableCell publicationCell)
        {
            var videoRect = publicationCell.GetVideoBounds(TableView);
            Debug.WriteLine($"Video rect Y: {videoRect.Y}, Table view Y: {TableView.Bounds.Y}");
            return TableView.Bounds.Contains(videoRect);
        }
    }
}
