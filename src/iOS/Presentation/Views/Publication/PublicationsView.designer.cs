// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PrankChat.Mobile.iOS.Presentation.Views.Publication
{
	[Register ("PublicationsView")]
	partial class PublicationsView
	{
		[Outlet]
		UIKit.UIView bottomSeparatorView { get; set; }

		[Outlet]
		UIKit.UIImageView filterArrowImageView { get; set; }

		[Outlet]
		UIKit.UIView filterContainerView { get; set; }

		[Outlet]
		UIKit.UILabel filterTitleLabel { get; set; }

		[Outlet]
		UIKit.UIView filterView { get; set; }

		[Outlet]
		UIKit.UIView loadingOverlayView { get; set; }

		[Outlet]
		Airbnb.Lottie.LOTAnimationView lottieAnimationView { get; set; }

		[Outlet]
		UIKit.UIStackView publicationTypeStackView { get; set; }

		[Outlet]
		UIKit.UITableView tableView { get; set; }

		[Outlet]
		UIKit.UIView topSeparatorView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (bottomSeparatorView != null) {
				bottomSeparatorView.Dispose ();
				bottomSeparatorView = null;
			}

			if (filterArrowImageView != null) {
				filterArrowImageView.Dispose ();
				filterArrowImageView = null;
			}

			if (filterContainerView != null) {
				filterContainerView.Dispose ();
				filterContainerView = null;
			}

			if (filterTitleLabel != null) {
				filterTitleLabel.Dispose ();
				filterTitleLabel = null;
			}

			if (filterView != null) {
				filterView.Dispose ();
				filterView = null;
			}

			if (publicationTypeStackView != null) {
				publicationTypeStackView.Dispose ();
				publicationTypeStackView = null;
			}

			if (tableView != null) {
				tableView.Dispose ();
				tableView = null;
			}

			if (topSeparatorView != null) {
				topSeparatorView.Dispose ();
				topSeparatorView = null;
			}

			if (loadingOverlayView != null) {
				loadingOverlayView.Dispose ();
				loadingOverlayView = null;
			}

			if (lottieAnimationView != null) {
				lottieAnimationView.Dispose ();
				lottieAnimationView = null;
			}
		}
	}
}
