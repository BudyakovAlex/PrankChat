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
	[Register ("RegistrationView")]
	partial class RegistrationView
	{
		[Outlet]
		UIKit.UIView contentContainerView { get; set; }

		[Outlet]
		UIKit.UITextField emailTextField { get; set; }

		[Outlet]
		UIKit.UIButton facebookButton { get; set; }

		[Outlet]
		UIKit.UIButton gmailButton { get; set; }

		[Outlet]
		UIKit.UIButton nextStepButton { get; set; }

		[Outlet]
		UIKit.UIButton okButton { get; set; }

		[Outlet]
		UIKit.UILabel registrationTitleLabel { get; set; }

		[Outlet]
		UIKit.UIScrollView scrollView { get; set; }

		[Outlet]
		UIKit.UIButton showLoginButton { get; set; }

		[Outlet]
		UIKit.UILabel socialNetworksLabel { get; set; }

		[Outlet]
		UIKit.UIButton vkButton { get; set; }

		[Outlet]
		UIKit.UILabel yetRegisteredLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (nextStepButton != null) {
				nextStepButton.Dispose ();
				nextStepButton = null;
			}

			if (registrationTitleLabel != null) {
				registrationTitleLabel.Dispose ();
				registrationTitleLabel = null;
			}

			if (emailTextField != null) {
				emailTextField.Dispose ();
				emailTextField = null;
			}

			if (socialNetworksLabel != null) {
				socialNetworksLabel.Dispose ();
				socialNetworksLabel = null;
			}

			if (vkButton != null) {
				vkButton.Dispose ();
				vkButton = null;
			}

			if (okButton != null) {
				okButton.Dispose ();
				okButton = null;
			}

			if (facebookButton != null) {
				facebookButton.Dispose ();
				facebookButton = null;
			}

			if (gmailButton != null) {
				gmailButton.Dispose ();
				gmailButton = null;
			}

			if (yetRegisteredLabel != null) {
				yetRegisteredLabel.Dispose ();
				yetRegisteredLabel = null;
			}

			if (showLoginButton != null) {
				showLoginButton.Dispose ();
				showLoginButton = null;
			}

			if (scrollView != null) {
				scrollView.Dispose ();
				scrollView = null;
			}

			if (contentContainerView != null) {
				contentContainerView.Dispose ();
				contentContainerView = null;
			}
		}
	}
}
