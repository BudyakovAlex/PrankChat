// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PrankChat.Mobile.iOS.Views.Maintanance
{
	[Register ("MaintananceView")]
	partial class MaintananceView
	{
		[Outlet]
		UIKit.UIButton downloadButton { get; set; }

		[Outlet]
		UIKit.UILabel titleLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (titleLabel != null) {
				titleLabel.Dispose ();
				titleLabel = null;
			}

			if (downloadButton != null) {
				downloadButton.Dispose ();
				downloadButton = null;
			}
		}
	}
}
