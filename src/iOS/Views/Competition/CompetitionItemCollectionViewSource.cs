using System;
using CoreGraphics;
using Foundation;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace PrankChat.Mobile.iOS.Views.Competition
{
    public class CompetitionItemCollectionViewSource : MvxCollectionViewSource, IUICollectionViewDelegateFlowLayout
    {
        public CompetitionItemCollectionViewSource(UICollectionView collectionView)
            : base(collectionView)
        {
            collectionView.RegisterNibForCell(CompetitionItemCell.Nib, CompetitionItemCell.CellId);
        }

        public Action<UIScrollView> OnScrolled { get; set; }

        public Action<UIScrollView> OnDecelerationEnded { get; set; }

        public Action<UIScrollView, bool> OnDraggingEnded { get; set; }

        protected override UICollectionViewCell GetOrCreateCellFor(UICollectionView collectionView, NSIndexPath indexPath, object item)
        {
            return (UICollectionViewCell)collectionView.DequeueReusableCell(CompetitionItemCell.CellId, indexPath);
        }

        public override void Scrolled(UIScrollView scrollView)
        {
            OnScrolled?.Invoke(scrollView);
        }

        public override void DraggingEnded(UIScrollView scrollView, bool willDecelerate)
        {
            OnDraggingEnded?.Invoke(scrollView, willDecelerate);
        }

        public override void DecelerationEnded(UIScrollView scrollView)
        {
            OnDecelerationEnded?.Invoke(scrollView);
        }

        [Export("collectionView:layout:sizeForItemAtIndexPath:")]
        public CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            return new CGSize(UIScreen.MainScreen.Bounds.Width - 40f, collectionView.Frame.Height - 10f);
        }

        [Export("collectionView:layout:minimumLineSpacingForSectionAtIndex:")]
        public nfloat GetMinimumLineSpacingForSection(UICollectionView collectionView, UICollectionViewLayout layout, nint section)
        {
            return 6f;
        }

        [Export("collectionView:layout:minimumInteritemSpacingForSectionAtIndex:")]
        public nfloat GetMinimumInteritemSpacingForSection(UICollectionView collectionView, UICollectionViewLayout layout, nint section)
        {
            return 5f;
        }
    }
}
