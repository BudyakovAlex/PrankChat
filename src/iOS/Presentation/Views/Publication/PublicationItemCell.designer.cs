// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PrankChat.Mobile.iOS.Presentation.Views.Publication
{
	[Register ("PublicationItemCell")]
	partial class PublicationItemCell
	{
		[Outlet]
		UIKit.UIButton bookmarkButton { get; set; }

		[Outlet]
		UIKit.UIButton likeButton { get; set; }

		[Outlet]
		UIKit.UILabel likeLabel { get; set; }

		[Outlet]
		UIKit.UIActivityIndicatorView loadingActivityIndicator { get; set; }

		[Outlet]
		UIKit.UIButton moreButton { get; set; }

		[Outlet]
		UIKit.UIView placeProcessingOverlay { get; set; }

		[Outlet]
		UIKit.UIView processingBackgroundView { get; set; }

		[Outlet]
		UIKit.UIActivityIndicatorView processingIndicatorView { get; set; }

		[Outlet]
		UIKit.UILabel processingLabel { get; set; }

		[Outlet]
		PrankChat.Mobile.iOS.Controls.CircleCachedImageView profileImage { get; set; }

		[Outlet]
		UIKit.UILabel profileNameLabel { get; set; }

		[Outlet]
		UIKit.UILabel publicationInfoLabel { get; set; }

		[Outlet]
		UIKit.UIButton shareButton { get; set; }

		[Outlet]
		FFImageLoading.Cross.MvxCachedImageView stubImageView { get; set; }

		[Outlet]
		UIKit.UILabel videoNameLabel { get; set; }

		[Outlet]
		UIKit.UIView videoView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (bookmarkButton != null) {
				bookmarkButton.Dispose ();
				bookmarkButton = null;
			}

			if (likeButton != null) {
				likeButton.Dispose ();
				likeButton = null;
			}

			if (processingIndicatorView != null) {
				processingIndicatorView.Dispose ();
				processingIndicatorView = null;
			}

			if (likeLabel != null) {
				likeLabel.Dispose ();
				likeLabel = null;
			}

			if (loadingActivityIndicator != null) {
				loadingActivityIndicator.Dispose ();
				loadingActivityIndicator = null;
			}

			if (moreButton != null) {
				moreButton.Dispose ();
				moreButton = null;
			}

			if (placeProcessingOverlay != null) {
				placeProcessingOverlay.Dispose ();
				placeProcessingOverlay = null;
			}

			if (processingBackgroundView != null) {
				processingBackgroundView.Dispose ();
				processingBackgroundView = null;
			}

			if (processingLabel != null) {
				processingLabel.Dispose ();
				processingLabel = null;
			}

			if (profileImage != null) {
				profileImage.Dispose ();
				profileImage = null;
			}

			if (profileNameLabel != null) {
				profileNameLabel.Dispose ();
				profileNameLabel = null;
			}

			if (publicationInfoLabel != null) {
				publicationInfoLabel.Dispose ();
				publicationInfoLabel = null;
			}

			if (shareButton != null) {
				shareButton.Dispose ();
				shareButton = null;
			}

			if (stubImageView != null) {
				stubImageView.Dispose ();
				stubImageView = null;
			}

			if (videoNameLabel != null) {
				videoNameLabel.Dispose ();
				videoNameLabel = null;
			}

			if (videoView != null) {
				videoView.Dispose ();
				videoView = null;
			}
		}
	}
}
