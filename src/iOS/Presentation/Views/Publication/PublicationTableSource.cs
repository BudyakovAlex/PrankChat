using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Foundation;
using MvvmCross.Platforms.Ios.Binding.Views;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Publication
{
    public class PublicationTableSource : MvxTableViewSource
    {
        private PublicationItemCell _previousCellToPlay;
        private bool _initialized;
        private readonly IVideoListViewModel _parentViewModel;

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
                _initialized = false;
            }
        }

        private string _filterName;
        public string FilterName
        {
            get => _filterName;
            set
            {
                _filterName = value;
                _initialized = false;
            }
        }

        public void Initialize()
        {
            if (_initialized)
                return;

            PlayFirstCompletelyVisibleVideoItem();

            _initialized = true;
        }

        public override void CellDisplayingEnded(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
        {
            base.CellDisplayingEnded(tableView, cell, indexPath);

            Initialize();
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
