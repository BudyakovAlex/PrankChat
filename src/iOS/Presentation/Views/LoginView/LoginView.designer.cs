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
		UIKit.UITextField emailTextField { get; set; }

		[Outlet]
		UIKit.UIButton facebookButton { get; set; }

		[Outlet]
		UIKit.UILabel forgotPasswordTitleLabel { get; set; }

		[Outlet]
		UIKit.UIButton gmailButton { get; set; }

		[Outlet]
		UIKit.UIButton loginButton { get; set; }

		[Outlet]
		UIKit.UILabel loginTitleLabel { get; set; }

		[Outlet]
		UIKit.UIButton okButton { get; set; }

		[Outlet]
		UIKit.UITextField passwordTextField { get; set; }

		[Outlet]
		UIKit.UIButton registrationButton { get; set; }

		[Outlet]
		UIKit.UIButton resetPasswordButton { get; set; }

		[Outlet]
		UIKit.UILabel socialNetworksTitleLabel { get; set; }

		[Outlet]
		UIKit.UIButton vkButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (emailTextField != null) {
				emailTextField.Dispose ();
				emailTextField = null;
			}

			if (facebookButton != null) {
				facebookButton.Dispose ();
				facebookButton = null;
			}

			if (gmailButton != null) {
				gmailButton.Dispose ();
				gmailButton = null;
			}

			if (loginButton != null) {
				loginButton.Dispose ();
				loginButton = null;
			}

			if (okButton != null) {
				okButton.Dispose ();
				okButton = null;
			}

			if (passwordTextField != null) {
				passwordTextField.Dispose ();
				passwordTextField = null;
			}

			if (registrationButton != null) {
				registrationButton.Dispose ();
				registrationButton = null;
			}

			if (resetPasswordButton != null) {
				resetPasswordButton.Dispose ();
				resetPasswordButton = null;
			}

			if (vkButton != null) {
				vkButton.Dispose ();
				vkButton = null;
			}

			if (loginTitleLabel != null) {
				loginTitleLabel.Dispose ();
				loginTitleLabel = null;
			}

			if (forgotPasswordTitleLabel != null) {
				forgotPasswordTitleLabel.Dispose ();
				forgotPasswordTitleLabel = null;
			}

			if (socialNetworksTitleLabel != null) {
				socialNetworksTitleLabel.Dispose ();
				socialNetworksTitleLabel = null;
			}
		}
	}
}
