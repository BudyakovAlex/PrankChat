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
	[Register ("UserProfileView")]
	partial class UserProfileView
	{
		[Outlet]
		UIKit.UILabel DescriptionLabel { get; set; }

		[Outlet]
		UIKit.UILabel NameLabel { get; set; }

		[Outlet]
		PrankChat.Mobile.iOS.Controls.CircleCachedImageView ProfileImageView { get; set; }

		[Outlet]
		UIKit.UIScrollView RootScrollView { get; set; }

		[Outlet]
		UIKit.UIButton SubscribeButton { get; set; }

		[Outlet]
		UIKit.UILabel SubscribersTitleLabel { get; set; }

		[Outlet]
		UIKit.UILabel SubscribersValueLabel { get; set; }

		[Outlet]
		UIKit.UIView SubscribersView { get; set; }

		[Outlet]
		UIKit.UILabel SubscriptionsTitleLabel { get; set; }

		[Outlet]
		UIKit.UILabel SubscriptionsValueLabel { get; set; }

		[Outlet]
		UIKit.UIView SubscriptionsView { get; set; }

		[Outlet]
		UIKit.UITableView TableView { get; set; }

		[Outlet]
		PrankChat.Mobile.iOS.Controls.TabView TabView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (DescriptionLabel != null) {
				DescriptionLabel.Dispose ();
				DescriptionLabel = null;
			}

			if (NameLabel != null) {
				NameLabel.Dispose ();
				NameLabel = null;
			}

			if (ProfileImageView != null) {
				ProfileImageView.Dispose ();
				ProfileImageView = null;
			}

			if (RootScrollView != null) {
				RootScrollView.Dispose ();
				RootScrollView = null;
			}

			if (SubscribeButton != null) {
				SubscribeButton.Dispose ();
				SubscribeButton = null;
			}

			if (SubscribersTitleLabel != null) {
				SubscribersTitleLabel.Dispose ();
				SubscribersTitleLabel = null;
			}

			if (SubscribersValueLabel != null) {
				SubscribersValueLabel.Dispose ();
				SubscribersValueLabel = null;
			}

			if (SubscriptionsTitleLabel != null) {
				SubscriptionsTitleLabel.Dispose ();
				SubscriptionsTitleLabel = null;
			}

			if (SubscriptionsValueLabel != null) {
				SubscriptionsValueLabel.Dispose ();
				SubscriptionsValueLabel = null;
			}

			if (TableView != null) {
				TableView.Dispose ();
				TableView = null;
			}

			if (TabView != null) {
				TabView.Dispose ();
				TabView = null;
			}

			if (SubscribersView != null) {
				SubscribersView.Dispose ();
				SubscribersView = null;
			}

			if (SubscriptionsView != null) {
				SubscriptionsView.Dispose ();
				SubscriptionsView = null;
			}
		}
	}
}
