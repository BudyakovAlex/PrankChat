using System;
using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile.Cashbox;
using PrankChat.Mobile.iOS.Presentation.Converters;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.ProfileView.Cashbox
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

            BackgroundView = new UIImageView(UIImage.FromBundle("bg_payment"));
            SelectedBackgroundView = new UIImageView(UIImage.FromBundle("bg_selected_payment"));
        }

        protected override void SetBindings()
        {
            base.SetBindings();

            var setBind = this.CreateBindingSet<PaymentMethodCell, PaymentMethodItemViewModel>();

            setBind.Bind(paymentImageView)
                .To(vm => vm.IsSelected)
                .WithConversion<BoolToUIImageConverter>(ViewModel.Type);

            setBind.Apply();
        }
    }
}

