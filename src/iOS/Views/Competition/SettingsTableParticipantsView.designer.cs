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
	[Register ("SettingsTableParticipantsView")]
	partial class SettingsTableParticipantsView
	{
		[Outlet]
		UIKit.UIImageView AddPlaceImageView { get; set; }

		[Outlet]
		UIKit.UILabel AddPlaceLabel { get; set; }

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

			if (ItemsTableView != null) {
				ItemsTableView.Dispose ();
				ItemsTableView = null;
			}

			if (LeftToDistributeLabel != null) {
				LeftToDistributeLabel.Dispose ();
				LeftToDistributeLabel = null;
			}

			if (WarningLabel != null) {
				WarningLabel.Dispose ();
				WarningLabel = null;
			}

			if (AddPlaceImageView != null) {
				AddPlaceImageView.Dispose ();
				AddPlaceImageView = null;
			}

			if (AddPlaceLabel != null) {
				AddPlaceLabel.Dispose ();
				AddPlaceLabel = null;
			}
		}
	}
}
