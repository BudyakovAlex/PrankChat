﻿using Foundation;
using MvvmCross.Platforms.Ios.Binding.Views;
using PrankChat.Mobile.Core.ViewModels.Order.Items;
using PrankChat.Mobile.Core.ViewModels.Publication.Items;
using PrankChat.Mobile.Core.ViewModels.Search.Items;
using PrankChat.Mobile.iOS.Views.Order;
using PrankChat.Mobile.iOS.Views.Publication;
using System;
using UIKit;

namespace PrankChat.Mobile.iOS.Views.Search
{
    public class SearchTableSource : MvxTableViewSource
    {
        public SearchTableSource(UITableView tableView) : base(tableView)
        {
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            switch (item)
            {
                case ProfileSearchItemViewModel _:
                    return tableView.DequeueReusableCell(ProfileSearchItemCell.CellId);
                case OrderItemViewModel _:
                    return tableView.DequeueReusableCell(OrderItemCell.CellId);
                default:
                    return new MvxStandardTableViewCell(string.Empty, UITableViewCellStyle.Default, new NSString("MvxStandardTableViewCell"));
            }
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return UITableView.AutomaticDimension;
        }
    }
}
