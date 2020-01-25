﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Foundation;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Publication
{
    public class PublicationTableSource : MvxTableViewSource
    {
        private PublicationItemCell _previousCellToPlay;
        private bool _initialized;

        public PublicationTableSource(UITableView tableView) : base(tableView)
        {
            UseAnimations = true;
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
            return PublicationItemCell.EstimatedHeight;
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
            var partiallyVisibleCells = new List<PublicationItemCell>();
            foreach (var visibleCell in visibleCells)
            {
                if (visibleCell == null)
                    continue;

                if (IsCompletelyVisible(visibleCell))
                {
                    cellToPlay = visibleCell;
                }
                else
                {
                    partiallyVisibleCells.Add(visibleCell);
                }
            }

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
