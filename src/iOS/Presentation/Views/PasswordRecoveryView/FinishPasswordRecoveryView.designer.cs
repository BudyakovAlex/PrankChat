// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PrankChat.Mobile.iOS.Presentation.Views.PasswordRecoveryView
{
	[Register ("FinishPasswordRecoveryView")]
	partial class FinishPasswordRecoveryView
	{
		[Outlet]
		UIKit.UIButton confirmButton { get; set; }

		[Outlet]
		UIKit.UILabel messageLabel { get; set; }

		[Outlet]
		UIKit.UIButton showPublicationButton { get; set; }

		[Outlet]
		UIKit.UILabel titleLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (confirmButton != null) {
				confirmButton.Dispose ();
				confirmButton = null;
			}

			if (messageLabel != null) {
				messageLabel.Dispose ();
				messageLabel = null;
			}

			if (titleLabel != null) {
				titleLabel.Dispose ();
				titleLabel = null;
			}

			if (showPublicationButton != null) {
				showPublicationButton.Dispose ();
				showPublicationButton = null;
			}
		}
	}
}
