using System;
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
    public partial class WithdrawalView : BaseGradientBarView<WithdrawalViewModel>
    {
        public MvxCollectionViewSource DataSource { get; private set; }

        protected override void SetupBinding()
        {
            var set = this.CreateBindingSet<WithdrawalView, WithdrawalViewModel>();

            set.Bind(DataSource)
                .To(vm => vm.Items);

            set.Bind(DataSource)
                .For(v => v.SelectionChangedCommand)
                .To(vm => vm.SelectionChangedCommand);

            set.Bind(withdrawButton)
                .To(vm => vm.WithdrawCommand);

            set.Bind(availableAmountTitleLabel)
                .To(vm => vm.AvailableForWithdrawal);

            set.Bind(costTextField)
                .To(vm => vm.Cost);

            set.Apply();
        }

        protected override void SetupControls()
        {
            costTextField.SetDarkStyle(Resources.CashboxView_Price_Placeholder);
            costTextField.TextAlignment = UITextAlignment.Right;

            withdrawButton.SetDarkStyle(Resources.CashboxView_Withdrawal_Button);

            availableAmountTitleLabel.SetRegularStyle(14, Theme.Color.Black);

            paymentMethodsTitleLabel.Text = Resources.CashboxView_WithdrawalMethodSelect_Title;
            paymentMethodsTitleLabel.SetRegularStyle(14, Theme.Color.Subtitle);

            verticalSeparatorView.BackgroundColor = Theme.Color.Accent;

            questionImageView.Image = UIImage.FromBundle("ic_question");

            DataSource = new PaymentMethodsCollectionViewSource(paymentMethodsCollectionView, new NSString(PaymentMethodCell.CellId));
            paymentMethodsCollectionView.Source = DataSource;
            paymentMethodsCollectionView.RegisterNibForCell(PaymentMethodCell.Nib, PaymentMethodCell.CellId);
            paymentMethodsCollectionView.UserInteractionEnabled = true;
            paymentMethodsCollectionView.AllowsSelection = true;
            paymentMethodsCollectionView.DelaysContentTouches = false;
        }
    }
}

