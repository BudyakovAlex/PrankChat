// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PrankChat.Mobile.iOS.Views.Order
{
	[Register ("CreateOrderView")]
	partial class CreateOrderView
	{
		[Outlet]
		PrankChat.Mobile.iOS.Controls.FloatPlaceholderTextField completeDateTextField { get; set; }

		[Outlet]
		UIKit.UIButton createButton { get; set; }

		[Outlet]
		UIKit.UIView descriptionContainerView { get; set; }

		[Outlet]
		UIKit.UILabel descriptionPlaceholderLabel { get; set; }

		[Outlet]
		UIKit.UITextView descriptionTextView { get; set; }

		[Outlet]
		UIKit.UILabel descriptionTopFloatingPlaceholderLabel { get; set; }

		[Outlet]
		PrankChat.Mobile.iOS.Controls.CheckBox HideExecutorCheckBoxButton { get; set; }

		[Outlet]
		UIKit.UILabel hideExecutorCheckboxLabel { get; set; }

		[Outlet]
		UIKit.UIImageView InfoImageView { get; set; }

		[Outlet]
		Airbnb.Lottie.LOTAnimationView lottieAnimationView { get; set; }

		[Outlet]
		PrankChat.Mobile.iOS.Controls.FloatPlaceholderTextField nameTextField { get; set; }

		[Outlet]
		PrankChat.Mobile.iOS.Controls.FloatPlaceholderTextField priceTextField { get; set; }

		[Outlet]
		UIKit.UILabel privacyPolicyLabel { get; set; }

		[Outlet]
		UIKit.UIView progressBarView { get; set; }

		[Outlet]
		UIKit.UIView rootView { get; set; }

		[Outlet]
		UIKit.UIScrollView scrollView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint scrollViewBottomConstraint { get; set; }

		[Outlet]
		UIKit.UIStackView stackView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint TextViewHeightConstraint { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (privacyPolicyLabel != null) {
				privacyPolicyLabel.Dispose ();
				privacyPolicyLabel = null;
			}

			if (completeDateTextField != null) {
				completeDateTextField.Dispose ();
				completeDateTextField = null;
			}

			if (createButton != null) {
				createButton.Dispose ();
				createButton = null;
			}

			if (descriptionContainerView != null) {
				descriptionContainerView.Dispose ();
				descriptionContainerView = null;
			}

			if (descriptionPlaceholderLabel != null) {
				descriptionPlaceholderLabel.Dispose ();
				descriptionPlaceholderLabel = null;
			}

			if (descriptionTextView != null) {
				descriptionTextView.Dispose ();
				descriptionTextView = null;
			}

			if (descriptionTopFloatingPlaceholderLabel != null) {
				descriptionTopFloatingPlaceholderLabel.Dispose ();
				descriptionTopFloatingPlaceholderLabel = null;
			}

			if (HideExecutorCheckBoxButton != null) {
				HideExecutorCheckBoxButton.Dispose ();
				HideExecutorCheckBoxButton = null;
			}

			if (hideExecutorCheckboxLabel != null) {
				hideExecutorCheckboxLabel.Dispose ();
				hideExecutorCheckboxLabel = null;
			}

			if (InfoImageView != null) {
				InfoImageView.Dispose ();
				InfoImageView = null;
			}

			if (lottieAnimationView != null) {
				lottieAnimationView.Dispose ();
				lottieAnimationView = null;
			}

			if (nameTextField != null) {
				nameTextField.Dispose ();
				nameTextField = null;
			}

			if (priceTextField != null) {
				priceTextField.Dispose ();
				priceTextField = null;
			}

			if (progressBarView != null) {
				progressBarView.Dispose ();
				progressBarView = null;
			}

			if (rootView != null) {
				rootView.Dispose ();
				rootView = null;
			}

			if (scrollView != null) {
				scrollView.Dispose ();
				scrollView = null;
			}

			if (scrollViewBottomConstraint != null) {
				scrollViewBottomConstraint.Dispose ();
				scrollViewBottomConstraint = null;
			}

			if (stackView != null) {
				stackView.Dispose ();
				stackView = null;
			}

			if (TextViewHeightConstraint != null) {
				TextViewHeightConstraint.Dispose ();
				TextViewHeightConstraint = null;
			}
		}
	}
}
