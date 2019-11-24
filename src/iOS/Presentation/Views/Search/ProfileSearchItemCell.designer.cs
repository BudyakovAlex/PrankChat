// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PrankChat.Mobile.iOS.Presentation.Views.Search
{
	[Register ("ProfileSearchItemCell")]
	partial class ProfileSearchItemCell
	{
		[Outlet]
		UIKit.UIView innerView { get; set; }

		[Outlet]
		UIKit.UILabel profileDescriptionLabel { get; set; }

		[Outlet]
		FFImageLoading.Cross.MvxCachedImageView profileImageView { get; set; }

		[Outlet]
		UIKit.UILabel profileNameLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (innerView != null) {
				innerView.Dispose ();
				innerView = null;
			}

			if (profileDescriptionLabel != null) {
				profileDescriptionLabel.Dispose ();
				profileDescriptionLabel = null;
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
