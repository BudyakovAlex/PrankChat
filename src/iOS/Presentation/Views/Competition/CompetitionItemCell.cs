using CoreAnimation;
using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Converters;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using System;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Competition
{
    public partial class CompetitionItemCell : BaseCollectionCell<CompetitionItemCell, CompetitionItemViewModel>
    {
        private CompetitionPhase _phase;
        private CAGradientLayer _titleGradientSublayer;

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

            prizeTitleLabel.Text = Resources.Competitions_Prize_Pool;
            button.SetDarkStyle(string.Empty);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            _titleGradientSublayer.Frame = titleContainer.Bounds;
            Layer.ShadowPath = UIBezierPath.FromRoundedRect(Bounds, Layer.CornerRadius).CGPath;
        }

        protected override void SetBindings()
        {
            base.SetBindings();

            var bindingSet = this.CreateBindingSet<CompetitionItemCell, CompetitionItemViewModel>();

            bindingSet.Bind(this)
                      .For(v => v.Phase)
                      .To(vm => vm.Phase);

            bindingSet.Bind(titleLabel)
                      .For(v => v.Text)
                      .To(vm => vm.Title);

            bindingSet.Bind(imageView)
                      .For(v => v.ImagePath)
                      .To(vm => vm.ImageUrl);

            bindingSet.Bind(descriptionLabel)
                      .For(v => v.Text)
                      .To(vm => vm.Description);

            bindingSet.Bind(termLabel)
                      .For(v => v.Text)
                      .To(vm => vm.Phase)
                      .WithConversion<CompetitionPhaseToTermTitleConverter>();

            bindingSet.Bind(timeContainer)
                      .For(v => v.Hidden)
                      .To(vm => vm.IsFinished);

            bindingSet.Bind(timeLabel)
                      .For(v => v.Text)
                      .To(vm => vm.NextPhaseCountdown)
                      .WithConversion(StringFormatValueConverter.Name, Constants.Formats.DateWithSpace);

            bindingSet.Bind(daysLabel)
                      .For(v => v.Text)
                      .To(vm => vm.DaysText);

            bindingSet.Bind(hoursLabel)
                      .For(v => v.Text)
                      .To(vm => vm.HoursText);

            bindingSet.Bind(minutesLabel)
                      .For(v => v.Text)
                      .To(vm => vm.MinutesText);

            bindingSet.Bind(termContainer)
                      .For(v => v.BindVisible())
                      .To(vm => vm.IsFinished);

            bindingSet.Bind(termFromLabel)
                      .For(v => v.Text)
                      .To(vm => vm.CreatedAt)
                      .WithConversion(StringFormatValueConverter.Name, Constants.Formats.DateTimeFormat);

            bindingSet.Bind(termToLabel)
                      .For(v => v.Text)
                      .To(vm => vm.ActiveTo)
                      .WithConversion(StringFormatValueConverter.Name, Constants.Formats.DateTimeFormat);

            bindingSet.Bind(prizeLabel)
                      .For(v => v.Text)
                      .To(vm => vm.PrizePoolPresentation);

            bindingSet.Bind(prizeBottomSeparator)
                      .For(v => v.Hidden)
                      .To(vm => vm.Phase)
                      .WithConversion<CompetitionPhaseToHiddenConverter>();

            bindingSet.Bind(idLabel)
                      .For(v => v.Text)
                      .To(vm => vm.Number);

            bindingSet.Bind(idLabel)
                      .For(v => v.Hidden)
                      .To(vm => vm.CanExecuteActionVideo);

            bindingSet.Bind(likeButton)
                      .For(v => v.BindTitle())
                      .To(vm => vm.LikesCountString);

            bindingSet.Bind(likeButton)
                      .For(v => v.Hidden)
                      .To(vm => vm.CanExecuteActionVideo);

            bindingSet.Bind(PrivateFlagImageView)
                      .For(v => v.BindVisible())
                      .To(vm => vm.Category)
                      .WithConversion(new DelegateConverter<OrderCategory, bool>((category) => category == OrderCategory.PrivatePaidCompetition));

            bindingSet.Bind(PaidFlagImageView)
                      .For(v => v.BindVisible())
                      .To(vm => vm.Category)
                      .WithConversion(new DelegateConverter<OrderCategory, bool>((category) => category == OrderCategory.PaidCompetition || category == OrderCategory.PrivatePaidCompetition));

            bindingSet.Bind(button)
                      .For(v => v.BindTitle())
                      .To(vm => vm.Phase)
                      .WithConversion<CompetitionPhaseToActionButtonTitleConverter>();

            bindingSet.Bind(button)
                      .For(v => v.BindTouchUpInside())
                      .To(vm => vm.ActionCommand);

            bindingSet.Apply();
        }

        private void InitializeLayer()
        {
            Layer.CornerRadius = 15f;
            Layer.BorderWidth = 3f;
            Layer.ShadowOffset = CGSize.Empty;
            Layer.ShadowOpacity = 1f;
            Layer.ShadowRadius = 5f;
            Layer.MasksToBounds = false;
            Layer.RasterizationScale = UIScreen.MainScreen.Scale;
            Layer.ShouldRasterize = true;
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

            Layer.BorderColor = color.CGColor;
            Layer.ShadowColor = color.CGColor;

            _titleGradientSublayer.Colors = GetGradient(_phase);
        }

        private UIColor GetPrimaryColor(CompetitionPhase phase)
        {
            switch (phase)
            {
                case CompetitionPhase.New:
                    return Theme.Color.CompetitionPhaseNewPrimary;

                case CompetitionPhase.Voting:
                    return Theme.Color.CompetitionPhaseVotingPrimary;

                case CompetitionPhase.Finished:
                    return Theme.Color.CompetitionPhaseFinishedPrimary;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

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

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}