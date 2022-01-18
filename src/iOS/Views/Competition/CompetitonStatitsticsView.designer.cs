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
	[Register ("CompetitonStatitsticsView")]
	partial class CompetitonStatitsticsView
	{
		[Outlet]
		UIKit.UILabel ContributionTitleLabel { get; set; }

		[Outlet]
		UIKit.UILabel ContributionValueLabel { get; set; }

		[Outlet]
		UIKit.UILabel ParticipantsLabel { get; set; }

		[Outlet]
		UIKit.UILabel ParticipantsTitleLabel { get; set; }

		[Outlet]
		UIKit.UILabel PercentageFromContributionTitleLabel { get; set; }

		[Outlet]
		UIKit.UILabel PercentageFromContributionValueLabel { get; set; }

		[Outlet]
		UIKit.UILabel ProfitLabel { get; set; }

		[Outlet]
		UIKit.UILabel ProfitTitleLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ProfitLabel != null) {
				ProfitLabel.Dispose ();
				ProfitLabel = null;
			}

			if (ParticipantsLabel != null) {
				ParticipantsLabel.Dispose ();
				ParticipantsLabel = null;
			}

			if (PercentageFromContributionValueLabel != null) {
				PercentageFromContributionValueLabel.Dispose ();
				PercentageFromContributionValueLabel = null;
			}

			if (ContributionValueLabel != null) {
				ContributionValueLabel.Dispose ();
				ContributionValueLabel = null;
			}

			if (ContributionTitleLabel != null) {
				ContributionTitleLabel.Dispose ();
				ContributionTitleLabel = null;
			}

			if (ParticipantsTitleLabel != null) {
				ParticipantsTitleLabel.Dispose ();
				ParticipantsTitleLabel = null;
			}

			if (PercentageFromContributionTitleLabel != null) {
				PercentageFromContributionTitleLabel.Dispose ();
				PercentageFromContributionTitleLabel = null;
			}

			if (ProfitTitleLabel != null) {
				ProfitTitleLabel.Dispose ();
				ProfitTitleLabel = null;
			}
		}
	}
}
