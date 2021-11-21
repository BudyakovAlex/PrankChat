using System;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.ViewModels.Order.Items;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Binding;
using PrankChat.Mobile.iOS.Extensions;
using PrankChat.Mobile.iOS.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Views.Order
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

            orderTitleLabel.SetWhiteTitleStyle();

            timeLabel.SetMediumStyle(10, Theme.Color.White);
            timeLabel.Text = Resources.OrderTime;

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

            statusOrderLabel.SetMediumStyle(14, Theme.Color.White);
            statusOrderLabel.Text = Resources.MyOrders;

            orderDetailsButton.TitleLabel.Text = Resources.MoreDetails;
        }

        protected override void Bind()
        {
            using var bindingSet = this.CreateBindingSet<OrderItemCell, OrderItemViewModel>();

            bindingSet.Bind(this).For(v => v.OrderTagType).To(vm => vm.OrderTagType);
            bindingSet.Bind(this).For(v => v.BindTap()).To(vm => vm.OpenDetailsOrderCommand)
                .Mode(MvxBindingMode.OneTime);
            bindingSet.Bind(backgroundImageView).For(v => v.BindOrderImageStyle()).To(vm => vm.OrderType);
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
            bindingSet.Bind(orderDetailsButton).For(v => v.BindOrderButtonStyle()).To(vm => vm.OrderType);
            bindingSet.Bind(statusOrderLabel).To(vm => vm.StatusText)
                .Mode(MvxBindingMode.OneWay);
            bindingSet.Bind(orderTimeLabel).For(v => v.BindVisible()).To(vm => vm.IsTimeAvailable);
            bindingSet.Bind(titleTimeView).For(v => v.BindVisible()).To(vm => vm.IsTimeAvailable);
            bindingSet.Bind(timeLabel).For(v => v.BindVisible()).To(vm => vm.IsTimeAvailable);
            bindingSet.Bind(OrderTagTypeImageView).For(v => v.BindVisible()).To(vm => vm.OrderTagType);
            bindingSet.Bind(IsHiddenOrderImageView).For(v => v.BindVisible()).To(vm => vm.IsHiddenOrder);
        }

        private string GetImageName(OrderTagType orderTagType) => orderTagType switch
        {
            OrderTagType.InModeration => "OrderTagTypeInModeration",
            OrderTagType.New => "OrderTagTypeNew",
            OrderTagType.NewNotMine => "OrderTagTypeNewNotMine",
            OrderTagType.Wait => "OrderTagTypeWait",
            OrderTagType.Finished => "OrderTagTypeFinished",
            OrderTagType.InArbitration => "OrderTagTypeInArbitration",
            OrderTagType.InWork => "OrderTagTypeInWork",
            OrderTagType.None => null,
            _ => null,
        };
    }
}