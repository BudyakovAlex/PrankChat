// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PrankChat.Mobile.iOS.Presentation.Views.NotificationView
{
	[Register ("NotificationItemCell")]
	partial class NotificationItemCell
	{
		[Outlet]
		UIKit.UILabel dateLabel { get; set; }

		[Outlet]
		UIKit.UILabel profileNameAndDescriptionLabel { get; set; }

		[Outlet]
		PrankChat.Mobile.iOS.Controls.CircleCachedImageView profilePhotoImageView { get; set; }

        [Outlet]
        UIKit.UIView isReadedView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (dateLabel != null) {
				dateLabel.Dispose ();
				dateLabel = null;
			}

			if (profileNameAndDescriptionLabel != null) {
				profileNameAndDescriptionLabel.Dispose ();
				profileNameAndDescriptionLabel = null;
			}

			if (profilePhotoImageView != null) {
				profilePhotoImageView.Dispose ();
				profilePhotoImageView = null;
			}

            if (isReadedView != null) {
				isReadedView.Dispose ();
				isReadedView = null;
            }
		}
	}
}
