using System;
using System.Linq;
using CoreAnimation;
using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Converters;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Competition
{
    public partial class CompetitionItemCell : BaseCollectionCell<CompetitionItemCell, CompetitionItemViewModel>
    {
        private CompetitionPhase _phase;

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

                SetTitleBackground(_phase);

                var color = GetPrimaryColor(_phase);
                likeButton.TintColor = color;
                idLabel.TextColor = color;

                Layer.BorderColor = color.CGColor;
                Layer.ShadowColor = color.CGColor;
                Layer.ShadowOffset = CGSize.Empty;
                Layer.ShadowOpacity = 1f;
                Layer.ShadowRadius = 5f;
                Layer.ShadowPath = UIBezierPath.FromRoundedRect(Bounds, Layer.CornerRadius).CGPath;
                Layer.MasksToBounds = false;
            }
        }

        protected override void SetupControls()
        {
            Layer.CornerRadius = 15f;
            Layer.BorderWidth = 3f;
            titleContainer.Layer.CornerRadius = Layer.CornerRadius;
            titleContainer.BackgroundColor = UIColor.Clear;

            prizeTitleLabel.Text = Resources.Competitions_Prize_Pool;
            button.SetDarkStyle(string.Empty);

            SetNeedsLayout();
            LayoutIfNeeded();
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
                      .To(vm => vm.IsFinished)
                      .WithConversion<MvxInvertedBooleanConverter>();

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
                      .For(v => v.Hidden)
                      .To(vm => vm.IsFinished);

            bindingSet.Bind(termFromLabel)
                      .For(v => v.Text)
                      .To(vm => vm.NewTerm)
                      .WithConversion(StringFormatValueConverter.Name, Constants.Formats.DateTimeFormat);

            bindingSet.Bind(termToLabel)
                      .For(v => v.Text)
                      .To(vm => vm.VoteTerm)
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
                      .To(vm => vm.Id);

            bindingSet.Bind(idLabel)
                      .For(v => v.Hidden)
                      .To(vm => vm.Phase)
                      .WithConversion<CompetitionPhaseToHiddenConverter>();

            bindingSet.Bind(likeButton)
                      .For(v => v.BindTitle())
                      .To(vm => vm.LikesCountString);

            bindingSet.Bind(likeButton)
                      .For(v => v.Hidden)
                      .To(vm => vm.Phase)
                      .WithConversion<CompetitionPhaseToHiddenConverter>();

            bindingSet.Bind(button)
                      .For(v => v.BindTitle())
                      .To(vm => vm.Phase)
                      .WithConversion<CompetitionPhaseToActionButtonTitleConverter>();

            bindingSet.Apply();
        }

        private void SetTitleBackground(CompetitionPhase phase)
        {
            var gradientLayer = new CAGradientLayer
            {
                Frame = titleContainer.Bounds,
                CornerRadius = titleContainer.Layer.CornerRadius,
                MaskedCorners = CACornerMask.MinXMinYCorner | CACornerMask.MaxXMinYCorner,
                Colors = GetGradient(phase),
                StartPoint = new CGPoint(0f, 1f),
                EndPoint = new CGPoint(1f, 1f)
            };

            titleContainer.Layer.Sublayers.OfType<CAGradientLayer>().ForEach(x => x.RemoveFromSuperLayer());
            titleContainer.Layer.InsertSublayer(gradientLayer, 0);
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