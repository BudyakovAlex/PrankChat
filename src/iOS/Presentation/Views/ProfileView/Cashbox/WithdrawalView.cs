using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Binding.Views.Gestures;
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

            #region CreditCardView

            set.Bind(creditCardView)
                .For(v => v.BindVisible())
                .To(vm => vm.IsWithdrawalAvailable);

            set.Bind(costTextField)
                .To(vm => vm.Cost)
                .WithConversion<PriceConverter>();

            set.Bind(cardNumberEditText)
                .To(vm => vm.CardNumber);

            set.Bind(cardNumberEditText)
                .For(v => v.Hidden)
                .To(vm => vm.IsPresavedWithdrawalAvailable);

            set.Bind(savedCardNumberEditText.Tap())
                .For(v => v.Command)
                .To(vm => vm.OpenCardOptionsCommand);

            set.Bind(savedCardNumberEditText)
                .For(v => v.Hidden)
                .To(vm => vm.IsPresavedWithdrawalAvailable)
                .WithConversion<MvxInvertedBooleanConverter>();

            set.Bind(savedCardNumberEditText)
                .To(vm => vm.CurrentCardNumber);

            set.Bind(firstNameTextField)
                .To(vm => vm.Name);

            set.Bind(surnameTextField)
                .To(vm => vm.Surname);

            set.Bind(nameContainerStackView)
                .For(v => v.Hidden)
                .To(vm => vm.IsPresavedWithdrawalAvailable);

            set.Bind(withdrawButton)
                .To(vm => vm.WithdrawCommand);

            set.Bind(availableAmountTitleLabel)
                .To(vm => vm.AvailableForWithdrawal);

            #endregion

            #region PendingWithdrawalView

            set.Bind(pendingWithdrawalView)
                .For(v => v.BindVisible())
                .To(vm => vm.IsWithdrawalPending);

            set.Bind(dateValueLabel)
                .To(vm => vm.CreateAtWithdrawal);

            set.Bind(costValueLabel)
                .To(vm => vm.AmountValue);

            #endregion

            #region VerifyUserView

            set.Bind(verifyUserView)
                .For(v => v.BindVisible())
                .To(vm => vm.IsAttachDocumentAvailable);

            set.Bind(attachDocumentButton)
                .To(vm => vm.AttachFileCommand);

            #endregion

            #region PendingVerifyUserView

            set.Bind(pendingVerifyUserView)
                .For(v => v.BindVisible())
                .To(vm => vm.IsDocumentPending);

            set.Bind(cancelWithdrawalButton)
                .To(vm => vm.CancelWithdrawCommand);

            #endregion

            set.Bind(progressBarView)
                .For(v => v.BindVisible())
                .To(vm => vm.IsBusy);

            set.Apply();
        }

        protected override void SetupControls()
        {
            cardNumberEditText.SetDarkStyle(Resources.WithdrawalView_CardNumber_Placeholder, UIImage.FromBundle("ic_credit_card"));
            savedCardNumberEditText.SetDarkStyle(
                Resources.WithdrawalView_CardNumber_Placeholder,
                UIImage.FromBundle("ic_credit_card"),
                UIImage.FromBundle("ic_arrow_dropdown"));

            firstNameTextField.SetDarkStyle(Resources.WithdrawalView_FirstName_Placeholder);
            surnameTextField.SetDarkStyle(Resources.WithdrawalView_LastName_Placeholder);
            costTextField.SetDarkStyle(Resources.CashboxView_Price_Placeholder);
            costTextField.TextAlignment = UITextAlignment.Right;

            attachDocumentButton.SetDarkStyle(Resources.WithdrawalView_AttachFile_Button);
            cancelWithdrawalButton.SetDarkStyle(Resources.WithdrawalView_Revoke_Button);
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

            statusValueLabel.SetRegularStyle(12, Theme.Color.Black);
            statusValueLabel.Text = "В ожидании";
            statusTitleLabel.SetRegularStyle(12, Theme.Color.Black);
            statusTitleLabel.Text = "Дата создания";

            costValueLabel.SetRegularStyle(12, Theme.Color.Black);
            costTitleLabel.SetRegularStyle(12, Theme.Color.Black);
            costTitleLabel.Text = "Сумма";

            dateValueLabel.SetRegularStyle(12, Theme.Color.Black);
            dateTitleLabel.SetRegularStyle(12, Theme.Color.Black);
            dateTitleLabel.Text = "Статус";

            lottieAnimationView.SetAnimationNamed("Animations/ripple_animation");
            lottieAnimationView.LoopAnimation = true;
            lottieAnimationView.Play();
        }
    }
}

