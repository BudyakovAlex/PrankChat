// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PrankChat.Mobile.iOS.Presentation.Views.RatingView
{
	[Register ("RatingItemCell")]
	partial class RatingItemCell
	{
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
		UIKit.UILabel orderTimeLabel { get; set; }

		[Outlet]
		UIKit.UILabel orderTitleLabel { get; set; }

		[Outlet]
		UIKit.UILabel priceLable { get; set; }

		[Outlet]
		UIKit.UILabel priceValueLabel { get; set; }

		[Outlet]
		FFImageLoading.Cross.MvxCachedImageView profilePhotoImage { get; set; }

		[Outlet]
		UIKit.UIButton thumbsDownButton { get; set; }

		[Outlet]
		UIKit.UILabel thumbsDownValueLabel { get; set; }

		[Outlet]
		UIKit.UIButton thumbsUpButton { get; set; }

		[Outlet]
		UIKit.UILabel thumbsUpValueLabel { get; set; }

		[Outlet]
		UIKit.UILabel timeLablel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
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

			if (thumbsUpButton != null) {
				thumbsUpButton.Dispose ();
				thumbsUpButton = null;
			}

			if (thumbsUpValueLabel != null) {
				thumbsUpValueLabel.Dispose ();
				thumbsUpValueLabel = null;
			}

			if (thumbsDownButton != null) {
				thumbsDownButton.Dispose ();
				thumbsDownButton = null;
			}

			if (thumbsDownValueLabel != null) {
				thumbsDownValueLabel.Dispose ();
				thumbsDownValueLabel = null;
			}
		}
	}
}