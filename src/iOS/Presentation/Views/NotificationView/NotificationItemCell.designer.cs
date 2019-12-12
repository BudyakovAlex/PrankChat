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
		UIKit.UILabel descriptionLabel { get; set; }

		[Outlet]
		UIKit.UILabel profileNameLabel { get; set; }

		[Outlet]
		FFImageLoading.Cross.MvxCachedImageView profilePhotoImageView { get; set; }

		[Outlet]
		UIKit.UILabel statusLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (profilePhotoImageView != null) {
				profilePhotoImageView.Dispose ();
				profilePhotoImageView = null;
			}

			if (profileNameLabel != null) {
				profileNameLabel.Dispose ();
				profileNameLabel = null;
			}

			if (descriptionLabel != null) {
				descriptionLabel.Dispose ();
				descriptionLabel = null;
			}

			if (statusLabel != null) {
				statusLabel.Dispose ();
				statusLabel = null;
			}

			if (dateLabel != null) {
				dateLabel.Dispose ();
				dateLabel = null;
			}
		}
	}
}
