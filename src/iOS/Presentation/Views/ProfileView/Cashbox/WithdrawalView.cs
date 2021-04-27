﻿using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Combiners;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Binding.Views.Gestures;
using MvvmCross.Platforms.Ios.Views;
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
        private MvxUIRefreshControl _refreshControl;

        protected override void SetupBinding()
        {
            using var bindingSet = this.CreateBindingSet<WithdrawalView, WithdrawalViewModel>();

            bindingSet.Bind(creditCardView).For(v => v.BindVisible()).To(vm => vm.IsWithdrawalAvailable);
            bindingSet.Bind(costTextField).To(vm => vm.Cost).WithConversion<PriceConverter>();
            bindingSet.Bind(cardNumberEditText).To(vm => vm.CardNumber);
            bindingSet.Bind(cardNumberEditText).For(v => v.Hidden).To(vm => vm.IsPresavedWithdrawalAvailable);

            bindingSet.Bind(savedCardNumberEditText.Tap()).For(v => v.Command).To(vm => vm.OpenCardOptionsCommand);
            bindingSet.Bind(savedCardNumberEditText).For(v => v.BindVisible()).To(vm => vm.IsPresavedWithdrawalAvailable);
            bindingSet.Bind(savedCardNumberEditText).To(vm => vm.CurrentCardNumber);

            bindingSet.Bind(firstNameTextField).To(vm => vm.Name);
            bindingSet.Bind(surnameTextField).To(vm => vm.Surname);
            bindingSet.Bind(middleNameTextField).To(vm => vm.MiddleName);
            bindingSet.Bind(locationTextField).To(vm => vm.Location);
            bindingSet.Bind(passportTextField).To(vm => vm.Passport);
            bindingSet.Bind(nationalityTextField).To(vm => vm.Nationality);

            bindingSet.Bind(middleNameTextField).For(v => v.Hidden).To(vm => vm.IsUserDataSaved);
            bindingSet.Bind(passportTextField).For(v => v.Hidden).To(vm => vm.IsUserDataSaved);
            bindingSet.Bind(nationalityTextField).For(v => v.Hidden).To(vm => vm.IsUserDataSaved);
            bindingSet.Bind(nationalityTextField).For(v => v.Hidden).To(vm => vm.IsUserDataSaved);

            bindingSet.Bind(firstNameTextField).For(v => v.Hidden).ByCombining(
               new MvxAndValueCombiner(),
               vm => vm.IsPresavedWithdrawalAvailable,
               vm => vm.IsUserDataSaved);

            bindingSet.Bind(surnameTextField).For(v => v.Hidden).ByCombining(
                new MvxAndValueCombiner(),
                vm => vm.IsPresavedWithdrawalAvailable,
                vm => vm.IsUserDataSaved);

            bindingSet.Bind(withdrawButton).To(vm => vm.WithdrawCommand);
            bindingSet.Bind(availableAmountTitleLabel).To(vm => vm.AvailableForWithdrawal);

            bindingSet.Bind(pendingWithdrawalView).For(v => v.BindVisible()).To(vm => vm.IsWithdrawalPending);
            bindingSet.Bind(dateValueLabel).To(vm => vm.CreateAtWithdrawal);
            bindingSet.Bind(costValueLabel).To(vm => vm.AmountValue);

            bindingSet.Bind(verifyUserView).For(v => v.BindVisible()).To(vm => vm.IsAttachDocumentAvailable);
            bindingSet.Bind(attachDocumentButton).To(vm => vm.AttachFileCommand);
            bindingSet.Bind(pendingVerifyUserView).For(v => v.BindVisible()).To(vm => vm.IsDocumentPending);
            bindingSet.Bind(cancelWithdrawalButton).To(vm => vm.CancelWithdrawCommand);

            bindingSet.Bind(progressBarView).For(v => v.BindVisible()).To(vm => vm.IsBusy);
            bindingSet.Bind(_refreshControl).For(v => v.IsRefreshing).To(vm => vm.IsUpdatingData);
            bindingSet.Bind(_refreshControl).For(v => v.RefreshCommand).To(vm => vm.UpdateDataCommand);
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
            middleNameTextField.SetDarkStyle(Resources.WithdrawalView_MiddleName);
            passportTextField.SetDarkStyle(Resources.WithdrawalView_Passport);
            nationalityTextField.SetDarkStyle(Resources.WithdrawalView_Nationality);
            locationTextField.SetDarkStyle(Resources.WithdrawalView_Location);

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

            statusValueLabel.SetRegularStyle(12, Theme.Color.Black);
            statusValueLabel.Text = Resources.WithdrawalView_Pending;
            statusTitleLabel.SetRegularStyle(12, Theme.Color.Black);
            statusTitleLabel.Text = Resources.WithdrawalView_Status_Text;

            costValueLabel.SetRegularStyle(12, Theme.Color.Black);
            costTitleLabel.SetRegularStyle(12, Theme.Color.Black);
            costTitleLabel.Text = Resources.WithdrawalView_Cost;

            dateValueLabel.SetRegularStyle(12, Theme.Color.Black);
            dateTitleLabel.SetRegularStyle(12, Theme.Color.Black);
            dateTitleLabel.Text = Resources.WithdrawalView_Create_Date;

            lottieAnimationView.SetAnimationNamed("Animations/ripple_animation");
            lottieAnimationView.LoopAnimation = true;
            lottieAnimationView.Play();

            _refreshControl = new MvxUIRefreshControl();
            scrollView.RefreshControl = _refreshControl;
        }
    }
}

