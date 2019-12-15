// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PrankChat.Mobile.iOS.Presentation.Views.Order
{
	[Register ("DetailsOrderView")]
	partial class DetailsOrderView
	{
		[Outlet]
		UIKit.UIView executorView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (executorView != null) {
				executorView.Dispose ();
				executorView = null;
			}
		}
	}
}
