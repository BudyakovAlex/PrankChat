// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PrankChat.Mobile.iOS.Views.Competition.Items
{
	[Register ("CompetitionItemTableViewCell")]
	partial class CompetitionItemTableViewCell
	{
		[Outlet]
		PrankChat.Mobile.iOS.Controls.CompetitionView competitionView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (competitionView != null) {
				competitionView.Dispose ();
				competitionView = null;
			}
		}
	}
}
