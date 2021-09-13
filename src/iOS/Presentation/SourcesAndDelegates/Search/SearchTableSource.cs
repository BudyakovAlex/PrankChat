using Foundation;
using MvvmCross.Platforms.Ios.Binding.Views;
using PrankChat.Mobile.Core.ViewModels.Order.Items;
using PrankChat.Mobile.Core.ViewModels.Publication.Items;
using PrankChat.Mobile.Core.ViewModels.Search.Items;
using PrankChat.Mobile.iOS.Presentation.Views.Order;
using PrankChat.Mobile.iOS.Presentation.Views.Publication;
using System;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Search
{
    public class SearchTableSource : MvxTableViewSource
    {
        public SearchTableSource(UITableView tableView) : base(tableView)
        {
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item) => item switch
        {
            ProfileSearchItemViewModel _ => tableView.DequeueReusableCell(ProfileSearchItemCell.CellId),
            OrderItemViewModel _ => tableView.DequeueReusableCell(OrderItemCell.CellId),
            _ => new MvxStandardTableViewCell(string.Empty, UITableViewCellStyle.Default, new NSString("MvxStandardTableViewCell")),
        };

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath) =>
            UITableView.AutomaticDimension;
    }
}
