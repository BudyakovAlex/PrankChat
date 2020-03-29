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
		UIKit.UIImageView filterArrowImageView { get; set; }

		[Outlet]
		UIKit.UIView filterContainerView { get; set; }

		[Outlet]
		UIKit.UILabel filterTitleLabel { get; set; }

		[Outlet]
		UIKit.UIStackView publicationTypeStackView { get; set; }

		[Outlet]
		UIKit.UITableView tableView { get; set; }

		[Outlet]
		UIKit.UIView topSeparatorView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
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

			if (tableView != null) {
				tableView.Dispose ();
				tableView = null;
			}

			if (publicationTypeStackView != null) {
				publicationTypeStackView.Dispose ();
				publicationTypeStackView = null;
			}

			if (topSeparatorView != null) {
				topSeparatorView.Dispose ();
				topSeparatorView = null;
			}
		}
	}
}
