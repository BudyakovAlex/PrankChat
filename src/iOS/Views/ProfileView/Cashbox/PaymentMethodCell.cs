﻿using System;
using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using PrankChat.Mobile.Core.ViewModels.Profile.Cashbox;
using PrankChat.Mobile.iOS.Common;
using PrankChat.Mobile.iOS.Converters;
using PrankChat.Mobile.iOS.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Views.ProfileView.Cashbox
{
    public partial class PaymentMethodCell : BaseCollectionCell<PaymentMethodCell, PaymentMethodItemViewModel>
    {
        static PaymentMethodCell()
        {
            EstimatedSize = new CGSize(90, 60);
        }

        protected PaymentMethodCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        protected override void SetupControls()
        {
            base.SetupControls();

            BackgroundView = new UIImageView(UIImage.FromBundle(ImageNames.BackgroundPayment));
            SelectedBackgroundView = new UIImageView(UIImage.FromBundle(ImageNames.BackgroundSelectedPayment));
        }

        protected override void SetBindings()
        {
            base.SetBindings();

            using var bindingSet = this.CreateBindingSet<PaymentMethodCell, PaymentMethodItemViewModel>();

            bindingSet.Bind(paymentImageView).To(vm => vm.IsSelected)
                .WithConversion<BoolToUIImageConverter>(ViewModel.Type);
        }
    }
}

