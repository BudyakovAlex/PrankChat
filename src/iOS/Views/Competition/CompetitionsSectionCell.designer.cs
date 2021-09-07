// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PrankChat.Mobile.iOS.Views.Competition
{
	[Register ("CompetitionsSectionCell")]
	partial class CompetitionsSectionCell
	{
		[Outlet]
		UIKit.UIButton backButton { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint buttonContainerZeroHeightConstraint { get; set; }

		[Outlet]
		UIKit.UICollectionView collectionView { get; set; }

		[Outlet]
		UIKit.UIButton forthButton { get; set; }

		[Outlet]
		UIKit.UIView leftTitleView { get; set; }

		[Outlet]
		UIKit.UIView rightTitleView { get; set; }

		[Outlet]
		UIKit.UILabel titleLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (rightTitleView != null) {
				rightTitleView.Dispose ();
				rightTitleView = null;
			}

			if (leftTitleView != null) {
				leftTitleView.Dispose ();
				leftTitleView = null;
			}

			if (backButton != null) {
				backButton.Dispose ();
				backButton = null;
			}

			if (buttonContainerZeroHeightConstraint != null) {
				buttonContainerZeroHeightConstraint.Dispose ();
				buttonContainerZeroHeightConstraint = null;
			}

			if (collectionView != null) {
				collectionView.Dispose ();
				collectionView = null;
			}

			if (forthButton != null) {
				forthButton.Dispose ();
				forthButton = null;
			}

			if (titleLabel != null) {
				titleLabel.Dispose ();
				titleLabel = null;
			}
		}
	}
}
