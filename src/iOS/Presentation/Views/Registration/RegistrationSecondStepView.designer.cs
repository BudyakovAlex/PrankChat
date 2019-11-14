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
		UIKit.UIButton nextStepButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (nextStepButton != null) {
				nextStepButton.Dispose ();
				nextStepButton = null;
			}
		}
	}
}
