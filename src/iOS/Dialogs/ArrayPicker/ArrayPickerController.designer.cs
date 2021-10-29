// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PrankChat.Mobile.iOS.Dialogs.ArrayPicker
{
	[Register ("ArrayPickerController")]
	partial class ArrayPickerController
	{
		[Outlet]
		UIKit.UIPickerView arrayPickerView { get; set; }

		[Outlet]
		UIKit.UIBarButtonItem cancelButton { get; set; }

		[Outlet]
		UIKit.UIBarButtonItem doneButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (arrayPickerView != null) {
				arrayPickerView.Dispose ();
				arrayPickerView = null;
			}

			if (doneButton != null) {
				doneButton.Dispose ();
				doneButton = null;
			}

			if (cancelButton != null) {
				cancelButton.Dispose ();
				cancelButton = null;
			}
		}
	}
}
