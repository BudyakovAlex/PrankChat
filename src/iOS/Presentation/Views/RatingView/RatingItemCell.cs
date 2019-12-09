using System;
using Foundation;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Rating.Items;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.RatingView
{
    public partial class RatingItemCell : BaseCell<RatingItemCell, RatingItemViewModel>
    {
        static RatingItemCell()
        {
            EstimatedHeight = 215;
        }

        protected RatingItemCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        protected override void SetupControls()
        {
            base.SetupControls();

            innerView.SetCornerRadius(10);

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

            orderDetailsButton.SetDarkStyle(Resources.RateView_Vote_Button);

            thumbsUpButton.SetImage(UIImage.FromBundle("ic_thumbs_up").ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal), UIControlState.Normal);

            thumbsDownButton.SetImage(UIImage.FromBundle("ic_thumbs_down").ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal), UIControlState.Normal);

            thumbsUpValueLabel.SetMediumStyle(14, Theme.Color.White);

            thumbsDownValueLabel.SetMediumStyle(14, Theme.Color.White);
        }

        protected override void SetBindings()
        {
            var set = this.CreateBindingSet<RatingItemCell, RatingItemViewModel>();

            set.Bind(profilePhotoImage)
                .For(v => v.DownsampleWidth)
                .To(vm => vm.DownsampleWidth)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(profilePhotoImage)
                .For(v => v.Transformations)
                .To(vm => vm.Transformations)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(profilePhotoImage)
                .For(v => v.ImagePath)
                .To(vm => vm.ProfilePhotoUrl)
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

            set.Bind(thumbsUpValueLabel)
                .To(vm => vm.Likes);

            set.Bind(thumbsDownValueLabel)
                .To(vm => vm.Dislikes);

            set.Apply();
        }
    }
}

