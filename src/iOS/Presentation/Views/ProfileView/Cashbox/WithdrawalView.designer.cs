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
		UIKit.UILabel availableAmountTitleLabel { get; set; }

		[Outlet]
		UIKit.UITextField costTextField { get; set; }

		[Outlet]
		UIKit.UICollectionView paymentMethodsCollectionView { get; set; }

		[Outlet]
		UIKit.UILabel paymentMethodsTitleLabel { get; set; }

		[Outlet]
		UIKit.UIImageView questionImageView { get; set; }

		[Outlet]
		UIKit.UIView verticalSeparatorView { get; set; }

		[Outlet]
		UIKit.UIButton withdrawButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (costTextField != null) {
				costTextField.Dispose ();
				costTextField = null;
			}

			if (withdrawButton != null) {
				withdrawButton.Dispose ();
				withdrawButton = null;
			}

			if (verticalSeparatorView != null) {
				verticalSeparatorView.Dispose ();
				verticalSeparatorView = null;
			}

			if (availableAmountTitleLabel != null) {
				availableAmountTitleLabel.Dispose ();
				availableAmountTitleLabel = null;
			}

			if (questionImageView != null) {
				questionImageView.Dispose ();
				questionImageView = null;
			}

			if (paymentMethodsTitleLabel != null) {
				paymentMethodsTitleLabel.Dispose ();
				paymentMethodsTitleLabel = null;
			}

			if (paymentMethodsCollectionView != null) {
				paymentMethodsCollectionView.Dispose ();
				paymentMethodsCollectionView = null;
			}
		}
	}
}
