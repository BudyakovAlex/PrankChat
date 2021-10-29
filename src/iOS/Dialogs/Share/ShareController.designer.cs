// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PrankChat.Mobile.iOS.Dialogs.Share
{
	[Register ("ShareController")]
	partial class ShareController
	{
		[Outlet]
		UIKit.UIButton cancelButton { get; set; }

		[Outlet]
		UIKit.UIView containerView { get; set; }

		[Outlet]
		UIKit.UIButton copyLinkImageButton { get; set; }

		[Outlet]
		UIKit.UILabel copyLinkTitleLabel { get; set; }

		[Outlet]
		UIKit.UIView separatorView { get; set; }

		[Outlet]
		UIKit.UIButton shareButton { get; set; }

		[Outlet]
		UIKit.UIButton shareInstagramImageButton { get; set; }

		[Outlet]
		UIKit.UILabel shareInstagramTitleLabel { get; set; }

		[Outlet]
		UIKit.UILabel shareTitleLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (shareTitleLabel != null) {
				shareTitleLabel.Dispose ();
				shareTitleLabel = null;
			}

			if (shareButton != null) {
				shareButton.Dispose ();
				shareButton = null;
			}

			if (copyLinkImageButton != null) {
				copyLinkImageButton.Dispose ();
				copyLinkImageButton = null;
			}

			if (copyLinkTitleLabel != null) {
				copyLinkTitleLabel.Dispose ();
				copyLinkTitleLabel = null;
			}

			if (shareInstagramImageButton != null) {
				shareInstagramImageButton.Dispose ();
				shareInstagramImageButton = null;
			}

			if (shareInstagramTitleLabel != null) {
				shareInstagramTitleLabel.Dispose ();
				shareInstagramTitleLabel = null;
			}

			if (separatorView != null) {
				separatorView.Dispose ();
				separatorView = null;
			}

			if (cancelButton != null) {
				cancelButton.Dispose ();
				cancelButton = null;
			}

			if (containerView != null) {
				containerView.Dispose ();
				containerView = null;
			}
		}
	}
}
