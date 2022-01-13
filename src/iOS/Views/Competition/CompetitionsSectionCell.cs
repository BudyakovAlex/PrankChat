using System;
using System.Linq;
using CoreFoundation;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.ViewModels.Competitions;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Views.Competition
{
    public partial class CompetitionsSectionCell : BaseTableCell<CompetitionsSectionCell, CompetitionsSectionViewModel>
    {
        private static readonly nfloat CollectionViewPadding = 20f;
        private static readonly nfloat EnabledButtonAlpha = 1f;
        private static readonly nfloat DisabledButtonAlpha = 0.3f;

        private CompetitionItemCollectionViewSource _source;
        private CompetitionPhase _phase;
        private NSIndexPath _currentItemIndexPath = NSIndexPath.FromRowSection(0, 0);

        protected CompetitionsSectionCell(IntPtr handle)
            : base(handle)
        {
        }

        public CompetitionPhase Phase
        {
            get => _phase;
            set
            {
                _phase = value;

                var backgroundColor = GetBackgroundColor(_phase);
                CustomizeTitleView(leftTitleView, backgroundColor);
                CustomizeTitleView(rightTitleView, backgroundColor);
            }
        }

        private UIColor GetBackgroundColor(CompetitionPhase phase) => phase switch
        {
            CompetitionPhase.New => Theme.Color.CompetitionPhaseNewPrimary,
            CompetitionPhase.Voting => Theme.Color.CompetitionPhaseVotingPrimary,
            CompetitionPhase.Finished => Theme.Color.CompetitionPhaseFinishedPrimary,
            CompetitionPhase.Moderation => Theme.Color.CompetitionPhaseModeration,
            _ => throw new ArgumentOutOfRangeException(),
        };
        
        protected override void SetupControls()
        {
            base.SetupControls();

            _source = new CompetitionItemCollectionViewSource(collectionView)
            {
                OnDecelerationEnded = OnDecelerationEnded,
                OnScrolled = OnScrolled,
                OnDraggingEnded = OnDraggingEnded
            };

            collectionView.Source = _source;
            collectionView.ContentInset = new UIEdgeInsets(5f, CollectionViewPadding, 5f, CollectionViewPadding);

            backButton.Alpha = DisabledButtonAlpha;
            backButton.AddGestureRecognizer(new UITapGestureRecognizer(OnBackButtonTap));
            forthButton.AddGestureRecognizer(new UITapGestureRecognizer(OnForthButtonTap));
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = this.CreateBindingSet<CompetitionsSectionCell, CompetitionsSectionViewModel>();

            bindingSet.Bind(this).For(v => v.Phase).To(vm => vm.Phase);
            bindingSet.Bind(titleLabel).For(v => v.Text).To(vm => vm.Phase)
                      .WithConversion<CompetitionPhaseToSectionTitleConverter>();
            bindingSet.Bind(_source).For(v => v.ItemsSource).To(vm => vm.Items);
            bindingSet.Bind(buttonContainerZeroHeightConstraint).For(v => v.Active).To(vm => vm.HasNavigationControls)
                      .WithConversion<MvxInvertedBooleanConverter>();
        }

        private void CustomizeTitleView(UIView view, UIColor color)
        {
            view.BackgroundColor = color;
            view.Layer.CornerRadius = 2f;

            view.Layer.BorderColor = color.CGColor;
            view.Layer.ShadowColor = color.CGColor;
            view.Layer.ShadowOffset = CGSize.Empty;
            view.Layer.ShadowOpacity = 1f;
            view.Layer.ShadowRadius = 5f;

            DispatchQueue.MainQueue.DispatchAsync(() =>
            {
                view.Layer.ShadowPath = UIBezierPath.FromRoundedRect(view.Bounds, view.Layer.CornerRadius).CGPath;
            });
        }

        private void OnBackButtonTap()
        {
            if (_currentItemIndexPath.Row > 0)
            {
                var indexPath = NSIndexPath.FromItemSection(_currentItemIndexPath.Row - 1, _currentItemIndexPath.Section);
                collectionView.ScrollToItem(indexPath, UICollectionViewScrollPosition.CenteredHorizontally, true);
            }
        }

        private void OnForthButtonTap()
        {
            var newRow = _currentItemIndexPath.Row + 1;
            if (newRow < ViewModel.Items.Count)
            {
                var indexPath = NSIndexPath.FromItemSection(newRow, _currentItemIndexPath.Section);
                collectionView.ScrollToItem(indexPath, UICollectionViewScrollPosition.CenteredHorizontally, true);
            }
        }

        private void OnScrolled(UIScrollView _)
        {
            var indexPath = GetCenterItemIndexPath();
            if (indexPath != null)
            {
                _currentItemIndexPath = indexPath;
                if (_currentItemIndexPath.Row == 0)
                {
                    ChangeButtonState(backButton, false);
                    ChangeButtonState(forthButton, true);
                }
                else if (_currentItemIndexPath.Row == ViewModel.Items.Count - 1)
                {
                    ChangeButtonState(backButton, true);
                    ChangeButtonState(forthButton, false);
                }
                else
                {
                    ChangeButtonState(backButton, true);
                    ChangeButtonState(forthButton, true);
                }
            }
        }

        private void OnDecelerationEnded(UIScrollView _) => ScrollToCenterItem();

        private void OnDraggingEnded(UIScrollView _, bool willDecelerate)
        {
            if (!willDecelerate)
            {
                ScrollToCenterItem();
            }
        }

        private void ScrollToCenterItem()
        {
            var indexPath = GetCenterItemIndexPath();
            if (indexPath != null)
            {
                collectionView.ScrollToItem(indexPath, UICollectionViewScrollPosition.CenteredHorizontally, true);
            }
        }

        private void ChangeButtonState(UIButton button, bool isEnabled)
        {
            button.Enabled = isEnabled;
            button.Alpha = isEnabled ? EnabledButtonAlpha : DisabledButtonAlpha;
        }

        private NSIndexPath GetCenterItemIndexPath()
        {
            var visibleCells = collectionView.VisibleCells;
            if (visibleCells.Length == 0)
            {
                return null;
            }

            var cell = collectionView.VisibleCells.FirstOrDefault();
            if (visibleCells.Length == 1)
            {
                return collectionView.IndexPathForCell(cell);
            }

            var x = collectionView.ContentOffset.X + CollectionViewPadding + (cell.Bounds.Width / 2f);
            var indexPath = collectionView.IndexPathForItemAtPoint(new CGPoint(x, 0f));
            if (indexPath != null)
            {
                return indexPath;
            }

            return collectionView.IndexPathForCell(cell);
        }
    }
}
