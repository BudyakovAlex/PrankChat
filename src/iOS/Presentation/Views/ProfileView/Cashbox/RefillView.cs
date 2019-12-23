using System.Collections.Generic;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile.Cashbox;
using PrankChat.Mobile.iOS.AppTheme;
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

            set.Bind(refillButton)
                .To(vm => vm.RefillCommand);

            set.Bind(costTextField)
                .To(vm => vm.Cost);

            set.Apply();
        }

        protected override void SetupControls()
        {
            costTextField.SetDarkStyle(Resources.CashboxView_Price_Placeholder);
            costTextField.TextAlignment = UITextAlignment.Right;

            refillButton.SetDarkStyle(Resources.CashboxView_Fillup_Button);

            paymentMethodsTitleLabel.Text = Resources.CashboxView_FillupMethodSelect_Title;
            paymentMethodsTitleLabel.SetRegularStyle(14, Theme.Color.Subtitle);

            mastercardImageView.Image = UIImage.FromBundle("ic_mastercard_banner");

            visaBannerImageView.Image = UIImage.FromBundle("ic_visa_banner");

            secureBannerImageView.Image = UIImage.FromBundle("ic_3dsecure_banner");

            DataSource = new PaymentMethodsCollectionViewSource(paymentMethodsCollectionView, new NSString(PaymentMethodCell.CellId));
            paymentMethodsCollectionView.Source = DataSource;
            paymentMethodsCollectionView.RegisterNibForCell(PaymentMethodCell.Nib, PaymentMethodCell.CellId);
            paymentMethodsCollectionView.UserInteractionEnabled = true;
            paymentMethodsCollectionView.AllowsSelection = true;
            paymentMethodsCollectionView.DelaysContentTouches = false;
        }

        protected override void RegisterKeyboardDismissResponders(List<UIView> views)
        {
            views.Add(refillButton);
            views.Add(paymentMethodsTitleLabel);
            views.Add(paymentMethodsCollectionView);
            base.RegisterKeyboardDismissResponders(views);
        }

        protected override void RegisterKeyboardDismissTextFields(List<UIView> viewList)
        {
            viewList.Add(costTextField);
            base.RegisterKeyboardDismissTextFields(viewList);
        }
    }
}

