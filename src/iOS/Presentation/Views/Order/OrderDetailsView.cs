using CoreAnimation;
using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Combiners;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
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

        protected override void SetupBinding()
        {
            var set = this.CreateBindingSet<OrderDetailsView, OrderDetailsViewModel>();

            #region Customer

            set.Bind(profileImageView)
                .For(v => v.ImagePath)
                .To(vm => vm.ProfilePhotoUrl);

            set.Bind(profileImageView)
                .For(v => v.PlaceholderText)
                .To(vm => vm.ProfileShortName);

            set.Bind(profileNameLabel)
                .To(vm => vm.ProfileName);

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

            set.Bind(videoContainerView)
               .For(v => v.BindVisible())
               .ByCombining(new MvxOrValueCombiner(),
                            vm => vm.IsVideoAvailable,
                            vm => vm.IsVideoProcessing);

            set.Bind(processingRootBackgroundView)
               .For(v => v.BindVisible())
               .To(vm => vm.IsVideoProcessing);

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

            set.Bind(orderActionsContainerView)
                .For(v => v.BindVisible())
                .To(vm => vm.IsAnyOrderActionAvailable);

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
                .For(v => v.ImagePath)
                .To(vm => vm.ExecutorPhotoUrl);

            set.Bind(executorImageView)
                .For(v => v.PlaceholderText)
                .To(vm => vm.ExecutorShortName);

            set.Bind(executorNameLabel)
                .To(vm => vm.ExecutorName);

            set.Bind(startDateLabel)
                .To(vm => vm.StartOrderDate);

            set.Bind(executorView)
                .For(v => v.BindVisible())
                .To(vm => vm.IsExecutorAvailable);

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

            set.Bind(_rightBarButtonItem)
                .To(vm => vm.OpenSettingsCommand);

            set.Bind(uploadingProgressView)
               .For(v => v.BindVisible())
               .To(vm => vm.IsUploading);

            set.Bind(uploadingProgressBar)
               .For(v => v.Progress)
               .To(vm => vm.UploadingProgress);

            set.Bind(uploadingProgressBar)
               .For(v => v.BindTap())
               .To(vm => vm.CancelUploadingCommand);

            set.Bind(uploadingLabel)
               .For(v => v.Text)
               .To(vm => vm.UploadingProgressStringPresentation);

            set.Apply();
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
                Image = UIImage.FromBundle("ic_three_dots").ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate),
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
