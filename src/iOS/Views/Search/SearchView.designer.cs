// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PrankChat.Mobile.iOS.Views.Search
{
	[Register ("SearchView")]
	partial class SearchView
	{
		[Outlet]
		UIKit.UIView loadingView { get; set; }

		[Outlet]
		Airbnb.Lottie.LOTAnimationView lottieAnimationView { get; set; }

		[Outlet]
		UIKit.UITableView ordersTableView { get; set; }

		[Outlet]
		UIKit.UITableView peoplesTableView { get; set; }

		[Outlet]
		PrankChat.Mobile.iOS.Controls.TabView tabView { get; set; }

		[Outlet]
		UIKit.UITableView videosTableView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (loadingView != null) {
				loadingView.Dispose ();
				loadingView = null;
			}

			if (lottieAnimationView != null) {
				lottieAnimationView.Dispose ();
				lottieAnimationView = null;
			}

			if (tabView != null) {
				tabView.Dispose ();
				tabView = null;
			}

			if (peoplesTableView != null) {
				peoplesTableView.Dispose ();
				peoplesTableView = null;
			}

			if (videosTableView != null) {
				videosTableView.Dispose ();
				videosTableView = null;
			}

			if (ordersTableView != null) {
				ordersTableView.Dispose ();
				ordersTableView = null;
			}
		}
	}
}
