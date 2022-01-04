using CoreAnimation;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.ViewModels.Competition.Items;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Converters;
using PrankChat.Mobile.iOS.Views.Base;
using System;
using UIKit;

namespace PrankChat.Mobile.iOS.Views.Competition
{
    public partial class CompetitionItemCell : BaseCollectionCell<CompetitionItemCell, CompetitionItemViewModel>
    {
        private CompetitionPhase _phase;
        private CAGradientLayer _titleGradientSublayer;
        private CAShapeLayer _dashedBorderLayer;
        private CAShapeLayer _defaultBorderLayer;

        protected CompetitionItemCell(IntPtr handle)
            : base(handle)
        {
        }

        public CompetitionPhase Phase
        {
            get => _phase;
            set
            {
                _phase = value;
                PhaseChanged(_phase);
            }
        }

        protected override void SetupControls()
        {
            InitializeLayer();
            InitializeTitleContainer();

            prizeTitleLabel.Text = Resources.TournamentPrizePool;
            button.SetDarkStyle(string.Empty);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            _titleGradientSublayer.Frame = titleContainer.Bounds;
            _defaultBorderLayer.Frame = Bounds;
            _dashedBorderLayer.Frame = Bounds;
            _dashedBorderLayer.Path = UIBezierPath.FromRoundedRect(new CGRect(1, 1, Bounds.Width - 2, Bounds.Height - 2), Layer.CornerRadius).CGPath;
            Layer.ShadowPath = UIBezierPath.FromRoundedRect(Bounds, Layer.CornerRadius).CGPath;
        }

        protected override void SetBindings()
        {
            base.SetBindings();

            using var bindingSet = this.CreateBindingSet<CompetitionItemCell, CompetitionItemViewModel>();

            bindingSet.Bind(this).For(v => v.Phase).To(vm => vm.Phase);
            bindingSet.Bind(titleLabel).For(v => v.Text).To(vm => vm.Title);
            bindingSet.Bind(imageView).For(v => v.ImagePath).To(vm => vm.ImageUrl);
            bindingSet.Bind(descriptionLabel).For(v => v.Text).To(vm => vm.Description);
            bindingSet.Bind(termLabel).For(v => v.Text).To(vm => vm.Phase)
                      .WithConversion<CompetitionPhaseToTermTitleConverter>();
            bindingSet.Bind(timeContainer).For(v => v.Hidden).To(vm => vm.IsFinished);
            bindingSet.Bind(timeLabel).For(v => v.Text).To(vm => vm.NextPhaseCountdown)
                      .WithConversion(StringFormatValueConverter.Name, Constants.Formats.DateWithSpace);
            bindingSet.Bind(daysLabel).For(v => v.Text).To(vm => vm.DaysText);
            bindingSet.Bind(hoursLabel).For(v => v.Text).To(vm => vm.HoursText);
            bindingSet.Bind(minutesLabel).For(v => v.Text).To(vm => vm.MinutesText);
            bindingSet.Bind(termContainer).For(v => v.BindVisible()).To(vm => vm.IsFinished);
            bindingSet.Bind(termFromLabel).For(v => v.Text).To(vm => vm.CreatedAt)
                      .WithConversion(StringFormatValueConverter.Name, Constants.Formats.DateTimeFormat);
            bindingSet.Bind(termToLabel).For(v => v.Text).To(vm => vm.ActiveTo)
                      .WithConversion(StringFormatValueConverter.Name, Constants.Formats.DateTimeFormat);
            bindingSet.Bind(prizeLabel).For(v => v.Text).To(vm => vm.PrizePoolPresentation);
            bindingSet.Bind(prizeBottomSeparator).For(v => v.Hidden).To(vm => vm.Phase)
                      .WithConversion<CompetitionPhaseToHiddenConverter>();
            bindingSet.Bind(idLabel).For(v => v.Text).To(vm => vm.Number);
            bindingSet.Bind(idLabel).For(v => v.Hidden).To(vm => vm.CanExecuteActionVideo);
            bindingSet.Bind(likeButton).For(v => v.BindTitle()).To(vm => vm.LikesCountString);
            bindingSet.Bind(likeButton).For(v => v.Hidden).To(vm => vm.CanExecuteActionVideo);
            bindingSet.Bind(PrivateFlagImageView).For(v => v.BindVisible()).To(vm => vm.Category)
                      .WithConversion(new DelegateConverter<OrderCategory, bool>((category) => category == OrderCategory.PrivatePaidCompetition));
            bindingSet.Bind(PaidFlagImageView).For(v => v.BindVisible()).To(vm => vm.Category)
                      .WithConversion(new DelegateConverter<OrderCategory, bool>((category) => category == OrderCategory.PaidCompetition || category == OrderCategory.PrivatePaidCompetition));
            bindingSet.Bind(button).For(v => v.BindTitle()).To(vm => vm.ActionButtonTitle);
            bindingSet.Bind(button).For(v => v.BindTouchUpInside()).To(vm => vm.ActionCommand);
            bindingSet.Bind(CustomerAvatarImageView).For(v => v.ImagePath).To(vm => vm.CustomerAvatarUrl).OneWay();
            bindingSet.Bind(CustomerAvatarImageView).For(v => v.BindVisible()).To(vm => vm.IsCustomerAttached);
            bindingSet.Bind(OnModerationView).For(v => v.BindVisible()).To(vm => vm.Phase)
                .WithConversion(new DelegateConverter<CompetitionPhase, bool>(phase => phase == CompetitionPhase.Moderation));
        }

        private void InitializeLayer()
        {
            _dashedBorderLayer = new CAShapeLayer
            {
                CornerRadius = 15f,
                StrokeColor = UIColor.FromRGB(134, 134, 134).CGColor,
                FillColor = null,
                ShouldRasterize = true,
                LineWidth = 2f,
                LineDashPattern = new[] { new NSNumber(10), new NSNumber(6) }
            };

            _defaultBorderLayer = new CAShapeLayer()
            {
                CornerRadius = 15f,
                BorderWidth = 3f
            };

            OverlayView.Layer.InsertSublayer(_defaultBorderLayer, 0);
            Layer.CornerRadius = 15f;
        }

        private void InitializeTitleContainer()
        {
            titleContainer.Layer.CornerRadius = Layer.CornerRadius;
            titleContainer.BackgroundColor = UIColor.Clear;

            _titleGradientSublayer = new CAGradientLayer
            {
                CornerRadius = titleContainer.Layer.CornerRadius,
                MaskedCorners = CACornerMask.MinXMinYCorner | CACornerMask.MaxXMinYCorner,
                StartPoint = new CGPoint(0f, 1f),
                EndPoint = new CGPoint(1f, 1f)
            };

            titleContainer.Layer.InsertSublayer(_titleGradientSublayer, 0);
        }

        private void PhaseChanged(CompetitionPhase phase)
        {
            var color = GetPrimaryColor(phase);
            idLabel.TextColor = color;

            _defaultBorderLayer.BorderColor = color.CGColor;
            Layer.ShadowColor = color.CGColor;

            _titleGradientSublayer.Colors = GetGradient(_phase);

            if (phase == CompetitionPhase.Moderation)
            {
                if (_dashedBorderLayer.SuperLayer == null)
                {
                    Layer.AddSublayer(_dashedBorderLayer);
                }

                Layer.ShadowOpacity = 0f;
            }
            else
            {
                _dashedBorderLayer.RemoveFromSuperLayer();
                Layer.ShadowOffset = CGSize.Empty;
                Layer.ShadowOpacity = 1f;
                Layer.ShadowRadius = 5f;
                Layer.MasksToBounds = false;
                Layer.RasterizationScale = UIScreen.MainScreen.Scale;
                Layer.ShouldRasterize = true;
            }
        }

        private UIColor GetPrimaryColor(CompetitionPhase phase) => phase switch
        {
            CompetitionPhase.New => Theme.Color.CompetitionPhaseNewPrimary,
            CompetitionPhase.Voting => Theme.Color.CompetitionPhaseVotingPrimary,
            CompetitionPhase.Finished => Theme.Color.CompetitionPhaseFinishedPrimary,
            CompetitionPhase.Moderation => UIColor.White,
            _ => throw new ArgumentOutOfRangeException(),
        };

        private CGColor[] GetGradient(CompetitionPhase phase)
        {
            switch (phase)
            {
                case CompetitionPhase.New:
                    return new[]
                    {
                        Theme.Color.CompetitionPhaseNewSecondary.CGColor,
                        Theme.Color.CompetitionPhaseNewPrimary.CGColor
                    };

                case CompetitionPhase.Voting:
                    return new[]
                    {
                        Theme.Color.CompetitionPhaseVotingSecondary.CGColor,
                        Theme.Color.CompetitionPhaseVotingPrimary.CGColor
                    };

                case CompetitionPhase.Finished:
                    return new[]
                    {
                        Theme.Color.CompetitionPhaseFinishedSecondary.CGColor,
                        Theme.Color.CompetitionPhaseFinishedPrimary.CGColor
                    };

                case CompetitionPhase.Moderation:
                    return new[]
                    {
                        Theme.Color.CompetitionPhaseModeration.CGColor,
                        Theme.Color.CompetitionPhaseModeration.CGColor
                    };

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}