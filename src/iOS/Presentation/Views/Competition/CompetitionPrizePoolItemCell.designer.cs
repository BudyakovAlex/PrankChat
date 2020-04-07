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
	[Register ("CompetitionPrizePoolItemCell")]
	partial class CompetitionPrizePoolItemCell
	{
		[Outlet]
		UIKit.UILabel participantLabel { get; set; }

		[Outlet]
		UIKit.UILabel positionLabel { get; set; }

		[Outlet]
		UIKit.UILabel ratingLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (positionLabel != null) {
				positionLabel.Dispose ();
				positionLabel = null;
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
