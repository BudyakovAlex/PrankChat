using System;
using Foundation;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Plugin.Visibility;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Order
{
	public partial class OrderDetailsView : BaseGradientBarView<OrderDetailsViewModel>
	{
		protected override void SetupBinding()
		{
			var set = this.CreateBindingSet<OrderDetailsView, OrderDetailsViewModel>();

			set.Bind(videoImageView)
				.For(v => v.ImagePath)
				.To(vm => vm.VideoUrl);

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

			set.Bind(videoNameLabel)
				.To(vm => vm.VideoName);

			set.Bind(videoDescriptionLabel)
				.To(vm => vm.VideoDetails);

			set.Bind(priceValueLabel)
				.To(vm => vm.PriceValue);

			set.Bind(timeValueLabel)
				.To(vm => vm.TimeValue);

			set.Bind(takeOrderButton)
				.To(vm => vm.TakeOrderCommand);

			set.Bind(takeOrderButton)
	            .For("Visibility")
	            .To(vm => vm.IsTakeOrderAvailable)
	            .WithConversion<MvxVisibilityValueConverter>();

			set.Bind(subscriptionButton)
				.To(vm => vm.SubscribeTheOrderCommand);

			set.Bind(subscriptionButton)
	            .For("Visibility")
	            .To(vm => vm.IsSubscribeAvailable)
	            .WithConversion<MvxVisibilityValueConverter>();

			set.Bind(unsubscriptionButton)
				.To(vm => vm.UnsubscribeOrderCommand);

			set.Bind(unsubscriptionButton)
	            .For("Visibility")
	            .To(vm => vm.IsUnsubscribeAvailable)
	            .WithConversion<MvxVisibilityValueConverter>();

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

			set.Bind(noButton)
				.To(vm => vm.NoCommand);

			set.Bind(yesButton)
				.To(vm => vm.YesCommand);

			set.Bind(downloadButton)
				.To(vm => vm.LoadVideoCommand);

			set.Bind(executeVideoButton)
				.To(vm => vm.ExecuteOrderCommand);

			set.Bind(cancelVideoButton)
				.To(vm => vm.CancelOrderCommand);

			set.Bind(cancelVideoButton)
	            .For("Visibility")
	            .To(vm => vm.IsUserCustomer)
	            .WithConversion<MvxVisibilityValueConverter>();

			set.Bind(acceptButton)
				.To(vm => vm.UnsubscribeOrderCommand);

			set.Bind(arqueButton)
				.To(vm => vm.ArqueOrderCommand);

			set.Bind(progressBarView)
                .For("Visibility")
                .To(vm => vm.IsBusy)
                .WithConversion<MvxVisibilityValueConverter>();

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

