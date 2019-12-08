using System;
using Foundation;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
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
            priceLable.SetMediumStyle(10, Theme.Color.White);

            dayLabel.SetRegularStyle(10, Theme.Color.White);
            hourLabel.SetRegularStyle(10, Theme.Color.White);
            minuteLabel.SetRegularStyle(10, Theme.Color.White);

            orderTimeLabel.SetMediumStyle(22, Theme.Color.White);
            priceValueLabel.SetMediumStyle(26, Theme.Color.White);

            statusOrderLabel.SetMediumStyle(14, Theme.Color.White);
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

