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
		UIKit.UILabel completedOrdersTitleLabel { get; set; }

		[Outlet]
		UIKit.UILabel completedOrdersValueLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		FFImageLoading.Cross.MvxCachedImageView imageChangeProfile { get; set; }

		[Outlet]
		UIKit.UILabel ordersTitleLabel { get; set; }

		[Outlet]
		UIKit.UILabel ordersValueLabel { get; set; }

		[Outlet]
		UIKit.UILabel priceLabel { get; set; }

		[Outlet]
		UIKit.UILabel profileDescriptionLabel { get; set; }

		[Outlet]
		FFImageLoading.Cross.MvxCachedImageView profileImageView { get; set; }

		[Outlet]
		UIKit.UILabel profileShortNameLabel { get; set; }

		[Outlet]
		UIKit.UIButton refillButton { get; set; }

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
			if (profileShortNameLabel != null) {
				profileShortNameLabel.Dispose ();
				profileShortNameLabel = null;
			}

			if (completedOrdersTitleLabel != null) {
				completedOrdersTitleLabel.Dispose ();
				completedOrdersTitleLabel = null;
			}

			if (completedOrdersValueLabel != null) {
				completedOrdersValueLabel.Dispose ();
				completedOrdersValueLabel = null;
			}

			if (ordersTitleLabel != null) {
				ordersTitleLabel.Dispose ();
				ordersTitleLabel = null;
			}

			if (ordersValueLabel != null) {
				ordersValueLabel.Dispose ();
				ordersValueLabel = null;
			}

			if (priceLabel != null) {
				priceLabel.Dispose ();
				priceLabel = null;
			}

			if (profileDescriptionLabel != null) {
				profileDescriptionLabel.Dispose ();
				profileDescriptionLabel = null;
			}

			if (profileImageView != null) {
				profileImageView.Dispose ();
				profileImageView = null;
			}

			if (refillButton != null) {
				refillButton.Dispose ();
				refillButton = null;
			}

			if (segmentedControl != null) {
				segmentedControl.Dispose ();
				segmentedControl = null;
			}

			if (subscribersTitleLabel != null) {
				subscribersTitleLabel.Dispose ();
				subscribersTitleLabel = null;
			}

			if (subscribersValueLabel != null) {
				subscribersValueLabel.Dispose ();
				subscribersValueLabel = null;
			}

			if (subscriptionsTitleLabel != null) {
				subscriptionsTitleLabel.Dispose ();
				subscriptionsTitleLabel = null;
			}

			if (subscriptionsValueLabel != null) {
				subscriptionsValueLabel.Dispose ();
				subscriptionsValueLabel = null;
			}

			if (tableView != null) {
				tableView.Dispose ();
				tableView = null;
			}

			if (withdrawalButton != null) {
				withdrawalButton.Dispose ();
				withdrawalButton = null;
			}

			if (imageChangeProfile != null) {
				imageChangeProfile.Dispose ();
				imageChangeProfile = null;
			}
		}
	}
}
