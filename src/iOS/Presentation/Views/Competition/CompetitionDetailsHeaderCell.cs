using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Converters;
using PrankChat.Mobile.iOS.Presentation.Views.Base;

namespace PrankChat.Mobile.iOS.Presentation.Views.Competition
{
    public partial class CompetitionDetailsHeaderCell : BaseTableCell<CompetitionDetailsHeaderCell, CompetitionDetailsHeaderViewModel>
    {
        protected CompetitionDetailsHeaderCell(IntPtr handle)
            : base(handle)
        {
        }

        protected override void SetupControls()
        {
            base.SetupControls();

            prizeTitleLabel.Text = Resources.Competitions_Prize_Pool;

            uploadVideoButton.SetDarkStyle(Resources.Competition_Load_Video);
            openRulesButton.SetBorderlessStyle(Resources.Competition_Rules, Theme.Color.White, Theme.Color.White);
            openPrizePoolButton.SetBorderlessStyle(Resources.Competition_Results, Theme.Color.White, Theme.Color.White);
        }

        protected override void SetBindings()
        {
            base.SetBindings();

            var bindingSet = this.CreateBindingSet<CompetitionDetailsHeaderCell, CompetitionDetailsHeaderViewModel>();

            bindingSet.Bind(backgroundImageView)
                      .For(v => v.ImagePath)
                      .To(vm => vm.ImageUrl);

            bindingSet.Bind(titleLabel)
                      .For(v => v.Text)
                      .To(vm => vm.Title);

            bindingSet.Bind(termLabel)
                      .For(v => v.Text)
                      .To(vm => vm.Phase)
                      .WithConversion<CompetitionPhaseToTermTitleConverter>();

            bindingSet.Bind(idLabel)
                      .For(v => v.Text)
                      .To(vm => vm.Number);

            bindingSet.Bind(idLabel)
                      .For(v => v.Hidden)
                      .To(vm => vm.Phase)
                      .WithConversion<CompetitionPhaseToHiddenConverter>();

            bindingSet.Bind(durationLabel)
                      .For(v => v.Text)
                      .To(vm => vm.Duration);

            bindingSet.Bind(durationLabel)
                      .For(v => v.Hidden)
                      .To(vm => vm.IsFinished);

            bindingSet.Bind(timeContainer)
                      .For(v => v.BindVisible())
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

            bindingSet.Bind(prizeLabel)
                      .For(v => v.Text)
                      .To(vm => vm.PrizePoolPresentation);

            bindingSet.Bind(descriptionLabel)
                      .For(v => v.Text)
                      .To(vm => vm.Description);

            bindingSet.Bind(idLabel)
                      .For(v => v.Text)
                      .To(vm => vm.Number);

            bindingSet.Bind(idLabel)
                      .For(v => v.Hidden)
                      .To(vm => vm.Phase)
                      .WithConversion<CompetitionPhaseToHiddenConverter>();

            bindingSet.Bind(likeButton)
                      .For(v => v.BindTitle())
                      .To(vm => vm.LikesCountString);

            // TODO: bind like command
            //bindingSet.Bind(likeButton)
            //          .For(v => v.BindTouchUpInside())
            //          .To(vm => vm.LikeCommand);

            bindingSet.Bind(likeButton)
                      .For(v => v.Hidden)
                      .To(vm => vm.Phase)
                      .WithConversion<CompetitionPhaseToHiddenConverter>();

            bindingSet.Bind(uploadVideoButton)
                      .For(v => v.BindTouchUpInside())
                      .To(vm => vm.LoadVideoCommand);

            bindingSet.Bind(uploadVideoButton)
                      .For(v => v.BindVisible())
                      .To(vm => vm.CanLoadVideo);

            bindingSet.Bind(openRulesButton)
                      .For(v => v.BindTouchUpInside())
                      .To(vm => vm.OpenRulesCommand);

            bindingSet.Bind(openPrizePoolButton)
                      .For(v => v.BindTouchUpInside())
                      .To(vm => vm.OpenPrizePoolCommand);

            bindingSet.Apply();
        }
    }
}
