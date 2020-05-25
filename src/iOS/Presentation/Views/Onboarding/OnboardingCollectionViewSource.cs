using System;
using CoreGraphics;
using Foundation;
using MvvmCross.Base;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Onboarding
{
    public class OnboardingCollectionViewSource : MvxCollectionViewSource, IUICollectionViewDelegateFlowLayout
    {
        private readonly UICollectionView _collectionView;

        public OnboardingCollectionViewSource(UICollectionView collectionView)
            : base(collectionView, new NSString(OnboardingItemCell.CellId))
        {
            collectionView.RegisterNibForCell(OnboardingItemCell.Nib, OnboardingItemCell.CellId);
            _collectionView = collectionView;
        }

        public event EventHandler PageIndexChanged;

        public int PageIndex { get; set; }

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

        public override void DecelerationEnded(UIScrollView scrollView)
        {
            var currentIndex = (int)(scrollView.ContentOffset.X / _collectionView.Frame.Width);
            PageIndex = currentIndex;
            PageIndexChanged.Raise(this);
        }

        public override void WillEndDragging(UIScrollView scrollView, CGPoint velocity, ref CGPoint targetContentOffset)
        {
            var currentIndex = (int)(_collectionView.ContentOffset.X / _collectionView.Frame.Width);
            PageIndex = currentIndex;
            PageIndexChanged.Raise(this);
        }
    }
}
