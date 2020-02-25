using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using MvvmCross.Platforms.Ios.Binding.Views;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using UIKit;
using PrankChat.Mobile.Core.Infrastructure.Extensions;

namespace PrankChat.Mobile.iOS.Presentation.Views.Publication
{
    public class PublicationTableSource : MvxTableViewSource
    {
        private const int InitializeDelayInMilliseconds = 300;
        private const int ReinitializeDelayInMilliseconds = 500;
        private readonly IVideoListViewModel _parentViewModel;
        private PublicationItemCell _previousCellToPlay;
        private bool _initialized;

        public PublicationTableSource(UITableView tableView, IVideoListViewModel parentViewModel) : base(tableView)
        {
            UseAnimations = true;
            _parentViewModel = parentViewModel;
        }

        private int _segment;
        public int Segment
        {
            get => _segment;
            set
            {
                _segment = value;
                Reinitialize();
            }
        }

        private string _filterName;
        public string FilterName
        {
            get => _filterName;
            set
            {
                _filterName = value;
                Reinitialize();
            }
        }

        public async Task Initialize()
        {
            if (_initialized)
                return;

            var indexPath = TableView.IndexPathsForVisibleRows?.FirstOrDefault();
            if (indexPath == null)
                return;

            var cellToPlay = TableView.CellAt(indexPath) as PublicationItemCell;
            if (cellToPlay != null)
            {
                // Duration of cell reinitialization load (e.g. tab switch) animation.
                await Task.Delay(ReinitializeDelayInMilliseconds);
                PlayFirstVideo(indexPath);
                _initialized = true;
            }
            else
            {
                while(cellToPlay == null)
                {
                    // Duration of cell init load animation.
                    await Task.Delay(InitializeDelayInMilliseconds);
                    cellToPlay = TableView.CellAt(indexPath) as PublicationItemCell;
                    if (cellToPlay != null)
                    {
                        PlayFirstVideo(indexPath);
                        _initialized = true;
                    }
                }
            }
        }

        public override void CellDisplayingEnded(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
        {
            base.CellDisplayingEnded(tableView, cell, indexPath);

            Initialize().FireAndForget();
        }

        public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
        {
            Initialize().FireAndForget();
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return UITableView.AutomaticDimension;
        }

        public override void DecelerationEnded(UIScrollView scrollView)
        {
            PlayFirstCompletelyVisibleVideoItem();
        }

        public override void DraggingEnded(UIScrollView scrollView, bool willDecelerate)
        {
            PlayFirstCompletelyVisibleVideoItem();
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            return tableView.DequeueReusableCell(PublicationItemCell.CellId);
        }

        private void Reinitialize()
        {
            _initialized = false;
            Initialize().FireAndForget();
        }

        private void PlayFirstVideo(NSIndexPath indexPath)
        {
            var cellToPlay = TableView.CellAt(indexPath) as PublicationItemCell;
            var viewModel = _parentViewModel.Items.ElementAtOrDefault(indexPath.Row);
            cellToPlay?.PlayVideo(viewModel?.VideoUrl);
        }

        private void PlayFirstCompletelyVisibleVideoItem()
        {
            var indexPaths = TableView.IndexPathsForVisibleRows;
            if (indexPaths.Length == 0)
                return;

            PublicationItemCell cellToPlay = null;
            var visibleCellsCollection = indexPaths.Select(indexPath => TableView.CellAt(indexPath) as PublicationItemCell);
            var visibleCells = visibleCellsCollection.ToList();
            var centralCellToPlay = visibleCells[indexPaths.Length / 2];
            var completelyVisibleCells = new List<PublicationItemCell>();
            var partiallyVisibleCells = new List<PublicationItemCell>();
            foreach (var visibleCell in visibleCells)
            {
                if (visibleCell == null)
                    continue;

                if (IsCompletelyVisible(visibleCell))
                {
                    completelyVisibleCells.Add(visibleCell);
                }
                else
                {
                    partiallyVisibleCells.Add(visibleCell);
                }
            }

            cellToPlay = completelyVisibleCells.FirstOrDefault();

            if (cellToPlay == null)
                if (centralCellToPlay != null)
                    cellToPlay = centralCellToPlay;
                else
                    return;

            Debug.WriteLine("Play activated:" + TableView.IndexPathForCell(cellToPlay).Row);

            if (cellToPlay == _previousCellToPlay)
                return;

            _previousCellToPlay = cellToPlay;

            foreach (var partiallyVisibleCell in partiallyVisibleCells)
            {
                partiallyVisibleCell.StopVideo();
            }

            if (completelyVisibleCells.Count > 0
                && TableView.IndexPathForCell(completelyVisibleCells.LastOrDefault()).Row == _parentViewModel.Items.Count - 1)
            {
                completelyVisibleCells.ForEach(c => c.PlayVideo(_parentViewModel.Items.ToList()[TableView.IndexPathForCell(c).Row].VideoUrl));
            }
            else
            {
                var viewModel = _parentViewModel.Items.ToList()[TableView.IndexPathForCell(cellToPlay).Row];
                cellToPlay.PlayVideo(viewModel.VideoUrl);
            }
        }

        private bool IsCompletelyVisible(PublicationItemCell publicationCell)
        {
            var videoRect = publicationCell.GetVideoBounds(TableView);
            Debug.WriteLine($"Video rect Y: {videoRect.Y}, Table view Y: {TableView.Bounds.Y}");
            return TableView.Bounds.Contains(videoRect);
        }
    }
}
