using System;
using CoreGraphics;
using Foundation;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Onboarding
{
    public class OnboardingCollectionViewSource : MvxCollectionViewSource, IUICollectionViewDelegateFlowLayout
    {
        public OnboardingCollectionViewSource(UICollectionView collectionView)
            : base(collectionView, new NSString(OnboardingItemCell.CellId))
        {
            collectionView.RegisterNibForCell(OnboardingItemCell.Nib, OnboardingItemCell.CellId);
        }

        [Export("collectionView:layout:sizeForItemAtIndexPath:")]
        public CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            return new CGSize(collectionView.Bounds.Size);
        }

        [Export("collectionView:layout:minimumInteritemSpacingForSectionAtIndex:")]
        public nfloat GetMinimumInteritemSpacingForSection(UICollectionView collectionView, UICollectionViewLayout layout, nint section)
        {
            return 0f;
        }

        [Export("collectionView:layout:minimumLineSpacingForSectionAtIndex:")]
        public nfloat GetMinimumLineSpacingForSection(UICollectionView collectionView, UICollectionViewLayout layout, nint section)
        {
            return 0f;
        }
    }
}
