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
	[Register ("MyCompetitionsView")]
	partial class MyCompetitionsView
	{
		[Outlet]
		UIKit.UITableView TableView { get; set; }

		[Outlet]
		PrankChat.Mobile.iOS.Controls.TabView TabView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (TabView != null) {
				TabView.Dispose ();
				TabView = null;
			}

			if (TableView != null) {
				TableView.Dispose ();
				TableView = null;
			}
		}
	}
}
