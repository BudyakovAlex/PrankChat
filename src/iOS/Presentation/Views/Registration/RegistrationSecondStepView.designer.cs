// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PrankChat.Mobile.iOS.Presentation.Views.Registration
{
	[Register ("RegistrationSecondStepView")]
	partial class RegistrationSecondStepView
	{
		[Outlet]
		UIKit.UIButton adultCheckButton { get; set; }

		[Outlet]
		UIKit.UIView adultContainerView { get; set; }

		[Outlet]
		UIKit.UILabel adultLabel { get; set; }

		[Outlet]
		UIKit.UILabel agreeWithLabel { get; set; }

		[Outlet]
		PrankChat.Mobile.iOS.Controls.FloatPlaceholderTextField birthdayTextField { get; set; }

		[Outlet]
		PrankChat.Mobile.iOS.Controls.FloatPlaceholderTextField nameTextField { get; set; }

		[Outlet]
		UIKit.UIButton nextStepButton { get; set; }

		[Outlet]
		PrankChat.Mobile.iOS.Controls.FloatPlaceholderTextField nicknameTextField { get; set; }

		[Outlet]
		PrankChat.Mobile.iOS.Controls.FloatPlaceholderTextField passwordRepeatTextField { get; set; }

		[Outlet]
		PrankChat.Mobile.iOS.Controls.FloatPlaceholderTextField passwordTextField { get; set; }

		[Outlet]
		UIKit.UIButton privacyCheckButton { get; set; }

		[Outlet]
		UIKit.UIView privacyContainerView { get; set; }

		[Outlet]
		UIKit.UIActivityIndicatorView progressBar { get; set; }

		[Outlet]
		UIKit.UIButton registerButton { get; set; }

		[Outlet]
		UIKit.UIScrollView scrollView { get; set; }

		[Outlet]
		UIKit.UIView termsBottomLineView { get; set; }

		[Outlet]
		UIKit.UILabel termsLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (adultContainerView != null) {
				adultContainerView.Dispose ();
				adultContainerView = null;
			}

			if (privacyContainerView != null) {
				privacyContainerView.Dispose ();
				privacyContainerView = null;
			}

			if (adultCheckButton != null) {
				adultCheckButton.Dispose ();
				adultCheckButton = null;
			}

			if (adultLabel != null) {
				adultLabel.Dispose ();
				adultLabel = null;
			}

			if (agreeWithLabel != null) {
				agreeWithLabel.Dispose ();
				agreeWithLabel = null;
			}

			if (birthdayTextField != null) {
				birthdayTextField.Dispose ();
				birthdayTextField = null;
			}

			if (nameTextField != null) {
				nameTextField.Dispose ();
				nameTextField = null;
			}

			if (nextStepButton != null) {
				nextStepButton.Dispose ();
				nextStepButton = null;
			}

			if (nicknameTextField != null) {
				nicknameTextField.Dispose ();
				nicknameTextField = null;
			}

			if (passwordRepeatTextField != null) {
				passwordRepeatTextField.Dispose ();
				passwordRepeatTextField = null;
			}

			if (passwordTextField != null) {
				passwordTextField.Dispose ();
				passwordTextField = null;
			}

			if (privacyCheckButton != null) {
				privacyCheckButton.Dispose ();
				privacyCheckButton = null;
			}

			if (progressBar != null) {
				progressBar.Dispose ();
				progressBar = null;
			}

			if (registerButton != null) {
				registerButton.Dispose ();
				registerButton = null;
			}

			if (scrollView != null) {
				scrollView.Dispose ();
				scrollView = null;
			}

			if (termsBottomLineView != null) {
				termsBottomLineView.Dispose ();
				termsBottomLineView = null;
			}

			if (termsLabel != null) {
				termsLabel.Dispose ();
				termsLabel = null;
			}
		}
	}
}
