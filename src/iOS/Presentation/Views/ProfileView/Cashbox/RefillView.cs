using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile.Cashbox;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using PrankChat.Mobile.iOS.Common;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.ProfileView.Cashbox
{
    public partial class RefillView : BaseView<RefillViewModel>
    {
        private UITextPosition _position;

        public MvxCollectionViewSource DataSource { get; private set; }

        protected override void Bind()
        {
            using var bindingSet = this.CreateBindingSet<RefillView, RefillViewModel>();

            bindingSet.Bind(DataSource).To(vm => vm.Items);
            bindingSet.Bind(DataSource).For(v => v.SelectionChangedCommand).To(vm => vm.SelectionChangedCommand);
            bindingSet.Bind(refillButton).To(vm => vm.RefillCommand);
            bindingSet.Bind(costTextField).To(vm => vm.Cost)
                .WithConversion<PriceConverter>();
        }

        protected override void SetupControls()
        {
            costTextField.SetDarkStyle(Resources.Million);
            costTextField.TextAlignment = UITextAlignment.Right;

            refillButton.SetDarkStyle(Resources.Replenish);

            WarningMessageLabel.SetRegularStyle(14, Theme.Color.Text);
            WarningMessageLabel.Text = Resources.CommisionWarning;

            paymentMethodsTitleLabel.Text = Resources.MethodToReplenish;
            paymentMethodsTitleLabel.SetRegularStyle(14, Theme.Color.Subtitle);

            mastercardImageView.Image = UIImage.FromBundle(ImageNames.IconMastercardBanner);

            visaBannerImageView.Image = UIImage.FromBundle(ImageNames.IconVisaBanner);

            secureBannerImageView.Image = UIImage.FromBundle(ImageNames.IconSecureBanner);

            //TODO: remove when all payment methods will be ready
            paymentMethodsCollectionView.Hidden = true;
            paymentMethodsTitleLabel.Hidden = true;

            DataSource = new PaymentMethodsCollectionViewSource(paymentMethodsCollectionView, new NSString(PaymentMethodCell.CellId));
            paymentMethodsCollectionView.Source = DataSource;
            paymentMethodsCollectionView.RegisterNibForCell(PaymentMethodCell.Nib, PaymentMethodCell.CellId);
            paymentMethodsCollectionView.UserInteractionEnabled = true;
            paymentMethodsCollectionView.AllowsSelection = true;
            paymentMethodsCollectionView.DelaysContentTouches = false;
        }

        protected override void Subscription()
        {
            costTextField.EditingChanged += PriceTextFieldEditingChanged;
        }

        protected override void Unsubscription()
        {
            costTextField.EditingChanged -= PriceTextFieldEditingChanged;
        }

        private void PriceTextFieldEditingChanged(object sender, System.EventArgs e)
        {
            var text = costTextField.Text;
            if (string.IsNullOrWhiteSpace(text))
                return;

            if (text.EndsWith(Resources.Currency))
            {
                var position = costTextField.GetPosition(costTextField.EndOfDocument, -2);
                if (_position == position)
                    return;

                _position = position;
                costTextField.SelectedTextRange = costTextField.GetTextRange(_position, _position);
            }
        }
    }
}

