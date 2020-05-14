// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PrankChat.Mobile.iOS.Presentation.Views.Walthroughs
{
	[Register ("WalthroughView")]
	partial class WalthroughView
	{
		[Outlet]
		UIKit.UIImageView closeImageView { get; set; }

		[Outlet]
		UIKit.UILabel descriptionLabel { get; set; }

		[Outlet]
		UIKit.UILabel titleLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (titleLabel != null) {
				titleLabel.Dispose ();
				titleLabel = null;
			}

			if (closeImageView != null) {
				closeImageView.Dispose ();
				closeImageView = null;
			}

			if (descriptionLabel != null) {
				descriptionLabel.Dispose ();
				descriptionLabel = null;
			}
		}
	}
}
