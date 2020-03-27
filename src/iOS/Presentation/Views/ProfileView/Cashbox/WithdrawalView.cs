using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile.Cashbox;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.ProfileView.Cashbox
{
    public partial class WithdrawalView : BaseGradientBarView<WithdrawalViewModel>
    {
        protected override void SetupBinding()
        {
            var set = this.CreateBindingSet<WithdrawalView, WithdrawalViewModel>();

            set.Bind(withdrawButton)
                .To(vm => vm.WithdrawCommand);

            set.Bind(availableAmountTitleLabel)
                .To(vm => vm.AvailableForWithdrawal);

            set.Bind(costTextField)
                .To(vm => vm.Cost);

            set.Bind(attachDocumentButton)
                .To(vm => vm.AttachFileCommand);

            set.Bind(cardNumberEditText)
                .To(vm => vm.CardNumber);

            set.Apply();
        }

        protected override void SetupControls()
        {
            costTextField.SetDarkStyle(Resources.CashboxView_Price_Placeholder);
            costTextField.TextAlignment = UITextAlignment.Right;

            withdrawButton.SetDarkStyle(Resources.CashboxView_Withdrawal_Button);

            availableAmountTitleLabel.SetRegularStyle(14, Theme.Color.Black);

            verticalSeparatorView.BackgroundColor = Theme.Color.Accent;

            questionImageView.Image = UIImage.FromBundle("ic_question");
        }
    }
}

