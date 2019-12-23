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
	[Register ("RefillView")]
	partial class RefillView
	{
		[Outlet]
		UIKit.UITextField costTextField { get; set; }

		[Outlet]
		UIKit.UIImageView mastercardImageView { get; set; }

		[Outlet]
		UIKit.UICollectionView paymentMethodsCollectionView { get; set; }

		[Outlet]
		UIKit.UILabel paymentMethodsTitleLabel { get; set; }

		[Outlet]
		UIKit.UIButton refillButton { get; set; }

		[Outlet]
		UIKit.UIImageView secureBannerImageView { get; set; }

		[Outlet]
		UIKit.UIImageView visaBannerImageView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (costTextField != null) {
				costTextField.Dispose ();
				costTextField = null;
			}

			if (refillButton != null) {
				refillButton.Dispose ();
				refillButton = null;
			}

			if (paymentMethodsTitleLabel != null) {
				paymentMethodsTitleLabel.Dispose ();
				paymentMethodsTitleLabel = null;
			}

			if (paymentMethodsCollectionView != null) {
				paymentMethodsCollectionView.Dispose ();
				paymentMethodsCollectionView = null;
			}

			if (mastercardImageView != null) {
				mastercardImageView.Dispose ();
				mastercardImageView = null;
			}

			if (visaBannerImageView != null) {
				visaBannerImageView.Dispose ();
				visaBannerImageView = null;
			}

			if (secureBannerImageView != null) {
				secureBannerImageView.Dispose ();
				secureBannerImageView = null;
			}
		}
	}
}
