using System;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Arbitration.Items;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Binding;
using PrankChat.Mobile.iOS.Presentation.Views.Base;

namespace PrankChat.Mobile.iOS.Presentation.Views.ArbitrationView
{
    public partial class ArbitrationItemCell : BaseTableCell<ArbitrationItemCell, ArbitrationItemViewModel>
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
			timeLablel.Text = Resources.Order_View_Time_Text;

			priceLable.SetMediumStyle(10, Theme.Color.White);
			priceLable.Text = Resources.Order_View_Price_Text;

			dayLabel.SetRegularStyle(10, Theme.Color.White);
			dayLabel.Text = Resources.Order_View_Day;

			hourLabel.SetRegularStyle(10, Theme.Color.White);
			hourLabel.Text = Resources.Order_View_Hour;

			minuteLabel.SetRegularStyle(10, Theme.Color.White);
			minuteLabel.Text = Resources.Order_View_Minute;

			orderTimeLabel.SetMediumStyle(22, Theme.Color.White);
			priceValueLabel.SetMediumStyle(26, Theme.Color.White);

			orderDetailsButton.TitleLabel.Text = Resources.RateView_Vote_Button;

			OrderStatusLabel.Text = Resources.OrderStatus_InArbitration;
		}

		protected override void SetBindings()
		{
			var set = this.CreateBindingSet<ArbitrationItemCell, ArbitrationItemViewModel>();

			set.Bind(this)
				.For(v => v.BindTap())
				.To(vm => vm.OpenDetailsOrderCommand)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(backgroundImageView)
				.For(UIImageViewOrderTypeTargetBinding.TargetBinding)
				.To(vm => vm.OrderType);

			set.Bind(profilePhotoImage)
				.For(v => v.ImagePath)
				.To(vm => vm.ProfilePhotoUrl)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(profilePhotoImage)
	            .For(v => v.PlaceholderText)
	            .To(vm => vm.ProfileShortName)
	            .Mode(MvxBindingMode.OneTime);

			set.Bind(profilePhotoImage)
			   .For(v => v.BindTap())
			   .To(vm => vm.OpenUserProfileCommand);

			set.Bind(orderTitleLabel)
				.To(vm => vm.OrderTitle)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(orderTimeLabel)
				.To(vm => vm.TimeText)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(priceValueLabel)
				.To(vm => vm.PriceText)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(orderDetailsButton)
				.To(vm => vm.OpenDetailsOrderCommand)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(orderDetailsButton)
				.For(UIButtonOrderTypeTargetBinding.TargetBinding)
				.To(vm => vm.OrderType);

			set.Apply();
		}
	}
}