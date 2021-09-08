using CoreAnimation;
using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Combiners;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using PrankChat.Mobile.iOS.Common;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Order
{
    public partial class OrderDetailsView : BaseGradientBarView<OrderDetailsViewModel>
    {
        private MvxUIRefreshControl _refreshControl;
        private UIBarButtonItem _rightBarButtonItem;

        private ArbitrationValueType? _arbitrationValue;
        private CAGradientLayer _gradientLayer;

        public ArbitrationValueType? ArbitrationValue
        {
            get => _arbitrationValue;
            set
            {
                _arbitrationValue = value;
                UpdateStyleArbitrationButtons();
            }
        }

        protected override void Bind()
        {
            using var bindingSet = this.CreateBindingSet<OrderDetailsView, OrderDetailsViewModel>();

            #region Customer

            bindingSet.Bind(profileImageView).For(v => v.ImagePath).To(vm => vm.CustomerSectionViewModel.ProfilePhotoUrl);
            bindingSet.Bind(profileImageView).For(v => v.PlaceholderText).To(vm => vm.CustomerSectionViewModel.ProfileShortName);
            bindingSet.Bind(profileImageView).For(v => v.BindTap()).To(vm => vm.CustomerSectionViewModel.OpenCustomerProfileCommand);
            bindingSet.Bind(profileNameLabel).To(vm => vm.CustomerSectionViewModel.ProfileName);

            #endregion Customer

            #region LoadVideo

            bindingSet.Bind(downloadButton).To(vm => vm.VideoSectionViewModel.LoadVideoCommand);
            bindingSet.Bind(downloadButton).For(v => v.BindVisible()).To(vm => vm.VideoSectionViewModel.IsVideoLoadAvailable);
            bindingSet.Bind(downloadView).For(v => v.BindVisible()).To(vm => vm.VideoSectionViewModel.IsVideoLoadAvailable);

            #endregion LoadVideo

            #region Orders

            bindingSet.Bind(videoNameLabel).To(vm => vm.OrderTitle);
            bindingSet.Bind(hiddentView).For(v => v.BindVisible()).To(vm => vm.IsHiddenOrder);
            bindingSet.Bind(videoDescriptionLabel).For(v => v.TextAlignment).To(vm => vm.IsHiddenOrder)
                      .WithConversion(new BoolToStateConverter<UITextAlignment>(UITextAlignment.Left, UITextAlignment.Center));
            bindingSet.Bind(videoDescriptionLabel).To(vm => vm.OrderDescription);
            bindingSet.Bind(priceValueLabel).To(vm => vm.PriceValue);
            bindingSet.Bind(daysValueLabel).To(vm => vm.TimeDaysValue);
            bindingSet.Bind(hourValueLabel).To(vm => vm.TimeHourValue);
            bindingSet.Bind(minutesValueLabel).To(vm => vm.TimeMinutesValue);
            bindingSet.Bind(timeTextLabel).For(v => v.BindVisible()).To(vm => vm.IsTimeAvailable);
            bindingSet.Bind(daysTitleLabel).For(v => v.BindVisible()).To(vm => vm.IsTimeAvailable);
            bindingSet.Bind(daysValueLabel).For(v => v.BindVisible()).To(vm => vm.IsTimeAvailable);
            bindingSet.Bind(delimiterTimeOneLabel).For(v => v.BindVisible()).To(vm => vm.IsTimeAvailable);
            bindingSet.Bind(delimiterTimeTwoLabel).For(v => v.BindVisible()).To(vm => vm.IsTimeAvailable);
            bindingSet.Bind(hourTitleLabel).For(v => v.BindVisible()).To(vm => vm.IsTimeAvailable);
            bindingSet.Bind(hourValueLabel).For(v => v.BindVisible()).To(vm => vm.IsTimeAvailable);
            bindingSet.Bind(minutesTitleLabel).For(v => v.BindVisible()).To(vm => vm.IsTimeAvailable);
            bindingSet.Bind(minutesValueLabel).For(v => v.BindVisible()).To(vm => vm.IsTimeAvailable);
            bindingSet.Bind(timeView).For(v => v.BindVisible()).To(vm => vm.IsTimeAvailable);
            bindingSet.Bind(stackViewToPriceValueLabelConstraint).For(v => v.Active).To(vm => vm.IsTimeAvailable)
                      .WithConversion<MvxInvertedBooleanConverter>();

            #endregion Orders

            #region Video

            bindingSet.Bind(videoContainerView).For(v => v.BindVisible())
                      .ByCombining(new MvxOrValueCombiner(),
                                   vm => vm.VideoSectionViewModel.IsVideoAvailable,
                                   vm => vm.VideoSectionViewModel.IsVideoProcessing);
            bindingSet.Bind(processingRootBackgroundView).For(v => v.BindVisible()).To(vm => vm.VideoSectionViewModel.IsVideoProcessing);
            bindingSet.Bind(videoView).For(v => v.BindVisible()).To(vm => vm.VideoSectionViewModel.IsVideoAvailable);
            bindingSet.Bind(videoImageView).For(v => v.ImagePath).To(vm => vm.VideoSectionViewModel.VideoPlaceholderUrl);
            bindingSet.Bind(videoImageView).For(v => v.BindTap()).To(vm => vm.VideoSectionViewModel.ShowFullVideoCommand);

            #endregion Video

            bindingSet.Bind(orderActionsContainerView).For(v => v.BindVisible()).To(vm => vm.IsAnyOrderActionAvailable);

            #region Subscribe and Unsubscribe

            bindingSet.Bind(subscriptionButton).To(vm => vm.SubscribeOrderCommand);
            bindingSet.Bind(subscriptionButton).For(v => v.BindVisible()).To(vm => vm.IsSubscribeAvailable);
            bindingSet.Bind(unsubscriptionButton).To(vm => vm.UnsubscribeOrderCommand);
            bindingSet.Bind(unsubscriptionButton).For(v => v.BindVisible()).To(vm => vm.IsUnsubscribeAvailable);

            #endregion Subscribe and Unsubscribe

            #region Take order

            bindingSet.Bind(takeOrderButton).To(vm => vm.TakeOrderCommand);
            bindingSet.Bind(takeOrderButton).For(v => v.BindVisible()).To(vm => vm.IsTakeOrderAvailable);

            #endregion Take order

            #region Decide View

            bindingSet.Bind(decideView).For(v => v.BindVisible()).To(vm => vm.VideoSectionViewModel.IsDecideVideoAvailable);
            bindingSet.Bind(noButton).To(vm => vm.NoCommand);
            bindingSet.Bind(noButton).For(v => v.BindTitle()).To(vm => vm.NoText);
            bindingSet.Bind(yesButton).To(vm => vm.YesCommand);
            bindingSet.Bind(yesButton).For(v => v.BindTitle()).To(vm => vm.YesText);
            bindingSet.Bind(this).For(v => v.ArbitrationValue).To(vm => vm.SelectedArbitration);

            #endregion Decide View

            #region Executor

            bindingSet.Bind(executorImageView).For(v => v.ImagePath).To(vm => vm.ExecutorSectionViewModel.ExecutorPhotoUrl);
            bindingSet.Bind(executorImageView).For(v => v.BindTap()).To(vm => vm.ExecutorSectionViewModel.OpenExecutorProfileCommand);
            bindingSet.Bind(executorImageView).For(v => v.PlaceholderText).To(vm => vm.ExecutorSectionViewModel.ExecutorShortName);
            bindingSet.Bind(executorNameLabel).To(vm => vm.ExecutorSectionViewModel.ExecutorName);
            bindingSet.Bind(startDateLabel).To(vm => vm.StartOrderDate);
            bindingSet.Bind(executorView).For(v => v.BindVisible()).To(vm => vm.ExecutorSectionViewModel.IsExecutorAvailable);

            #endregion Executor

            #region Decision View

            bindingSet.Bind(decisionView).For(v => v.BindVisible()).To(vm => vm.VideoSectionViewModel.IsDecisionVideoAvailable);
            bindingSet.Bind(acceptButton).To(vm => vm.AcceptOrderCommand);
            bindingSet.Bind(arqueButton).To(vm => vm.ArqueOrderCommand);

            #endregion Decision View

            #region Cancel

            bindingSet.Bind(cancelVideoButton).To(vm => vm.CancelOrderCommand);
            bindingSet.Bind(cancelVideoButton).For(v => v.BindVisible()).To(vm => vm.IsCancelOrderAvailable);

            #endregion Cancel

            #region Execute video

            bindingSet.Bind(executeVideoButton).To(vm => vm.ExecuteOrderCommand);
            bindingSet.Bind(executeVideoButton).For(v => v.BindVisible()).To(vm => vm.IsExecuteOrderAvailable);

            #endregion Execute video

            bindingSet.Bind(progressBarView).For(v => v.BindVisible()).To(vm => vm.VideoSectionViewModel.IsBusy);
            bindingSet.Bind(_refreshControl).For(v => v.IsRefreshing).To(vm => vm.IsBusy);
            bindingSet.Bind(_refreshControl).For(v => v.RefreshCommand).To(vm => vm.LoadOrderDetailsCommand);
            bindingSet.Bind(_rightBarButtonItem).To(vm => vm.OpenSettingsCommand);
            bindingSet.Bind(uploadingProgressView).For(v => v.BindVisible()).To(vm => vm.VideoSectionViewModel.IsUploading);
            bindingSet.Bind(uploadingProgressBar).For(v => v.Progress).To(vm => vm.VideoSectionViewModel.UploadingProgress);
            bindingSet.Bind(uploadingProgressBar).For(v => v.BindTap()).To(vm => vm.VideoSectionViewModel.CancelUploadingCommand);
            bindingSet.Bind(uploadingLabel).For(v => v.Text).To(vm => vm.VideoSectionViewModel.UploadingProgressStringPresentation);
        }

        protected override void SetupControls()
        {
            InitializeRightBarButtonItem();

            Title = Resources.OrderDetailsView_Title;

            takeOrderButton.SetDarkStyle(Resources.OrderDetailsView_Take_Order_Button);
            subscriptionButton.SetDarkStyle(Resources.OrderDetailsView_Subscribe_Button);
            unsubscriptionButton.SetDarkStyle(Resources.OrderDetailsView_Unsubscribe_Button);
            executeVideoButton.SetDarkStyle(Resources.OrderDetailsView_Execute_Button);
            acceptButton.SetDarkStyle(Resources.OrderDetailsView_Accept_Button);
            arqueButton.SetBorderlessStyle(Resources.OrderDetailsView_Argue_Button);
            downloadButton.SetDarkStyle(Resources.OrderDetailsView_LoadVideo);
            cancelVideoButton.SetDarkStyle(Resources.OrderDetailsView_Cancel_Button);

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
            videoImageView.BackgroundColor = Theme.Color.Black;
            videoImageView.ContentMode = UIViewContentMode.ScaleAspectFit;

            rootScrollView.RefreshControl = _refreshControl = new MvxUIRefreshControl();

            processingRootBackgroundView.BackgroundColor = UIColor.Clear;
            _gradientLayer = new CAGradientLayer
            {
                Colors = new[] { Theme.Color.CompetitionPhaseNewPrimary.CGColor, Theme.Color.CompetitionPhaseNewSecondary.CGColor },
                CornerRadius = 10,
                StartPoint = new CGPoint(0f, 1f),
                EndPoint = new CGPoint(1f, 0f)
            };

            processingLabel.Text = Resources.Processing_Video;
            processingRootBackgroundView.Layer.InsertSublayer(_gradientLayer, 0);
            processingBackgroundView.Layer.CornerRadius = 8;

            uploadingInfoView.Layer.CornerRadius = 15;
            uploadingLabel.SetRegularStyle(12, UIColor.White);

            uploadingProgressBar.ProgressColor = UIColor.White;
            uploadingProgressBar.RingThickness = 5;
            uploadingProgressBar.BaseColor = UIColor.DarkGray;
            uploadingProgressBar.Progress = 0f;
        }

        private void InitializeRightBarButtonItem()
        {
            _rightBarButtonItem = new UIBarButtonItem
            {
                Title = string.Empty,
                Image = UIImage.FromBundle(ImageNames.IconThreeDots).ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate),
                TintColor = Theme.Color.White
            };

            NavigationItem.RightBarButtonItem = _rightBarButtonItem;
        }
        
        private void SetStyleForButton(UIButton button, string type)
        {
            button.BackgroundColor = Theme.Color.AccentDark;
            button.SetTitleColor(UIColor.White, UIControlState.Selected);
            button.SetTitleColor(Theme.Color.AccentDark, UIControlState.Normal);
            button.SetSelectableImageStyle($"ic_accent_thumbs_{type}", $"ic_thumbs_{type}");
            button.UserInteractionEnabled = true;
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            _gradientLayer.Frame = processingRootBackgroundView.Bounds;
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
