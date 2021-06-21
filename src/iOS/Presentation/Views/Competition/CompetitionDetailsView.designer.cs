// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PrankChat.Mobile.iOS.Presentation.Views.Competition
{
	[Register ("CompetitionDetailsView")]
	partial class CompetitionDetailsView
	{
		[Outlet]
		UIKit.UIView loadingView { get; set; }

		[Outlet]
		Airbnb.Lottie.LOTAnimationView lottieAnimationView { get; set; }

		[Outlet]
		UIKit.UITableView tableView { get; set; }

		[Outlet]
		UIKit.UIView uploadingBackgroundView { get; set; }

		[Outlet]
		UIKit.UIView uploadingInfoView { get; set; }

		[Outlet]
		UIKit.UILabel uploadingLabel { get; set; }

		[Outlet]
		PrankChat.Mobile.iOS.Controls.CircleProgressBar uploadingProgressBar { get; set; }
		
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

			if (tableView != null) {
				tableView.Dispose ();
				tableView = null;
			}

			if (uploadingBackgroundView != null) {
				uploadingBackgroundView.Dispose ();
				uploadingBackgroundView = null;
			}

			if (uploadingProgressBar != null) {
				uploadingProgressBar.Dispose ();
				uploadingProgressBar = null;
			}

			if (uploadingLabel != null) {
				uploadingLabel.Dispose ();
				uploadingLabel = null;
			}

			if (uploadingInfoView != null) {
				uploadingInfoView.Dispose ();
				uploadingInfoView = null;
			}
		}
	}
}
