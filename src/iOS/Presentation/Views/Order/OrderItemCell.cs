using System;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Binding;
using PrankChat.Mobile.iOS.Presentation.Views.Base;

namespace PrankChat.Mobile.iOS.Presentation.Views.Order
{
    public partial class OrderItemCell : BaseTableCell<OrderItemCell, OrderItemViewModel>
    {
        static OrderItemCell()
        {
            EstimatedHeight = 215;
        }

        protected OrderItemCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        protected override void SetupControls()
        {
            base.SetupControls();

            orderTitleLabel.SetScreenTitleStyle();

            timeLabel.SetMediumStyle(10, Theme.Color.White);
            timeLabel.Text = Resources.Order_View_Time_Text;

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

            statusOrderLabel.SetMediumStyle(14, Theme.Color.White);
            statusOrderLabel.Text = Resources.Order_View_My_Task;

            orderDetailsButton.TitleLabel.Text = Resources.Order_View_Details;
        }

        protected override void SetBindings()
        {
            var set = this.CreateBindingSet<OrderItemCell, OrderItemViewModel>();

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
                .To(vm => vm.Title)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(orderTimeLabel)
                .To(vm => vm.TimeText);

            set.Bind(priceValueLabel)
                .To(vm => vm.PriceText)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(orderDetailsButton)
                .To(vm => vm.OpenDetailsOrderCommand)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(orderDetailsButton)
                .For(UIButtonOrderTypeTargetBinding.TargetBinding)
                .To(vm => vm.OrderType);

            set.Bind(statusOrderLabel)
                .To(vm => vm.StatusText)
                .Mode(MvxBindingMode.OneWay);

            set.Bind(orderTimeLabel)
                .For(v => v.BindVisible())
                .To(vm => vm.IsTimeAvailable);

            set.Bind(titleTimeView)
                .For(v => v.BindVisible())
                .To(vm => vm.IsTimeAvailable);

            set.Bind(timeLabel)
                .For(v => v.BindVisible())
                .To(vm => vm.IsTimeAvailable);

            set.Apply();
        }
    }
}