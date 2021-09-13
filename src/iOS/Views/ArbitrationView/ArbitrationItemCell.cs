using System;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.ViewModels.Arbitration.Items;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Binding;
using PrankChat.Mobile.iOS.Extensions;
using PrankChat.Mobile.iOS.Views.Base;

namespace PrankChat.Mobile.iOS.Views.ArbitrationView
{
    public partial class ArbitrationItemCell : BaseTableCell<ArbitrationItemCell, ArbitrationOrderItemViewModel>
	{
		protected ArbitrationItemCell(IntPtr handle)
            : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		protected override void SetupControls()
		{
			base.SetupControls();

			orderTitleLabel.SetScreenTitleStyle();

			timeLablel.SetMediumStyle(10, Theme.Color.White);
			timeLablel.Text = Resources.OrderTime;

			priceLable.SetMediumStyle(10, Theme.Color.White);
			priceLable.Text = Resources.OrderPrice;

			dayLabel.SetRegularStyle(10, Theme.Color.White);
			dayLabel.Text = Resources.Day;

			hourLabel.SetRegularStyle(10, Theme.Color.White);
			hourLabel.Text = Resources.Hour;

			minuteLabel.SetRegularStyle(10, Theme.Color.White);
			minuteLabel.Text = Resources.Minute;

			orderTimeLabel.SetMediumStyle(22, Theme.Color.White);
			priceValueLabel.SetMediumStyle(26, Theme.Color.White);

			orderDetailsButton.TitleLabel.Text = Resources.Estimate;

			OrderStatusLabel.Text = Resources.InDispute;
		}

		protected override void Bind()
		{
			using var bindingSet = this.CreateBindingSet<ArbitrationItemCell, ArbitrationOrderItemViewModel>();

			bindingSet.Bind(this).For(v => v.BindTap()).To(vm => vm.OpenDetailsOrderCommand)
				.Mode(MvxBindingMode.OneTime);
			bindingSet.Bind(backgroundImageView).For(v => v.BindOrderImageStyle()).To(vm => vm.OrderType);
			bindingSet.Bind(profilePhotoImage).For(v => v.ImagePath).To(vm => vm.ProfilePhotoUrl)
				.Mode(MvxBindingMode.OneTime);
			bindingSet.Bind(profilePhotoImage).For(v => v.PlaceholderText).To(vm => vm.ProfileShortName)
	            .Mode(MvxBindingMode.OneTime);
			bindingSet.Bind(profilePhotoImage).For(v => v.BindTap()).To(vm => vm.OpenUserProfileCommand);
			bindingSet.Bind(orderTitleLabel).To(vm => vm.OrderTitle)
				.Mode(MvxBindingMode.OneTime);
			bindingSet.Bind(orderTimeLabel).To(vm => vm.TimeText)
				.Mode(MvxBindingMode.OneTime);
			bindingSet.Bind(priceValueLabel).To(vm => vm.PriceText)
				.Mode(MvxBindingMode.OneTime);
			bindingSet.Bind(orderDetailsButton).To(vm => vm.OpenDetailsOrderCommand)
				.Mode(MvxBindingMode.OneTime);
			bindingSet.Bind(orderDetailsButton).For(v => v.BindOrderButtonStyle()).To(vm => vm.OrderType);
		}
	}
}