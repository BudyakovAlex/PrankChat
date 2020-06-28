using System;
using System.ComponentModel;
using Foundation;
using MvvmCross.Commands;
using PrankChat.Mobile.iOS.AppTheme;
using UIKit;

namespace PrankChat.Mobile.iOS.Controls
{
    [Register(nameof(TabView))]
    [DesignTimeVisible(true)]
    public class TabView : UIView
    {
        private const float IndicatorHeight = 2f;

        private UIStackView _stackView;
        private UIView _indicatorView;
        private TabItemView _selectedTab;
        private NSLayoutConstraint _indicatorViewLeadingConstraint;
        private NSLayoutConstraint _indicatorViewWidthConstraint;

        public TabView()
        {
            Initialize();
        }

        protected TabView(IntPtr handle)
            : base(handle)
        {
        }

        public TabItemView SelectedTab
        {
            get => _selectedTab;
            set
            {
                if (_selectedTab == value)
                {
                    return;
                }

                if (_selectedTab != null)
                {
                    _selectedTab.IsSelected = false;
                }

                _selectedTab = value;
                _selectedTab.IsSelected = true;

                MoveIndicatorTo(_selectedTab);
            }
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            Initialize();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            if (SelectedTab != null)
            {
                MoveIndicatorTo(SelectedTab);
            }
        }

        public void AddTab(TabItemView tab)
        {
            _stackView.AddArrangedSubview(tab);

            tab.AddGestureRecognizer(new UITapGestureRecognizer(_ =>
            {
                if (tab != SelectedTab)
                {
                    SelectedTab = tab;
                    tab.OnTap();
                }
            }));

            if (SelectedTab == null)
            {
                SelectedTab = tab;
            }

            _indicatorViewWidthConstraint.Constant = Frame.Width / _stackView.ArrangedSubviews.Length;
        }

        public void AddTab(string text, Action action)
        {
            var tab = new TabItemView(text, action);
            AddTab(tab);
        }

        private void Initialize()
        {
            InitializeStackView();
            InitializeSeparator();
            InitializeIndicatorView();
        }

        private void InitializeStackView()
        {
            _stackView = new UIStackView
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                Axis = UILayoutConstraintAxis.Horizontal,
                Alignment = UIStackViewAlignment.Fill,
                Distribution = UIStackViewDistribution.FillEqually
            };

            AddSubview(_stackView);

            NSLayoutConstraint.ActivateConstraints(new[]
            {
                _stackView.LeadingAnchor.ConstraintEqualTo(LeadingAnchor),
                _stackView.TopAnchor.ConstraintEqualTo(TopAnchor),
                _stackView.TrailingAnchor.ConstraintEqualTo(TrailingAnchor),
                _stackView.BottomAnchor.ConstraintEqualTo(BottomAnchor)
            });
        }

        private void InitializeSeparator()
        {
            var separator = new UIView
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                BackgroundColor = Theme.Color.Separator
            };

            AddSubview(separator);

            NSLayoutConstraint.ActivateConstraints(new[]
            {
                separator.LeadingAnchor.ConstraintEqualTo(_stackView.LeadingAnchor),
                separator.TrailingAnchor.ConstraintEqualTo(_stackView.TrailingAnchor),
                separator.BottomAnchor.ConstraintEqualTo(_stackView.BottomAnchor),
                separator.HeightAnchor.ConstraintEqualTo(1f),
            });
        }

        private void InitializeIndicatorView()
        {
            _indicatorView = new UIView();
            _indicatorView.TranslatesAutoresizingMaskIntoConstraints = false;
            _indicatorView.BackgroundColor = Theme.Color.CompetitionPhaseNewPrimary;

            AddSubview(_indicatorView);

            NSLayoutConstraint.ActivateConstraints(new[]
            {
                _indicatorView.BottomAnchor.ConstraintEqualTo(BottomAnchor),
                _indicatorView.HeightAnchor.ConstraintEqualTo(2f),

                _indicatorViewWidthConstraint = _indicatorView.WidthAnchor.ConstraintEqualTo(0f),
                _indicatorViewLeadingConstraint = _indicatorView.LeadingAnchor.ConstraintEqualTo(LeadingAnchor)                
            });
        }

        private void MoveIndicatorTo(UIView view)
        {
            var index = Array.IndexOf(_stackView.ArrangedSubviews, view);
            _indicatorViewLeadingConstraint.Constant = _indicatorViewWidthConstraint.Constant * index;
        }
    }
}
