using System;
using Foundation;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.ProfileView.Cashbox
{
    public class PaymentMethodsCollectionViewSource : MvxCollectionViewSource
    {
        public PaymentMethodsCollectionViewSource(UICollectionView collectionView, NSString defaultCellIdentifier) : base(collectionView, defaultCellIdentifier)
        {
        }

        protected override UICollectionViewCell GetOrCreateCellFor(UICollectionView collectionView, NSIndexPath indexPath, object item)
        {
            var cell = base.GetOrCreateCellFor(collectionView, indexPath, item);
            cell.ContentView.AddGestureRecognizer(
                new UITapGestureRecognizer(() =>
                {
                    collectionView.SelectItem(indexPath, true, UICollectionViewScrollPosition.None);
                    ItemSelected(collectionView, indexPath);
                }));
            return cell;
        }

        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            base.ItemSelected(collectionView, indexPath);
        }
    }
}
