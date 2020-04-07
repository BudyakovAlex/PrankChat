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
	[Register ("CompetitionPrizePoolView")]
	partial class CompetitionPrizePoolView
	{
		[Outlet]
		UIKit.UILabel participantLabel { get; set; }

		[Outlet]
		UIKit.UILabel prizePoolLabel { get; set; }

		[Outlet]
		UIKit.UILabel ratingLabel { get; set; }

		[Outlet]
		UIKit.UITableView tableView { get; set; }

		[Outlet]
		UIKit.UILabel titleLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (titleLabel != null) {
				titleLabel.Dispose ();
				titleLabel = null;
			}

			if (prizePoolLabel != null) {
				prizePoolLabel.Dispose ();
				prizePoolLabel = null;
			}

			if (tableView != null) {
				tableView.Dispose ();
				tableView = null;
			}

			if (participantLabel != null) {
				participantLabel.Dispose ();
				participantLabel = null;
			}

			if (ratingLabel != null) {
				ratingLabel.Dispose ();
				ratingLabel = null;
			}
		}
	}
}
