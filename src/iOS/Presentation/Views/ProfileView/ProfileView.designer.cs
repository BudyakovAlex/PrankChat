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
		UIKit.UILabel descriptionLabel { get; set; }

		[Outlet]
		UIKit.UILabel nameLabel { get; set; }

		[Outlet]
		UIKit.UILabel priceLabel { get; set; }

		[Outlet]
		PrankChat.Mobile.iOS.Controls.CircleCachedImageView profileImageView { get; set; }

		[Outlet]
		UIKit.UIButton refillButton { get; set; }

		[Outlet]
		UIKit.UIScrollView rootScrollView { get; set; }

		[Outlet]
		UIKit.UILabel subscribersTitleLabel { get; set; }

		[Outlet]
		UIKit.UILabel subscribersValueLabel { get; set; }

		[Outlet]
		UIKit.UIView subscribersView { get; set; }

		[Outlet]
		UIKit.UILabel subscriptionsTitleLabel { get; set; }

		[Outlet]
		UIKit.UILabel subscriptionsValueLabel { get; set; }

		[Outlet]
		UIKit.UIView subscriptionsView { get; set; }

		[Outlet]
		UIKit.UITableView tableView { get; set; }

		[Outlet]
		PrankChat.Mobile.iOS.Controls.TabView tabView { get; set; }

		[Outlet]
		UIKit.UIButton withdrawalButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (descriptionLabel != null) {
				descriptionLabel.Dispose ();
				descriptionLabel = null;
			}

			if (nameLabel != null) {
				nameLabel.Dispose ();
				nameLabel = null;
			}

			if (priceLabel != null) {
				priceLabel.Dispose ();
				priceLabel = null;
			}

			if (profileImageView != null) {
				profileImageView.Dispose ();
				profileImageView = null;
			}

			if (refillButton != null) {
				refillButton.Dispose ();
				refillButton = null;
			}

			if (rootScrollView != null) {
				rootScrollView.Dispose ();
				rootScrollView = null;
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

			if (tabView != null) {
				tabView.Dispose ();
				tabView = null;
			}

			if (withdrawalButton != null) {
				withdrawalButton.Dispose ();
				withdrawalButton = null;
			}

			if (subscribersView != null) {
				subscribersView.Dispose ();
				subscribersView = null;
			}

			if (subscriptionsView != null) {
				subscriptionsView.Dispose ();
				subscriptionsView = null;
			}
		}
	}
}
