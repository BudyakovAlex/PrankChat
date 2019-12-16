using System;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile.Cashbox;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.ProfileView.Cashbox
{
    public partial class RefillView : BaseView<RefillViewModel>
    {
        public MvxCollectionViewSource DataSource { get; private set; }

        protected override void SetupBinding()
        {
            var set = this.CreateBindingSet<RefillView, RefillViewModel>();

            set.Bind(DataSource)
                .To(vm => vm.Items);

            set.Bind(DataSource)
                .For(v => v.SelectionChangedCommand)
                .To(vm => vm.SelectionChangedCommand);

            set.Apply();
        }

        protected override void SetupControls()
        {
            DataSource = new PaymentMethodsCollectionViewSource(paymentMethodsCollectionView, new NSString(PaymentMethodCell.CellId));
            paymentMethodsCollectionView.Source = DataSource;
            paymentMethodsCollectionView.RegisterNibForCell(PaymentMethodCell.Nib, PaymentMethodCell.CellId);
            paymentMethodsCollectionView.UserInteractionEnabled = true;
            paymentMethodsCollectionView.AllowsSelection = true;
            paymentMethodsCollectionView.DelaysContentTouches = false;
        }
    }
}

