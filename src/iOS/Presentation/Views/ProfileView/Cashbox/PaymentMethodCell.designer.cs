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
	[Register ("PaymentMethodCell")]
	partial class PaymentMethodCell
	{
		[Outlet]
		UIKit.UIView innerView { get; set; }

		[Outlet]
		UIKit.UIImageView paymentImageView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (innerView != null) {
				innerView.Dispose ();
				innerView = null;
			}

			if (paymentImageView != null) {
				paymentImageView.Dispose ();
				paymentImageView = null;
			}
		}
	}
}
