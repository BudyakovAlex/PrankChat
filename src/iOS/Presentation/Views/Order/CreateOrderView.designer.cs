// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PrankChat.Mobile.iOS.Presentation.Views.Order
{
	[Register ("CreateOrderView")]
	partial class CreateOrderView
	{
		[Outlet]
		UIKit.UITextField completeDateTextField { get; set; }

		[Outlet]
		UIKit.UIButton createButton { get; set; }

		[Outlet]
		PrankChat.Mobile.iOS.Controls.PlaceholderTextView descriptionTextView { get; set; }

		[Outlet]
		UIKit.UIImageView hideExecuterCheckboxImageView { get; set; }

		[Outlet]
		UIKit.UILabel hideExecutorCheckboxLabel { get; set; }

		[Outlet]
		Airbnb.Lottie.LOTAnimationView lottieAnimationView { get; set; }

		[Outlet]
		UIKit.UITextField nameTextField { get; set; }

		[Outlet]
		UIKit.UITextField priceTextField { get; set; }

		[Outlet]
		UIKit.UIActivityIndicatorView progressBar { get; set; }

		[Outlet]
		UIKit.UIView progressBarView { get; set; }

		[Outlet]
		UIKit.UIScrollView scrollView { get; set; }

		[Outlet]
		UIKit.UIStackView stackView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (completeDateTextField != null) {
				completeDateTextField.Dispose ();
				completeDateTextField = null;
			}

			if (createButton != null) {
				createButton.Dispose ();
				createButton = null;
			}

			if (descriptionTextView != null) {
				descriptionTextView.Dispose ();
				descriptionTextView = null;
			}

			if (hideExecuterCheckboxImageView != null) {
				hideExecuterCheckboxImageView.Dispose ();
				hideExecuterCheckboxImageView = null;
			}

			if (hideExecutorCheckboxLabel != null) {
				hideExecutorCheckboxLabel.Dispose ();
				hideExecutorCheckboxLabel = null;
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

			if (lottieAnimationView != null) {
				lottieAnimationView.Dispose ();
				lottieAnimationView = null;
			}

			if (progressBar != null) {
				progressBar.Dispose ();
				progressBar = null;
			}

			if (scrollView != null) {
				scrollView.Dispose ();
				scrollView = null;
			}

			if (stackView != null) {
				stackView.Dispose ();
				stackView = null;
			}
		}
	}
}
