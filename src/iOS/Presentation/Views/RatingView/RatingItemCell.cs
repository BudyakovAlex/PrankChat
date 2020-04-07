using System;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Rating.Items;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Binding;
using PrankChat.Mobile.iOS.Presentation.Converters;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.RatingView
{
	public partial class RatingItemCell : BaseTableCell<RatingItemCell, RatingItemViewModel>
	{
		protected RatingItemCell(IntPtr handle) : base(handle)
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

			thumbsUpButton.SetImage(UIImage.FromBundle("ic_thumbs_up").ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal), UIControlState.Normal);

			thumbsDownButton.SetImage(UIImage.FromBundle("ic_thumbs_down").ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal), UIControlState.Normal);

			thumbsUpValueLabel.SetMediumStyle(14, Theme.Color.White);

			thumbsDownValueLabel.SetMediumStyle(14, Theme.Color.White);
		}

		protected override void SetBindings()
		{
			var set = this.CreateBindingSet<RatingItemCell, RatingItemViewModel>();

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

			set.Bind(thumbsUpValueLabel)
				.To(vm => vm.Likes);

			set.Bind(thumbsDownValueLabel)
				.To(vm => vm.Dislikes);

			set.Apply();
		}
	}
}