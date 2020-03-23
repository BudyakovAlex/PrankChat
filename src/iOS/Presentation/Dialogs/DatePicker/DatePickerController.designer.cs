// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PrankChat.Mobile.iOS.Presentation.Dialogs.DatePicker
{
	[Register ("DatePickerController")]
	partial class DatePickerController
	{
		[Outlet]
		UIKit.UIBarButtonItem cancelButton { get; set; }

		[Outlet]
		UIKit.UIDatePicker datePicker { get; set; }

		[Outlet]
		UIKit.UIBarButtonItem doneButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (datePicker != null) {
				datePicker.Dispose ();
				datePicker = null;
			}

			if (cancelButton != null) {
				cancelButton.Dispose ();
				cancelButton = null;
			}

			if (doneButton != null) {
				doneButton.Dispose ();
				doneButton = null;
			}
		}
	}
}
