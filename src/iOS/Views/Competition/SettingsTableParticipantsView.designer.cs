// WARNING
//
// This file has been generated automatically by Rider IDE
//   to store outlets and actions made in Xcode.
// If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PrankChat.Mobile.iOS.Views.Competition
{
	[Register ("SettingsTableParticipantsView")]
	partial class SettingsTableParticipantsView
	{
		[Outlet]
		UIKit.UIView AddPrizePlaceView { get; set; }

		[Outlet]
		UIKit.UIButton ApplyButton { get; set; }

		[Outlet]
		UIKit.UIScrollView ContentScrollView { get; set; }

		[Outlet]
		UIKit.UILabel FullPrizePoolLabel { get; set; }

		[Outlet]
		PrankChat.Mobile.iOS.Controls.SelfSizeTableView ItemsTableView { get; set; }

		[Outlet]
		UIKit.UILabel LeftToDistributeLabel { get; set; }

		[Outlet]
		UIKit.UILabel WarningLabel { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (AddPrizePlaceView != null) {
				AddPrizePlaceView.Dispose ();
				AddPrizePlaceView = null;
			}

			if (ApplyButton != null) {
				ApplyButton.Dispose ();
				ApplyButton = null;
			}

			if (ContentScrollView != null) {
				ContentScrollView.Dispose ();
				ContentScrollView = null;
			}

			if (FullPrizePoolLabel != null) {
				FullPrizePoolLabel.Dispose ();
				FullPrizePoolLabel = null;
			}

			if (LeftToDistributeLabel != null) {
				LeftToDistributeLabel.Dispose ();
				LeftToDistributeLabel = null;
			}

			if (WarningLabel != null) {
				WarningLabel.Dispose ();
				WarningLabel = null;
			}

			if (ItemsTableView != null) {
				ItemsTableView.Dispose ();
				ItemsTableView = null;
			}

		}
	}
}
