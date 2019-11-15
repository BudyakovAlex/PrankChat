// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PrankChat.Mobile.iOS.Presentation.Views.LoginView
{
	[Register ("LoginView")]
	partial class LoginView
	{
		[Outlet]
		UIKit.UIButton loginButton { get; set; }

		[Outlet]
		UIKit.UIButton registrationButton { get; set; }

		[Outlet]
		UIKit.UIButton resetPasswordButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (loginButton != null) {
				loginButton.Dispose ();
				loginButton = null;
			}

			if (registrationButton != null) {
				registrationButton.Dispose ();
				registrationButton = null;
			}

			if (resetPasswordButton != null) {
				resetPasswordButton.Dispose ();
				resetPasswordButton = null;
			}
		}
	}
}
