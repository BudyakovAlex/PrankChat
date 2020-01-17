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
		UIKit.UITextField birthdayTextField { get; set; }

		[Outlet]
		UIKit.UIView femaleButtonsContainerView { get; set; }

		[Outlet]
		UIKit.UIButton femaleIconButton { get; set; }

		[Outlet]
		UIKit.UIButton femaleTitleButton { get; set; }

		[Outlet]
		UIKit.UIView maleButtonsContainerView { get; set; }

		[Outlet]
		UIKit.UIButton maleIconButton { get; set; }

		[Outlet]
		UIKit.UIButton maleTitleButton { get; set; }

		[Outlet]
		UIKit.UITextField nameTextField { get; set; }

		[Outlet]
		UIKit.UIButton nextStepButton { get; set; }

		[Outlet]
		UIKit.UITextField nicknameTextField { get; set; }

		[Outlet]
		UIKit.UITextField passwordRepeatTextField { get; set; }

		[Outlet]
		UIKit.UITextField passwordTextField { get; set; }

		[Outlet]
		UIKit.UIActivityIndicatorView progressBar { get; set; }

		[Outlet]
		UIKit.UIButton registerButton { get; set; }

		[Outlet]
		UIKit.UIScrollView scrollView { get; set; }

		[Outlet]
		UIKit.UILabel sexSelectTitleLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (progressBar != null) {
				progressBar.Dispose ();
				progressBar = null;
			}

			if (birthdayTextField != null) {
				birthdayTextField.Dispose ();
				birthdayTextField = null;
			}

			if (femaleButtonsContainerView != null) {
				femaleButtonsContainerView.Dispose ();
				femaleButtonsContainerView = null;
			}

			if (femaleIconButton != null) {
				femaleIconButton.Dispose ();
				femaleIconButton = null;
			}

			if (femaleTitleButton != null) {
				femaleTitleButton.Dispose ();
				femaleTitleButton = null;
			}

			if (maleButtonsContainerView != null) {
				maleButtonsContainerView.Dispose ();
				maleButtonsContainerView = null;
			}

			if (maleIconButton != null) {
				maleIconButton.Dispose ();
				maleIconButton = null;
			}

			if (maleTitleButton != null) {
				maleTitleButton.Dispose ();
				maleTitleButton = null;
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

			if (registerButton != null) {
				registerButton.Dispose ();
				registerButton = null;
			}

			if (scrollView != null) {
				scrollView.Dispose ();
				scrollView = null;
			}

			if (sexSelectTitleLabel != null) {
				sexSelectTitleLabel.Dispose ();
				sexSelectTitleLabel = null;
			}
		}
	}
}
