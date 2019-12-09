﻿using System;
using Foundation;
using MvvmCross.Platforms.Ios.Binding.Views;
using PrankChat.Mobile.Core.Presentation.ViewModels.Rating.Items;
using PrankChat.Mobile.iOS.Presentation.Views.RatingView;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Order
{
    public class RatingTableSource : MvxTableViewSource
    {
        public RatingTableSource(UITableView tableView) : base(tableView)
        {
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            if (item is RatingItemViewModel)
            {
                return tableView.DequeueReusableCell(RatingItemCell.CellId);
            }

            return null;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return RatingItemCell.EstimatedHeight;
        }
    }
}