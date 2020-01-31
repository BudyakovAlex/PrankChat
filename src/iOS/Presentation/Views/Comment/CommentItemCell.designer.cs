// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PrankChat.Mobile.iOS.Presentation.Views.Comment
{
	[Register ("CommentItemCell")]
	partial class CommentItemCell
	{
		[Outlet]
		UIKit.UILabel commentDateLabel { get; set; }

		[Outlet]
		FFImageLoading.Cross.MvxCachedImageView profileImageView { get; set; }

		[Outlet]
		UIKit.UILabel profileNameLabel { get; set; }

		[Outlet]
		UIKit.UILabel profileShortName { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (profileShortName != null) {
				profileShortName.Dispose ();
				profileShortName = null;
			}

			if (commentDateLabel != null) {
				commentDateLabel.Dispose ();
				commentDateLabel = null;
			}

			if (profileImageView != null) {
				profileImageView.Dispose ();
				profileImageView = null;
			}

			if (profileNameLabel != null) {
				profileNameLabel.Dispose ();
				profileNameLabel = null;
			}
		}
	}
}
