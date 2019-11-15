// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PrankChat.Mobile.iOS.Presentation.Views.NotificationView
{
	[Register ("NotificationView")]
	partial class NotificationView
	{
		[Outlet]
		UIKit.UIButton backButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (backButton != null) {
				backButton.Dispose ();
				backButton = null;
			}
		}
	}
}
