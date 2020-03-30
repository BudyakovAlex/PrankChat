// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PrankChat.Mobile.iOS.Presentation.Views.Order
{
	[Register ("OrdersView")]
	partial class OrdersView
	{
		[Outlet]
		UIKit.UIImageView filterArrowImageView { get; set; }

		[Outlet]
		UIKit.UIView filterContainerView { get; set; }

		[Outlet]
		UIKit.UILabel filterTitleLabel { get; set; }

		[Outlet]
		UIKit.UIView orderTabIndicator { get; set; }

		[Outlet]
		UIKit.UILabel orderTabLabel { get; set; }

		[Outlet]
		UIKit.UIView ratingTabIndicator { get; set; }

		[Outlet]
		UIKit.UILabel ratingTabLabel { get; set; }

		[Outlet]
		UIKit.UITableView tableView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (filterArrowImageView != null) {
				filterArrowImageView.Dispose ();
				filterArrowImageView = null;
			}

			if (filterContainerView != null) {
				filterContainerView.Dispose ();
				filterContainerView = null;
			}

			if (filterTitleLabel != null) {
				filterTitleLabel.Dispose ();
				filterTitleLabel = null;
			}

			if (orderTabIndicator != null) {
				orderTabIndicator.Dispose ();
				orderTabIndicator = null;
			}

			if (orderTabLabel != null) {
				orderTabLabel.Dispose ();
				orderTabLabel = null;
			}

			if (ratingTabIndicator != null) {
				ratingTabIndicator.Dispose ();
				ratingTabIndicator = null;
			}

			if (ratingTabLabel != null) {
				ratingTabLabel.Dispose ();
				ratingTabLabel = null;
			}

			if (tableView != null) {
				tableView.Dispose ();
				tableView = null;
			}
		}
	}
}
