// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PrankChat.Mobile.iOS.Presentation.Views.Competition
{
	[Register ("CompetitionVideoCell")]
	partial class CompetitionVideoCell
	{
		[Outlet]
		UIKit.UIButton likeButton { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint likeButtonBottomConstraint { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint likeButtonHeightConstraint { get; set; }

		[Outlet]
		UIKit.UIActivityIndicatorView loadingActivityIndicator { get; set; }

		[Outlet]
		UIKit.UILabel postDateLabel { get; set; }

		[Outlet]
		PrankChat.Mobile.iOS.Controls.CircleCachedImageView profileImageView { get; set; }

		[Outlet]
		UIKit.UILabel profileNameLabel { get; set; }

		[Outlet]
		FFImageLoading.Cross.MvxCachedImageView stubImageView { get; set; }

		[Outlet]
		UIKit.UIView videoView { get; set; }

		[Outlet]
		UIKit.UILabel viewsLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (likeButton != null) {
				likeButton.Dispose ();
				likeButton = null;
			}

			if (loadingActivityIndicator != null) {
				loadingActivityIndicator.Dispose ();
				loadingActivityIndicator = null;
			}

			if (postDateLabel != null) {
				postDateLabel.Dispose ();
				postDateLabel = null;
			}

			if (profileImageView != null) {
				profileImageView.Dispose ();
				profileImageView = null;
			}

			if (profileNameLabel != null) {
				profileNameLabel.Dispose ();
				profileNameLabel = null;
			}

			if (stubImageView != null) {
				stubImageView.Dispose ();
				stubImageView = null;
			}

			if (videoView != null) {
				videoView.Dispose ();
				videoView = null;
			}

			if (viewsLabel != null) {
				viewsLabel.Dispose ();
				viewsLabel = null;
			}

			if (likeButtonHeightConstraint != null) {
				likeButtonHeightConstraint.Dispose ();
				likeButtonHeightConstraint = null;
			}

			if (likeButtonBottomConstraint != null) {
				likeButtonBottomConstraint.Dispose ();
				likeButtonBottomConstraint = null;
			}
		}
	}
}
