using System;
using System.Collections.Generic;
using System.Linq;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.ViewModels.Onboarding;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Onboarding
{
    [MvxRootPresentation(WrapInNavigationController = false)]
    public partial class OnboardingView : BaseView<OnboardingViewModel>
    {
        private readonly List<PageIndicatorView> _pageIndicatorViews = new List<PageIndicatorView>();

        private OnboardingCollectionViewSource _source;

        private int _count;
        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                CountChanged(_count);
            }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                _selectedIndex = value;

                if (_selectedIndex != _source.PageIndex)
                {
                    var indexPath = NSIndexPath.FromRowSection(_selectedIndex, 0);
                    collectionView.ScrollToItem(indexPath, UICollectionViewScrollPosition.CenteredHorizontally, true);
                }

                for (var i = 0; i < _pageIndicatorViews.Count; i++)
                {
                    var indicator = _pageIndicatorViews[i].Indicator;

                    if (i == _selectedIndex)
                    {
                        var gradientLayer = new CAGradientLayer()
                        {
                            Frame = new CGRect(0f, 0f, 16f, 16f),
                            CornerRadius = indicator.Layer.CornerRadius,
                            StartPoint = new CGPoint(0f, 1f),
                            EndPoint = new CGPoint(1f, 1f),
                            Colors = new CGColor[]
                            {
                                Theme.Color.CompetitionPhaseVotingSecondary.CGColor,
                                Theme.Color.CompetitionPhaseVotingPrimary.CGColor
                            }
                        };

                        indicator.Layer.AddSublayer(gradientLayer);
                    }
                    else
                    {
                        indicator.Layer.Sublayers?.ForEach(x => x.RemoveFromSuperLayer());
                        indicator.BackgroundColor = i > _selectedIndex
                            ? UIColor.Clear
                            : Theme.Color.CompetitionPhaseNewPrimary;
                    }
                }
            }
        }

        protected override void SetupControls()
        {
            base.SetupControls();

            InitializeCollectionView();
            actionButton.SetDarkStyle(string.Empty);
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = this.CreateBindingSet<OnboardingView, OnboardingViewModel>();

            bindingSet.Bind(this).For(v => v.Count).To(vm => vm.ItemsCount);
            bindingSet.Bind(this).For(v => v.SelectedIndex).To(vm => vm.SelectedIndex);
            bindingSet.Bind(actionButton).For(v => v.BindTitle()).To(vm => vm.ActionTitle);
            bindingSet.Bind(actionButton).For(v => v.BindTouchUpInside()).To(vm => vm.ActionCommand);
            bindingSet.Bind(_source).For(p => p.PageIndex).To(vm => vm.SelectedIndex).TwoWay();
            bindingSet.Bind(_source).For(v => v.ItemsSource).To(vm => vm.Items);
        }

        private void InitializeCollectionView()
        {
            _source = new OnboardingCollectionViewSource(collectionView);
            collectionView.Source = _source;
        }

        private void CountChanged(int count)
        {
            pageIndicatorStackView.ArrangedSubviews.ForEach(x => x.RemoveFromSuperview());
            _pageIndicatorViews.Clear();

            for (var i = 0; i < count; i++)
            {
                var pageIndicatorView = new PageIndicatorView();
                _pageIndicatorViews.Add(pageIndicatorView);

                pageIndicatorStackView.AddArrangedSubview(pageIndicatorView);
            }
        }

        private class PageIndicatorView : UIView
        {
            public PageIndicatorView()
            {
                Initialize();
            }

            protected PageIndicatorView(IntPtr handle)
                : base(handle)
            {
            }

            public UIView Indicator { get; private set; }

            private void Initialize()
            {
                TranslatesAutoresizingMaskIntoConstraints = false;
                BackgroundColor = Theme.Color.Cobalt;
                Layer.BorderWidth = 1f;
                Layer.BorderColor = Theme.Color.Violet.CGColor;
                Layer.CornerRadius = 10f;

                Indicator = new UIView();
                Indicator.TranslatesAutoresizingMaskIntoConstraints = false;
                Indicator.BackgroundColor = UIColor.Clear;
                Indicator.Layer.CornerRadius = 8f;

                AddSubview(Indicator);

                NSLayoutConstraint.ActivateConstraints(new[]
                {
                    HeightAnchor.ConstraintEqualTo(20f),
                    WidthAnchor.ConstraintEqualTo(20f),

                    Indicator.CenterXAnchor.ConstraintEqualTo(CenterXAnchor),
                    Indicator.CenterYAnchor.ConstraintEqualTo(CenterYAnchor),
                    Indicator.HeightAnchor.ConstraintEqualTo(16f),
                    Indicator.WidthAnchor.ConstraintEqualTo(16f)
                });
            }
        }
    }
}
