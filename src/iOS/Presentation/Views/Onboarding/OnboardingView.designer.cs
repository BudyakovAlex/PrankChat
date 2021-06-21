// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PrankChat.Mobile.iOS.Presentation.Views.Onboarding
{
	[Register ("OnboardingView")]
	partial class OnboardingView
   
	{
		[Outlet]
		UIKit.UIButton actionButton { get; set; }

		[Outlet]
		UIKit.UICollectionView collectionView { get; set; }

		[Outlet]
		UIKit.UIStackView pageIndicatorStackView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (collectionView != null) {
				collectionView.Dispose ();
				collectionView = null;
			}

			if (actionButton != null) {
				actionButton.Dispose ();
				actionButton = null;
			}

			if (pageIndicatorStackView != null) {
				pageIndicatorStackView.Dispose ();
				pageIndicatorStackView = null;
			}
		}
	}
}
