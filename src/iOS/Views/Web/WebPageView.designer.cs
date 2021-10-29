// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using PrankChat.Mobile.iOS.Controls;
using System.CodeDom.Compiler;

namespace PrankChat.Mobile.iOS.Views.Web
{
	[Register ("WebPageView")]
	partial class WebPageView
	{
		[Outlet]
		CustomWKWebView webView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (webView != null) {
				webView.Dispose ();
				webView = null;
			}
		}
	}
}
