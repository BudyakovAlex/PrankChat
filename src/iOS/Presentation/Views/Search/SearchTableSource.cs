using System;
using Foundation;
using MvvmCross.Platforms.Ios.Binding.Views;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items;
using PrankChat.Mobile.Core.Presentation.ViewModels.Search.Items;
using PrankChat.Mobile.iOS.Infrastructure;
using PrankChat.Mobile.iOS.Presentation.Views.Order;
using PrankChat.Mobile.iOS.Presentation.Views.Publication;
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
            switch (item)
            {
                case ProfileSearchItemViewModel _:
                    return tableView.DequeueReusableCell(ProfileSearchItemCell.CellId);
                case OrderItemViewModel _:
                    return tableView.DequeueReusableCell(OrderItemCell.CellId);
                case PublicationItemViewModel _:
                    return tableView.DequeueReusableCell(PublicationItemCell.CellId);
            }

            return null;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return UITableView.AutomaticDimension;
        }
    }
}
