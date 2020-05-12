// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PrankChat.Mobile.iOS.Presentation.Views.ArbitrationView
{
	[Register ("RatingItemCell")]
	partial class ArbitrationItemCell
	{
		[Outlet]
		UIKit.UIImageView backgroundImageView { get; set; }

		[Outlet]
		UIKit.UILabel dayLabel { get; set; }

		[Outlet]
		UIKit.UILabel hourLabel { get; set; }

		[Outlet]
		UIKit.UIView innerView { get; set; }

		[Outlet]
		UIKit.UILabel minuteLabel { get; set; }

		[Outlet]
		UIKit.UIButton orderDetailsButton { get; set; }

		[Outlet]
		UIKit.UILabel OrderStatusLabel { get; set; }

		[Outlet]
		UIKit.UILabel orderTimeLabel { get; set; }

		[Outlet]
		UIKit.UILabel orderTitleLabel { get; set; }

		[Outlet]
		UIKit.UILabel priceLable { get; set; }

		[Outlet]
		UIKit.UILabel priceValueLabel { get; set; }

		[Outlet]
		PrankChat.Mobile.iOS.Controls.CircleCachedImageView profilePhotoImage { get; set; }

		[Outlet]
		UIKit.UILabel timeLablel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (backgroundImageView != null) {
				backgroundImageView.Dispose ();
				backgroundImageView = null;
			}

			if (dayLabel != null) {
				dayLabel.Dispose ();
				dayLabel = null;
			}

			if (hourLabel != null) {
				hourLabel.Dispose ();
				hourLabel = null;
			}

			if (innerView != null) {
				innerView.Dispose ();
				innerView = null;
			}

			if (minuteLabel != null) {
				minuteLabel.Dispose ();
				minuteLabel = null;
			}

			if (orderDetailsButton != null) {
				orderDetailsButton.Dispose ();
				orderDetailsButton = null;
			}

			if (OrderStatusLabel != null) {
				OrderStatusLabel.Dispose ();
				OrderStatusLabel = null;
			}

			if (orderTimeLabel != null) {
				orderTimeLabel.Dispose ();
				orderTimeLabel = null;
			}

			if (orderTitleLabel != null) {
				orderTitleLabel.Dispose ();
				orderTitleLabel = null;
			}

			if (priceLable != null) {
				priceLable.Dispose ();
				priceLable = null;
			}

			if (priceValueLabel != null) {
				priceValueLabel.Dispose ();
				priceValueLabel = null;
			}

			if (profilePhotoImage != null) {
				profilePhotoImage.Dispose ();
				profilePhotoImage = null;
			}

			if (timeLablel != null) {
				timeLablel.Dispose ();
				timeLablel = null;
			}
		}
	}
}
