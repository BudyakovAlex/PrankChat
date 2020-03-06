using System;
using CoreFoundation;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.Plugin.Visibility;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Controls;
using PrankChat.Mobile.iOS.Presentation.Binding;
using PrankChat.Mobile.iOS.Presentation.Converters;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Order
{
    public partial class OrderDetailsView : BaseGradientBarView<OrderDetailsViewModel>
    {
        private MvxUIRefreshControl _refreshControl;

        private ArbitrationValueType? _arbitrationValue;
        public ArbitrationValueType? ArbitrationValue
        {
            get => _arbitrationValue;
            set
            {
                _arbitrationValue = value;
                UpdateStyleArbitrationButtons();
            }
        }

        protected override void SetupBinding()
        {
            var set = this.CreateBindingSet<OrderDetailsView, OrderDetailsViewModel>();

            #region Customer

            set.Bind(profileImageView)
                .For(v => v.DownsampleWidth)
                .To(vm => vm.DownsampleWidth);

            set.Bind(profileImageView)
                .For(v => v.Transformations)
                .To(vm => vm.Transformations);

            set.Bind(profileImageView)
                .For(v => v.ImagePath)
                .WithConversion<PlaceholderImageConverter>()
                .To(vm => vm.ProfilePhotoUrl);

            set.Bind(profileNameLabel)
                .To(vm => vm.ProfileName);

            set.Bind(customerShortNameLabel)
                .To(vm => vm.ProfileShortName);

            set.Bind(customerShortNameLabel)
                .For(v => v.BindHidden())
                .To(vm => vm.ProfilePhotoUrl);

            #endregion Customer

            #region LoadVideo

            set.Bind(downloadButton)
                .To(vm => vm.LoadVideoCommand);

            set.Bind(downloadButton)
                .For(v => v.BindVisible())
                .To(vm => vm.IsVideoLoadAvailable);

            set.Bind(downloadView)
                .For(v => v.BindVisible())
                .To(vm => vm.IsVideoLoadAvailable);

            #endregion LoadVideo

            #region Orders

            set.Bind(videoNameLabel)
                .To(vm => vm.VideoName);

            set.Bind(videoDescriptionLabel)
                .To(vm => vm.VideoDetails);

            set.Bind(priceValueLabel)
                .To(vm => vm.PriceValue);

            set.Bind(daysValueLabel)
                .To(vm => vm.TimeDaysValue);

            set.Bind(hourValueLabel)
                .To(vm => vm.TimeHourValue);

            set.Bind(minutesValueLabel)
                .To(vm => vm.TimeMinutesValue);

            set.Bind(timeTextLabel)
                .For(v => v.BindVisible())
                .To(vm => vm.IsTimeAvailable);

            set.Bind(daysTitleLabel)
                .For(v => v.BindVisible())
                .To(vm => vm.IsTimeAvailable);

            set.Bind(daysValueLabel)
                .For(v => v.BindVisible())
                .To(vm => vm.IsTimeAvailable);

            set.Bind(delimiterTimeOneLabel)
                .For(v => v.BindVisible())
                .To(vm => vm.IsTimeAvailable);

            set.Bind(delimiterTimeTwoLabel)
                .For(v => v.BindVisible())
                .To(vm => vm.IsTimeAvailable);

            set.Bind(hourTitleLabel)
                .For(v => v.BindVisible())
                .To(vm => vm.IsTimeAvailable);

            set.Bind(hourValueLabel)
                .For(v => v.BindVisible())
                .To(vm => vm.IsTimeAvailable);

            set.Bind(minutesTitleLabel)
                .For(v => v.BindVisible())
                .To(vm => vm.IsTimeAvailable);

            set.Bind(minutesValueLabel)
                .For(v => v.BindVisible())
                .To(vm => vm.IsTimeAvailable);

            set.Bind(timeView)
                .For(v => v.BindVisible())
                .To(vm => vm.IsTimeAvailable);

            set.Bind(stackViewToPriceValueLabelConstraint)
                .For(v => v.Active)
                .To(vm => vm.IsTimeAvailable)
                .WithConversion<MvxInvertedBooleanConverter>();

            #endregion Orders

            #region Video

            set.Bind(videoView)
                .For(v => v.BindVisible())
                .To(vm => vm.IsVideoAvailable);

            set.Bind(videoImageView)
                .For(v => v.ImagePath)
                .To(vm => vm.VideoPlaceholderUrl);

            set.Bind(videoImageView)
                .For(v => v.BindTap())
                .To(vm => vm.ShowFullVideoCommand);

            #endregion Video

            #region Subscribe and Unsubscribe

            set.Bind(subscriptionButton)
                .To(vm => vm.SubscribeOrderCommand);

            set.Bind(subscriptionButton)
                .For(v => v.BindVisible())
                .To(vm => vm.IsSubscribeAvailable);

            set.Bind(unsubscriptionButton)
                .To(vm => vm.UnsubscribeOrderCommand);

            set.Bind(unsubscriptionButton)
                .For(v => v.BindVisible())
                .To(vm => vm.IsUnsubscribeAvailable);

            #endregion Subscribe and Unsubscribe

            #region Take order

            set.Bind(takeOrderButton)
                .To(vm => vm.TakeOrderCommand);

            set.Bind(takeOrderButton)
                .For(v => v.BindVisible())
                .To(vm => vm.IsTakeOrderAvailable);

            #endregion Take order

            #region Decide View

            set.Bind(decideView)
                .For(v => v.BindVisible())
                .To(vm => vm.IsDecideVideoAvailable);

            set.Bind(noButton)
                .To(vm => vm.NoCommand);

            set.Bind(noButton)
                .For(v => v.BindTitle())
                .To(vm => vm.NoText);

            set.Bind(yesButton)
                .To(vm => vm.YesCommand);

            set.Bind(yesButton)
                .For(v => v.BindTitle())
                .To(vm => vm.YesText);

            set.Bind(this)
                .For(v => v.ArbitrationValue)
                .To(vm => vm.SelectedArbitration);

            #endregion Decide View

            #region Executor

            set.Bind(executorImageView)
                .For(v => v.DownsampleWidth)
                .To(vm => vm.DownsampleWidth);

            set.Bind(executorImageView)
                .For(v => v.Transformations)
                .To(vm => vm.Transformations);

            set.Bind(executorImageView)
                .For(v => v.ImagePath)
                .WithConversion<PlaceholderImageConverter>()
                .To(vm => vm.ExecutorPhotoUrl);

            set.Bind(executorNameLabel)
                .To(vm => vm.ExecutorName);

            set.Bind(startDateLabel)
                .To(vm => vm.StartOrderDate);

            set.Bind(executorView)
                .For(v => v.BindVisible())
                .To(vm => vm.IsExecutorAvailable);

            set.Bind(executorShortNameLabel)
                .To(vm => vm.ExecutorShortName);

            set.Bind(executorShortNameLabel)
                .For(v => v.BindHidden())
                .To(vm => vm.ExecutorPhotoUrl);

            #endregion Executor

            #region Decision View

            set.Bind(decisionView)
                .For(v => v.BindVisible())
                .To(vm => vm.IsDecisionVideoAvailable);

            set.Bind(acceptButton)
                .To(vm => vm.AcceptOrderCommand);

            set.Bind(arqueButton)
                .To(vm => vm.ArqueOrderCommand);

            #endregion Decision View

            #region Cancel

            set.Bind(cancelVideoButton)
                .To(vm => vm.CancelOrderCommand);

            set.Bind(cancelVideoButton)
                .For(v => v.BindVisible())
                .To(vm => vm.IsCancelOrderAvailable);

            #endregion Cancel

            #region Execute video

            set.Bind(executeVideoButton)
                .To(vm => vm.ExecuteOrderCommand);

            set.Bind(executeVideoButton)
                .For(v => v.BindVisible())
                .To(vm => vm.IsExecuteOrderAvailable);

            #endregion Execute video

            set.Bind(progressBarView)
                .For(v => v.BindVisible())
                .To(vm => vm.IsBusy);

            set.Bind(_refreshControl)
               .For(v => v.IsRefreshing)
               .To(vm => vm.IsBusy);

            set.Bind(_refreshControl)
                .For(v => v.RefreshCommand)
                .To(vm => vm.LoadOrderDetailsCommand);

            set.Apply();
        }

        protected override void SetupControls()
        {
            Title = Resources.OrderDetailsView_Title;

            takeOrderButton.SetDarkStyle(Resources.OrderDetailsView_Take_Order_Button);
            subscriptionButton.SetDarkStyle(Resources.OrderDetailsView_Subscribe_Button);
            unsubscriptionButton.SetDarkStyle(Resources.OrderDetailsView_Unsubscribe_Button);
            executeVideoButton.SetDarkStyle(Resources.OrderDetailsView_Execute_Button);
            acceptButton.SetDarkStyle(Resources.OrderDetailsView_Accept_Button);
            arqueButton.SetBorderlessStyle(Resources.OrderDetailsView_Argue_Button);
            downloadButton.SetDarkStyle(Resources.OrderDetailsView_LoadVideo);
            cancelVideoButton.SetBorderlessStyle(Resources.OrderDetailsView_Cancel_Button, Theme.Color.Accent);

            profileNameLabel.SetTitleStyle();
            videoNameLabel.SetBoldTitleStyle();
            videoDescriptionLabel.SetTitleStyle();
            priceTextLabel.SetSmallTitleStyle(Resources.OrderDetailsView_Price_Text);
            priceValueLabel.SetMediumStyle(26, Theme.Color.Text);
            timeTextLabel.SetSmallTitleStyle(Resources.OrderDetailsView_Time_Text);
            daysValueLabel.SetMediumStyle(26, Theme.Color.Text);
            hourValueLabel.SetMediumStyle(26, Theme.Color.Text);
            minutesValueLabel.SetMediumStyle(26, Theme.Color.Text);
            delimiterTimeOneLabel.SetMediumStyle(26, Theme.Color.Text);
            delimiterTimeTwoLabel.SetMediumStyle(26, Theme.Color.Text);
            daysTitleLabel.SetSmallTitleStyle(Resources.Order_View_Day, 10);
            hourTitleLabel.SetSmallTitleStyle(Resources.Order_View_Hour, 10);
            minutesTitleLabel.SetSmallTitleStyle(Resources.Order_View_Minute, 10);
            downloadVideotextLabel.SetSmallTitleStyle(Resources.OrderDetailsView_Download_Text);
            tookOrderTextLabel.SetSmallTitleStyle(Resources.OrderDetailsView_Took_The_Order_Text);
            executorNameLabel.SetTitleStyle();
            startDateLabel.SetSmallSubtitleStyle();

            lottieAnimationView.SetAnimationNamed("Animations/ripple_animation");
            lottieAnimationView.LoopAnimation = true;
            lottieAnimationView.Play();

            yesButton.ImageEdgeInsets = new UIEdgeInsets(0, 0, 0, 10);
            noButton.ImageEdgeInsets = new UIEdgeInsets(0, 0, 0, 10);
            SetStyleForButton(yesButton, "up");
            SetStyleForButton(noButton, "down");

            UpdateStyleArbitrationButtons();

            videoImageView.SetCornerRadius(5);

            rootScrollView.RefreshControl = _refreshControl = new MvxUIRefreshControl();
        }

        private void SetStyleForButton(UIButton button, string type)
        {

            button.BackgroundColor = Theme.Color.AccentDark;
            button.SetTitleColor(UIColor.White, UIControlState.Selected);
            button.SetTitleColor(Theme.Color.AccentDark, UIControlState.Normal);
            button.SetSelectableImageStyle($"ic_accent_thumbs_{type}", $"ic_thumbs_{type}");
            button.UserInteractionEnabled = true;
        }

        private void UpdateStyleArbitrationButtons()
        {
            if (_arbitrationValue == null)
            {
                yesButton.Selected = true;
                noButton.Selected = true;
                return;
            }

            var selectedButton = _arbitrationValue == ArbitrationValueType.Positive ? yesButton : noButton;
            var deselectedButton = _arbitrationValue == ArbitrationValueType.Positive ? noButton : yesButton;

            selectedButton.Selected = true;
            selectedButton.Layer.BackgroundColor = Theme.Color.AccentDark.CGColor;

            deselectedButton.Selected = false;
            deselectedButton.SetBorderlessStyle(borderColor: Theme.Color.AccentDark);
        }
    }
}
