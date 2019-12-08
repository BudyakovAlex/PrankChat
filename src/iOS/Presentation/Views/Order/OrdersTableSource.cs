﻿using System;
using Foundation;
using MvvmCross.Platforms.Ios.Binding.Views;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Order
{
    public class OrdersTableSource : MvxTableViewSource
    {
        public OrdersTableSource(UITableView tableView) : base(tableView)
        {
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            if (item is OrderItemViewModel)
            {
                return tableView.DequeueReusableCell(OrderItemCell.CellId);
            }

            return null;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return OrderItemCell.EstimatedHeight;
        }
    }
}
