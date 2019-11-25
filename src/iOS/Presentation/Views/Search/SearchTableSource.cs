﻿using System;
using Foundation;
using MvvmCross.Platforms.Ios.Binding.Views;
using PrankChat.Mobile.Core.Presentation.ViewModels.Search;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Search
{
    public class SearchTableSource : MvxTableViewSource
    {
        public SearchTableSource(UITableView tableView) : base(tableView)
        {
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            if (item is ProfileSearchItemViewModel)
            {
                return tableView.DequeueReusableCell(ProfileSearchItemCell.CellId);
            }

            return null;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return ProfileSearchItemCell.EstimatedHeight;
        }
    }
}
