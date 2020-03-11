// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PrankChat.Mobile.iOS.Presentation.Views.ProfileView
{
	[Register ("ProfileView")]
	partial class ProfileView
	{
		[Outlet]
		FFImageLoading.Cross.MvxCachedImageView changeProfileImageView { get; set; }

		[Outlet]
		UIKit.UILabel completedOrdersTitleLabel { get; set; }

		[Outlet]
		UIKit.UILabel completedOrdersValueLabel { get; set; }

		[Outlet]
		UIKit.UILabel ordersTitleLabel { get; set; }

		[Outlet]
		UIKit.UILabel ordersValueLabel { get; set; }

		[Outlet]
		UIKit.UILabel priceLabel { get; set; }

		[Outlet]
		UIKit.UILabel profileDescriptionLabel { get; set; }

		[Outlet]
		PrankChat.Mobile.iOS.Controls.CircleCachedImageView profileImageView { get; set; }

		[Outlet]
		UIKit.UIButton refillButton { get; set; }

		[Outlet]
		UIKit.UIScrollView rootScrollView { get; set; }

		[Outlet]
		UIKit.UISegmentedControl segmentedControl { get; set; }

		[Outlet]
		UIKit.UILabel subscribersTitleLabel { get; set; }

		[Outlet]
		UIKit.UILabel subscribersValueLabel { get; set; }

		[Outlet]
		UIKit.UILabel subscriptionsTitleLabel { get; set; }

		[Outlet]
		UIKit.UILabel subscriptionsValueLabel { get; set; }

		[Outlet]
		UIKit.UITableView tableView { get; set; }

		[Outlet]
		UIKit.UIButton withdrawalButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (rootScrollView != null) {
				rootScrollView.Dispose ();
				rootScrollView = null;
			}

			if (tableView != null) {
				tableView.Dispose ();
				tableView = null;
			}

			if (profileImageView != null) {
				profileImageView.Dispose ();
				profileImageView = null;
			}

			if (changeProfileImageView != null) {
				changeProfileImageView.Dispose ();
				changeProfileImageView = null;
			}

			if (profileDescriptionLabel != null) {
				profileDescriptionLabel.Dispose ();
				profileDescriptionLabel = null;
			}

			if (priceLabel != null) {
				priceLabel.Dispose ();
				priceLabel = null;
			}

			if (refillButton != null) {
				refillButton.Dispose ();
				refillButton = null;
			}

			if (withdrawalButton != null) {
				withdrawalButton.Dispose ();
				withdrawalButton = null;
			}

			if (ordersValueLabel != null) {
				ordersValueLabel.Dispose ();
				ordersValueLabel = null;
			}

			if (ordersTitleLabel != null) {
				ordersTitleLabel.Dispose ();
				ordersTitleLabel = null;
			}

			if (completedOrdersValueLabel != null) {
				completedOrdersValueLabel.Dispose ();
				completedOrdersValueLabel = null;
			}

			if (completedOrdersTitleLabel != null) {
				completedOrdersTitleLabel.Dispose ();
				completedOrdersTitleLabel = null;
			}

			if (subscribersValueLabel != null) {
				subscribersValueLabel.Dispose ();
				subscribersValueLabel = null;
			}

			if (subscribersTitleLabel != null) {
				subscribersTitleLabel.Dispose ();
				subscribersTitleLabel = null;
			}

			if (subscriptionsValueLabel != null) {
				subscriptionsValueLabel.Dispose ();
				subscriptionsValueLabel = null;
			}

			if (subscriptionsTitleLabel != null) {
				subscriptionsTitleLabel.Dispose ();
				subscriptionsTitleLabel = null;
			}

			if (segmentedControl != null) {
				segmentedControl.Dispose ();
				segmentedControl = null;
			}
		}
	}
}
