// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PrankChat.Mobile.iOS.Presentation.Views.Search
{
	[Register ("SearchView")]
	partial class SearchView
	{
		[Outlet]
		UIKit.UIView loadingView { get; set; }

		[Outlet]
		Airbnb.Lottie.LOTAnimationView lottieAnimationView { get; set; }

		[Outlet]
		UIKit.UITableView tableView { get; set; }

		[Outlet]
		PrankChat.Mobile.iOS.Controls.TabView tabView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (tabView != null) {
				tabView.Dispose ();
				tabView = null;
			}

			if (tableView != null) {
				tableView.Dispose ();
				tableView = null;
			}

			if (loadingView != null) {
				loadingView.Dispose ();
				loadingView = null;
			}

			if (lottieAnimationView != null) {
				lottieAnimationView.Dispose ();
				lottieAnimationView = null;
			}
		}
	}
}
