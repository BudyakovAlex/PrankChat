// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PrankChat.Mobile.iOS.Presentation.Views.ProfileView.Cashbox
{
	[Register ("WithdrawalView")]
	partial class WithdrawalView
	{
		[Outlet]
		UIKit.UIButton attachDocumentButton { get; set; }

		[Outlet]
		UIKit.UILabel availableAmountTitleLabel { get; set; }

		[Outlet]
		UIKit.UITextField costTextField { get; set; }

		[Outlet]
		UIKit.UIView creditCardView { get; set; }

		[Outlet]
		UIKit.UIView pendingVerifyUserView { get; set; }

		[Outlet]
		UIKit.UIImageView questionImageView { get; set; }

		[Outlet]
		UIKit.UIView verifyUserView { get; set; }

		[Outlet]
		UIKit.UIView verticalSeparatorView { get; set; }

		[Outlet]
		UIKit.UIButton withdrawButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (availableAmountTitleLabel != null) {
				availableAmountTitleLabel.Dispose ();
				availableAmountTitleLabel = null;
			}

			if (costTextField != null) {
				costTextField.Dispose ();
				costTextField = null;
			}

			if (questionImageView != null) {
				questionImageView.Dispose ();
				questionImageView = null;
			}

			if (verticalSeparatorView != null) {
				verticalSeparatorView.Dispose ();
				verticalSeparatorView = null;
			}

			if (withdrawButton != null) {
				withdrawButton.Dispose ();
				withdrawButton = null;
			}

			if (creditCardView != null) {
				creditCardView.Dispose ();
				creditCardView = null;
			}

			if (verifyUserView != null) {
				verifyUserView.Dispose ();
				verifyUserView = null;
			}

			if (pendingVerifyUserView != null) {
				pendingVerifyUserView.Dispose ();
				pendingVerifyUserView = null;
			}

			if (attachDocumentButton != null) {
				attachDocumentButton.Dispose ();
				attachDocumentButton = null;
			}
		}
	}
}
