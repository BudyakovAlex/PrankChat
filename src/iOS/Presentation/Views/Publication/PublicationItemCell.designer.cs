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
		UIKit.UIButton moreButton { get; set; }

		[Outlet]
		FFImageLoading.Cross.MvxCachedImageView profileImage { get; set; }

		[Outlet]
		UIKit.UILabel profileNameLabel { get; set; }

		[Outlet]
		UIKit.UILabel publicationInfoLabel { get; set; }

		[Outlet]
		UIKit.UIButton shareButton { get; set; }

		[Outlet]
		UIKit.UILabel shareLabel { get; set; }

		[Outlet]
		FFImageLoading.Cross.MvxCachedImageView videoImage { get; set; }

		[Outlet]
		UIKit.UILabel videoNameLabel { get; set; }
		
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

			if (likeLabel != null) {
				likeLabel.Dispose ();
				likeLabel = null;
			}

			if (moreButton != null) {
				moreButton.Dispose ();
				moreButton = null;
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

			if (shareLabel != null) {
				shareLabel.Dispose ();
				shareLabel = null;
			}

			if (videoImage != null) {
				videoImage.Dispose ();
				videoImage = null;
			}

			if (videoNameLabel != null) {
				videoNameLabel.Dispose ();
				videoNameLabel = null;
			}
		}
	}
}
