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
	[Register ("RegistrationThirdStepView")]
	partial class RegistrationThirdStepView
	{
		[Outlet]
		UIKit.UILabel confirmationDescriptionLabel { get; set; }

		[Outlet]
		UIKit.UILabel congratsTitleLabel { get; set; }

		[Outlet]
		UIKit.UIButton finishRegistrationButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (finishRegistrationButton != null) {
				finishRegistrationButton.Dispose ();
				finishRegistrationButton = null;
			}

			if (congratsTitleLabel != null) {
				congratsTitleLabel.Dispose ();
				congratsTitleLabel = null;
			}

			if (confirmationDescriptionLabel != null) {
				confirmationDescriptionLabel.Dispose ();
				confirmationDescriptionLabel = null;
			}
		}
	}
}
