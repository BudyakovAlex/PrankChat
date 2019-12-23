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
	[Register ("PublicationDetailsView")]
	partial class PublicationDetailsView
	{
		[Outlet]
		UIKit.UILabel commentatorNameLabel { get; set; }

		[Outlet]
		FFImageLoading.Cross.MvxCachedImageView commentatorPhotoImageView { get; set; }

		[Outlet]
		UIKit.UIButton commentButton { get; set; }

		[Outlet]
		UIKit.UILabel commentDateLabel { get; set; }

		[Outlet]
		UIKit.UILabel commentLabel { get; set; }

		[Outlet]
		UIKit.UIView commentView { get; set; }

		[Outlet]
		UIKit.UIButton likeButton { get; set; }

		[Outlet]
		UIKit.UILabel likeLabel { get; set; }

		[Outlet]
		UIKit.UIButton moreButton { get; set; }

		[Outlet]
		UIKit.UILabel profileNameLabel { get; set; }

		[Outlet]
		FFImageLoading.Cross.MvxCachedImageView profilePhotoImageView { get; set; }

		[Outlet]
		UIKit.UILabel publicationInfoLabel { get; set; }

		[Outlet]
		UIKit.UIButton shareButton { get; set; }

		[Outlet]
		UIKit.UILabel shareLabel { get; set; }

		[Outlet]
		UIKit.UILabel videoDescriptionButton { get; set; }

		[Outlet]
		FFImageLoading.Cross.MvxCachedImageView videoImageView { get; set; }

		[Outlet]
		UIKit.UILabel videoNameLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (shareLabel != null) {
				shareLabel.Dispose ();
				shareLabel = null;
			}

			if (commentatorNameLabel != null) {
				commentatorNameLabel.Dispose ();
				commentatorNameLabel = null;
			}

			if (commentatorPhotoImageView != null) {
				commentatorPhotoImageView.Dispose ();
				commentatorPhotoImageView = null;
			}

			if (commentButton != null) {
				commentButton.Dispose ();
				commentButton = null;
			}

			if (commentDateLabel != null) {
				commentDateLabel.Dispose ();
				commentDateLabel = null;
			}

			if (commentLabel != null) {
				commentLabel.Dispose ();
				commentLabel = null;
			}

			if (commentView != null) {
				commentView.Dispose ();
				commentView = null;
			}

			if (likeButton != null) {
				likeButton.Dispose ();
				likeButton = null;
			}

			if (likeLabel != null) {
				likeLabel.Dispose ();
				likeLabel = null;
			}

			if (moreButton != null) {
				moreButton.Dispose ();
				moreButton = null;
			}

			if (profileNameLabel != null) {
				profileNameLabel.Dispose ();
				profileNameLabel = null;
			}

			if (profilePhotoImageView != null) {
				profilePhotoImageView.Dispose ();
				profilePhotoImageView = null;
			}

			if (publicationInfoLabel != null) {
				publicationInfoLabel.Dispose ();
				publicationInfoLabel = null;
			}

			if (shareButton != null) {
				shareButton.Dispose ();
				shareButton = null;
			}

			if (videoDescriptionButton != null) {
				videoDescriptionButton.Dispose ();
				videoDescriptionButton = null;
			}

			if (videoImageView != null) {
				videoImageView.Dispose ();
				videoImageView = null;
			}

			if (videoNameLabel != null) {
				videoNameLabel.Dispose ();
				videoNameLabel = null;
			}
		}
	}
}
