using System;
using Foundation;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
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
				.To(vm => vm.VideoUrl)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(profileImageView)
				.For(v => v.DownsampleWidth)
				.To(vm => vm.DownsampleWidth)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(profileImageView)
				.For(v => v.Transformations)
				.To(vm => vm.Transformations)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(profileImageView)
				.For(v => v.ImagePath)
				.To(vm => vm.ProfilePhotoUrl)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(profileNameLabel)
				.To(vm => vm.ProfileName)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(videoNameLabel)
				.To(vm => vm.VideoName)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(videoDescriptionLabel)
				.To(vm => vm.VideoDetails)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(priceValueLabel)
				.To(vm => vm.PriceValue)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(timeValueLabel)
				.To(vm => vm.TimeValue)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(takeOrderButton)
				.To(vm => vm.TakeOrderCommand)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(subscriptionButton)
				.To(vm => vm.SubscribeTheOrderCommand)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(unsubscriptionButton)
				.To(vm => vm.UnsubscribeOrderCommand)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(executorImageView)
				.For(v => v.DownsampleWidth)
				.To(vm => vm.DownsampleWidth)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(executorImageView)
				.For(v => v.Transformations)
				.To(vm => vm.Transformations)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(executorImageView)
				.For(v => v.ImagePath)
				.To(vm => vm.ExecutorPhotoUrl)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(executorNameLabel)
				.To(vm => vm.ExecutorName)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(startDateLabel)
				.To(vm => vm.StartOrderDate)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(noButton)
				.To(vm => vm.NoCommand)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(yesButton)
				.To(vm => vm.YesCommand)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(downloadButton)
				.To(vm => vm.UnsubscribeOrderCommand)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(executeVideoButton)
				.To(vm => vm.ExecuteOrderCommand)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(cancelVideoButton)
				.To(vm => vm.CancelOrderCommand)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(acceptButton)
				.To(vm => vm.UnsubscribeOrderCommand)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(arqueButton)
				.To(vm => vm.ArqueOrderCommand)
				.Mode(MvxBindingMode.OneTime);

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

