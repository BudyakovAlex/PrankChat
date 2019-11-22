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
		PrankChat.Mobile.iOS.Controls.CustomSegmentedControl publicationTypeSegment { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (publicationTypeSegment != null) {
				publicationTypeSegment.Dispose ();
				publicationTypeSegment = null;
			}
		}
	}
}
