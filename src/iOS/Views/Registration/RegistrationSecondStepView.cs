using System;
using System.Collections.Generic;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Extensions.MvvmCross;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.ViewModels.Registration;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Common;
using PrankChat.Mobile.iOS.Extensions;
using PrankChat.Mobile.iOS.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Views.Registration
{
    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class RegistrationSecondStepView : BaseTransparentBarViewController<RegistrationSecondStepViewModel>
    {
        public override bool CanHandleKeyboardNotifications => true;

        protected override void Bind()
        {
            using var bindingSet = this.CreateBindingSet<RegistrationSecondStepView, RegistrationSecondStepViewModel>();

            bindingSet.Bind(nicknameTextField).To(vm => vm.Login);
            bindingSet.Bind(nameTextField).To(vm => vm.Name);
            bindingSet.Bind(birthdayTextField).To(vm => vm.BirthdayText);

            bindingSet.Bind(birthdayTextField).For(v => v.BindTap()).To(vm => vm.SelectBirthdayCommand);
            bindingSet.Bind(passwordTextField).To(vm => vm.Password);
            bindingSet.Bind(passwordRepeatTextField).To(vm => vm.RepeatedPassword);
            bindingSet.Bind(nextStepButton).To(vm => vm.UserRegistrationCommand);
            bindingSet.Bind(termsLabel).For(v => v.BindTap()).To(vm => vm.ShowTermsAndRulesCommand);
            bindingSet.Bind(privacyCheckButton).For(v => v.BindSelected()).To(vm => vm.IsPolicyChecked).OneWay();
            bindingSet.Bind(adultCheckButton).For(v => v.BindSelected()).To(vm => vm.IsAdultChecked).OneWay();
            bindingSet.Bind(progressBar).For(v => v.BindHidden()).To(vm => vm.IsBusy).WithBoolInvertionConversion();
        }

        protected override void SetupControls()
        {
            Title = Resources.StepTwo;

            nicknameTextField.SetLightStyle(Resources.EnterNickname);
            nameTextField.SetLightStyle(Resources.EnterYourName);
            birthdayTextField.SetLightStyle(Resources.EnterBirthday, rightImage: UIImage.FromBundle(ImageNames.IconCalendar));

            passwordTextField.SetLightStyle(Resources.EnterPassword);
            passwordTextField.SecureTextEntry = true;
            passwordTextField.TextContentType = UITextContentType.OneTimeCode;

            passwordRepeatTextField.SetLightStyle(Resources.EnterPasswordAgain);
            passwordRepeatTextField.SecureTextEntry = true;
            passwordTextField.TextContentType = UITextContentType.OneTimeCode;

            adultCheckButton.SetSelectableImageStyleWithTint(ImageNames.IconUnchecked, ImageNames.IconChecked, Theme.Color.White);
            privacyCheckButton.SetSelectableImageStyleWithTint(ImageNames.IconUnchecked, ImageNames.IconChecked, Theme.Color.White);

            privacyCheckButton.AddGestureRecognizer(new UITapGestureRecognizer(SwitchPolicyState));
            adultCheckButton.AddGestureRecognizer(new UITapGestureRecognizer(SwitchAdultState));

            privacyContainerView.AddGestureRecognizer(new UITapGestureRecognizer(SwitchPolicyState));
            adultContainerView.AddGestureRecognizer(new UITapGestureRecognizer(SwitchAdultState));

            adultLabel.SetSmallSubtitleStyle(Resources.ConfirmAdult);
            adultLabel.TextColor = Theme.Color.White;

            agreeWithLabel.SetSmallSubtitleStyle(Resources.AgreeWith);
            agreeWithLabel.TextColor = Theme.Color.White;

            termsLabel.SetSmallSubtitleStyle(Resources.TermsAndConditions);
            termsBottomLineView.BackgroundColor = termsLabel.TextColor;

            registerButton.SetLightStyle(Resources.Register);
        }

        protected override void RegisterKeyboardDismissResponders(List<UIView> views)
        {
            views.Add(scrollView);
            base.RegisterKeyboardDismissResponders(views);
        }

        protected override void RegisterKeyboardDismissTextFields(List<UIView> viewList)
        {
            viewList.AddRange(new[] {
                nicknameTextField,
                nameTextField,
                birthdayTextField,
                passwordTextField,
                passwordRepeatTextField
            });

            base.RegisterKeyboardDismissTextFields(viewList);
        }

        protected override void OnKeyboardChanged(bool visible, nfloat keyboardHeight)
        {
            base.OnKeyboardChanged(visible, keyboardHeight);

            var bottomInset = visible ? keyboardHeight : 0;
            scrollView.ContentInset = new UIEdgeInsets(0, 0, bottomInset, 0);
        }

        private void SwitchPolicyState() =>
            ViewModel.IsPolicyChecked = !ViewModel.IsPolicyChecked;

        private void SwitchAdultState() =>
            ViewModel.IsAdultChecked = !ViewModel.IsAdultChecked;
    }
}