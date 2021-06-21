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
	[Register ("PasswordRecoveryView")]
	partial class PasswordRecoveryView
	{
		[Outlet]
		UIKit.UITextField emailEditText { get; set; }

		[Outlet]
		UIKit.UIActivityIndicatorView progresBar { get; set; }

		[Outlet]
		UIKit.UIButton recoverPasswordButton { get; set; }

		[Outlet]
		UIKit.UILabel titleLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (emailEditText != null) {
				emailEditText.Dispose ();
				emailEditText = null;
			}

			if (recoverPasswordButton != null) {
				recoverPasswordButton.Dispose ();
				recoverPasswordButton = null;
			}

			if (titleLabel != null) {
				titleLabel.Dispose ();
				titleLabel = null;
			}

			if (progresBar != null) {
				progresBar.Dispose ();
				progresBar = null;
			}
		}
	}
}
