// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PrankChat.Mobile.iOS.Views.Comment
{
	[Register ("CommentItemCell")]
	partial class CommentItemCell
	{
		[Outlet]
		UIKit.UILabel commentDateLabel { get; set; }

		[Outlet]
		UIKit.UILabel commentLabel { get; set; }

		[Outlet]
		PrankChat.Mobile.iOS.Controls.CircleCachedImageView profileImageView { get; set; }

		[Outlet]
		UIKit.UILabel profileNameLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (commentDateLabel != null) {
				commentDateLabel.Dispose ();
				commentDateLabel = null;
			}

			if (commentLabel != null) {
				commentLabel.Dispose ();
				commentLabel = null;
			}

			if (profileNameLabel != null) {
				profileNameLabel.Dispose ();
				profileNameLabel = null;
			}

			if (profileImageView != null) {
				profileImageView.Dispose ();
				profileImageView = null;
			}
		}
	}
}
