// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PrankChat.Mobile.iOS.Views.Subscriptions.Items
{
	[Register ("SubscriptionItemCell")]
	partial class SubscriptionItemCell
	{
		[Outlet]
		PrankChat.Mobile.iOS.Controls.CircleCachedImageView AvatarImageView { get; set; }

		[Outlet]
		UIKit.UILabel DescriptionLabel { get; set; }

		[Outlet]
		UIKit.UILabel NameLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (DescriptionLabel != null) {
				DescriptionLabel.Dispose ();
				DescriptionLabel = null;
			}

			if (NameLabel != null) {
				NameLabel.Dispose ();
				NameLabel = null;
			}

			if (AvatarImageView != null) {
				AvatarImageView.Dispose ();
				AvatarImageView = null;
			}
		}
	}
}
