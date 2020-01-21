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

        public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
        {
            if (_initialized)
                return;

            var cellToPlay = (PublicationItemCell)cell;
            _previousCellToPlay = cellToPlay;
            cellToPlay.PlayVideo();
            _initialized = true;
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
            var visibleCells = indexPaths.Select(indexPath => TableView.CellAt(indexPath) as PublicationItemCell).ToList();
            var centralCellToPlay = visibleCells[indexPaths.Length / 2];
            var partiallyVisibleCells = new List<PublicationItemCell>();
            foreach (var visibleCell in visibleCells)
            {
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
