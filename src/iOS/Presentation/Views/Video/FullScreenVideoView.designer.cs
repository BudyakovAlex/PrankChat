// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PrankChat.Mobile.iOS.Presentation.Views.Video
{
	[Register ("FullScreenVideoView")]
	partial class FullScreenVideoView
	{
		[Outlet]
		UIKit.UIButton closeButton { get; set; }

		[Outlet]
		UIKit.UILabel descriptionLabel { get; set; }

		[Outlet]
		UIKit.UIView loadProgressView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint loadProgressViewWidthConstraint { get; set; }

		[Outlet]
		UIKit.UIButton muteButton { get; set; }

		[Outlet]
		UIKit.UIView overlayView { get; set; }

		[Outlet]
		UIKit.UIButton playButton { get; set; }

		[Outlet]
		UIKit.UIView progressView { get; set; }

		[Outlet]
		UIKit.UILabel timeLabel { get; set; }

		[Outlet]
		UIKit.UILabel titleLabel { get; set; }

		[Outlet]
		UIKit.UIView watchProgressControl { get; set; }

		[Outlet]
		UIKit.UIView watchProgressControlContainer { get; set; }

		[Outlet]
		UIKit.UIView watchProgressView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint watchProgressViewWidthConstraint { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (watchProgressViewWidthConstraint != null) {
				watchProgressViewWidthConstraint.Dispose ();
				watchProgressViewWidthConstraint = null;
			}

			if (loadProgressViewWidthConstraint != null) {
				loadProgressViewWidthConstraint.Dispose ();
				loadProgressViewWidthConstraint = null;
			}

			if (closeButton != null) {
				closeButton.Dispose ();
				closeButton = null;
			}

			if (descriptionLabel != null) {
				descriptionLabel.Dispose ();
				descriptionLabel = null;
			}

			if (loadProgressView != null) {
				loadProgressView.Dispose ();
				loadProgressView = null;
			}

			if (muteButton != null) {
				muteButton.Dispose ();
				muteButton = null;
			}

			if (overlayView != null) {
				overlayView.Dispose ();
				overlayView = null;
			}

			if (playButton != null) {
				playButton.Dispose ();
				playButton = null;
			}

			if (progressView != null) {
				progressView.Dispose ();
				progressView = null;
			}

			if (timeLabel != null) {
				timeLabel.Dispose ();
				timeLabel = null;
			}

			if (titleLabel != null) {
				titleLabel.Dispose ();
				titleLabel = null;
			}

			if (watchProgressControl != null) {
				watchProgressControl.Dispose ();
				watchProgressControl = null;
			}

			if (watchProgressControlContainer != null) {
				watchProgressControlContainer.Dispose ();
				watchProgressControlContainer = null;
			}

			if (watchProgressView != null) {
				watchProgressView.Dispose ();
				watchProgressView = null;
			}
		}
	}
}
