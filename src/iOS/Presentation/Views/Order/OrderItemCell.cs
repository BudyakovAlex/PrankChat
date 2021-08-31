﻿using System;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.ViewModels.Order.Items;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Binding;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Order
{
    public partial class OrderItemCell : BaseTableCell<OrderItemCell, OrderItemViewModel>
    {
        private OrderTagType _orderTagType;

        protected OrderItemCell(IntPtr handle)
            : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public OrderTagType OrderTagType
        {
            get => _orderTagType;
            set
            {
                _orderTagType = value;

                var imageName = GetImageName(_orderTagType);
                OrderTagTypeImageView.Image = imageName == null ? null : UIImage.FromBundle(imageName);
            }
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

        protected override void Bind()
        {
            using var bindingSet = this.CreateBindingSet<OrderItemCell, OrderItemViewModel>();

            bindingSet.Bind(this).For(v => v.OrderTagType).To(vm => vm.OrderTagType);
            bindingSet.Bind(this).For(v => v.BindTap()).To(vm => vm.OpenDetailsOrderCommand)
                .Mode(MvxBindingMode.OneTime);
            bindingSet.Bind(backgroundImageView).For(UIImageViewOrderTypeTargetBinding.TargetBinding).To(vm => vm.OrderType);
            bindingSet.Bind(profilePhotoImage).For(v => v.ImagePath).To(vm => vm.ProfilePhotoUrl)
                .Mode(MvxBindingMode.OneTime);
            bindingSet.Bind(profilePhotoImage).For(v => v.PlaceholderText).To(vm => vm.ProfileShortName)
                .Mode(MvxBindingMode.OneTime);
            bindingSet.Bind(profilePhotoImage).For(v => v.BindTap()).To(vm => vm.OpenUserProfileCommand);
            bindingSet.Bind(orderTitleLabel).To(vm => vm.Title)
                .Mode(MvxBindingMode.OneTime);
            bindingSet.Bind(orderTimeLabel).To(vm => vm.TimeText)
                .Mode(MvxBindingMode.OneWay);
            bindingSet.Bind(priceValueLabel).To(vm => vm.PriceText)
                .Mode(MvxBindingMode.OneTime);
            bindingSet.Bind(orderDetailsButton).To(vm => vm.OpenDetailsOrderCommand)
                .Mode(MvxBindingMode.OneTime);
            bindingSet.Bind(orderDetailsButton).For(UIButtonOrderTypeTargetBinding.TargetBinding).To(vm => vm.OrderType);
            bindingSet.Bind(statusOrderLabel).To(vm => vm.StatusText)
                .Mode(MvxBindingMode.OneWay);
            bindingSet.Bind(orderTimeLabel).For(v => v.BindVisible()).To(vm => vm.IsTimeAvailable);
            bindingSet.Bind(titleTimeView).For(v => v.BindVisible()).To(vm => vm.IsTimeAvailable);
            bindingSet.Bind(timeLabel).For(v => v.BindVisible()).To(vm => vm.IsTimeAvailable);
            bindingSet.Bind(OrderTagTypeImageView).For(v => v.BindVisible()).To(vm => vm.OrderTagType);
            bindingSet.Bind(IsHiddenOrderImageView).For(v => v.BindVisible()).To(vm => vm.IsHiddenOrder);
        }

        private string GetImageName(OrderTagType orderTagType)
        {
            switch (orderTagType)
            {
                case OrderTagType.InModeration:
                    return "OrderTagTypeInModeration";
                case OrderTagType.New:
                    return "OrderTagTypeNew";
                case OrderTagType.NewNotMine:
                    return "OrderTagTypeNewNotMine";
                case OrderTagType.Wait:
                    return "OrderTagTypeWait";
                case OrderTagType.Finished:
                    return "OrderTagTypeFinished";
                case OrderTagType.InArbitration:
                    return "OrderTagTypeInArbitration";
                case OrderTagType.InWork:
                    return "OrderTagTypeInWork";
                case OrderTagType.None:
                default:
                    return null;
            }
        }
    }
}