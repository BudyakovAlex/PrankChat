﻿using System;
using Foundation;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;

namespace PrankChat.Mobile.iOS.Presentation.Views.Order
{
    public partial class OrderItemCell : BaseCell<OrderItemCell, OrderItemViewModel>
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

            innerView.SetCornerRadius(10);

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

            statusOrderLabel.SetMediumStyle(14, Theme.Color.White);
            statusOrderLabel.Text = Resources.Order_View_My_Task;

            orderDetailsButton.SetDarkStyle(Resources.Order_View_Details);
        }

        protected override void SetBindings()
        {
            var set = this.CreateBindingSet<OrderItemCell, OrderItemViewModel>();

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

            set.Apply();
        }
    }
}
