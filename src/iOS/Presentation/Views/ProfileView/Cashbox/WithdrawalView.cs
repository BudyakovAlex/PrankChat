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
                .To(vm => vm.Cost)
                .WithConversion<PriceConverter>();

            set.Bind(attachDocumentButton)
                .To(vm => vm.AttachFileCommand);

            set.Bind(cardNumberEditText)
                .To(vm => vm.CardNumber);

            set.Bind(firstNameTextField)
                .To(vm => vm.Name);

            set.Bind(surnameTextField)
                .To(vm => vm.Surname);

            set.Apply();
        }

        protected override void SetupControls()
        {
            cardNumberEditText.SetDarkStyle("Номер карты");
            firstNameTextField.SetDarkStyle("Имя");
            surnameTextField.SetDarkStyle("Фамилия");
            costTextField.SetDarkStyle(Resources.CashboxView_Price_Placeholder);
            costTextField.TextAlignment = UITextAlignment.Right;

            attachDocumentButton.SetDarkStyle("Прикрепить файл");
            withdrawButton.SetDarkStyle(Resources.CashboxView_Withdrawal_Button);

            availableAmountTitleLabel.SetRegularStyle(14, Theme.Color.Black);
            verifyUserLabel.SetRegularStyle(14, Theme.Color.Black);
            pendingVerifyUserLabel.SetRegularStyle(14, Theme.Color.Black);
            pendingWithdrawalLabel.SetRegularStyle(14, Theme.Color.Black);

            verticalSeparatorView.BackgroundColor = Theme.Color.Accent;
            verifyUserSeparator.BackgroundColor = Theme.Color.Accent;
            pendingVerifyUserSeparator.BackgroundColor = Theme.Color.Accent;
            pendingWithdrawalSeparator.BackgroundColor = Theme.Color.Accent;

            questionImageView.Image = UIImage.FromBundle("ic_question");
        }
    }
}

