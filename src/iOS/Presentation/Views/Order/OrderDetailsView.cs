using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;

namespace PrankChat.Mobile.iOS.Presentation.Views.Order
{
    public partial class OrderDetailsView : BaseGradientBarView<OrderDetailsViewModel>
    {
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
                .To(vm => vm.ProfilePhotoUrl);

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

            set.Bind(timeValueLabel)
                .To(vm => vm.TimeValue);

            #endregion Orders

            #region Video

            set.Bind(videoImageView)
                .For(v => v.ImagePath)
                .To(vm => vm.VideoUrl);

            set.Bind(videoImageView)
                .For(v => v.BindVisible())
                .To(vm => vm.IsVideoAvailable);

            #endregion Video

            #region Subscribe and Unsubscribe

            set.Bind(subscriptionButton)
                .To(vm => vm.SubscribeTheOrderCommand);

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

            set.Bind(yesButton)
                .To(vm => vm.YesCommand);

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
                .To(vm => vm.ExecutorPhotoUrl);

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
                .To(vm => vm.UnsubscribeOrderCommand);

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

            set.Apply();
        }

        protected override void SetupControls()
        {
            Title = Resources.OrderDetailsView_Title;

            takeOrderButton.SetDarkStyle(Resources.OrderDetailsView_Take_Order_Button);
            subscriptionButton.SetDarkStyle(Resources.OrderDetailsView_Subscribe_Button);
            unsubscriptionButton.SetDarkStyle(Resources.OrderDetailsView_Unsubscribe_Button);
            noButton.SetDarkStyle(Resources.OrderDetailsView_No_Button);
            yesButton.SetDarkStyle(Resources.OrderDetailsView_Yes_Button);
            executeVideoButton.SetDarkStyle(Resources.OrderDetailsView_Execute_Button);
            acceptButton.SetDarkStyle(Resources.OrderDetailsView_Accept_Button);
            arqueButton.SetBorderlessStyle(Resources.OrderDetailsView_Argue_Button);
            cancelVideoButton.SetBorderlessStyle(Resources.OrderDetailsView_Cancel_Button, Theme.Color.Accent);

            profileNameLabel.SetTitleStyle();
            videoNameLabel.SetBoldTitleStyle();
            videoDescriptionLabel.SetTitleStyle();
            priceTextLabel.SetSmallTitleStyle(Resources.OrderDetailsView_Price_Text);
            priceValueLabel.SetMediumStyle(26, Theme.Color.Text);
            timeTextLabel.SetSmallTitleStyle(Resources.OrderDetailsView_Time_Text);
            timeValueLabel.SetMediumStyle(26, Theme.Color.Text);
            downloadVideotextLabel.SetSmallTitleStyle(Resources.OrderDetailsView_Download_Text);
            tookOrderTextLabel.SetSmallTitleStyle(Resources.OrderDetailsView_Took_The_Order_Text);
            executorNameLabel.SetTitleStyle();
            startDateLabel.SetSmallSubtitleStyle();
        }
    }
}
