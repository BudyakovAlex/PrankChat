using System;
using System.Collections.Generic;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Extensions.MvvmCross;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Extensions;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using PrankChat.Mobile.iOS.Providers;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Registration
{
    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class RegistrationSecondStepView : BaseTransparentBarView<RegistrationSecondStepViewModel>
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
            Title = Resources.RegistrationView_StepTwo_Title;

            nicknameTextField.SetLightStyle(Resources.RegistrationView_Login_Placeholder);
            nameTextField.SetLightStyle(Resources.RegistrationView_Name_Placeholder);
            birthdayTextField.SetLightStyle(Resources.RegistrationView_Birthday_Placeholder, rightImage: UIImage.FromBundle(ImagePathProvider.IconCalendar));

            passwordTextField.SetLightStyle(Resources.RegistrationView_Password_Placeholder);
            passwordTextField.SecureTextEntry = true;
            passwordTextField.TextContentType = UITextContentType.OneTimeCode;

            passwordRepeatTextField.SetLightStyle(Resources.RegistrationView_PasswordRepeat_Placeholder);
            passwordRepeatTextField.SecureTextEntry = true;
            passwordTextField.TextContentType = UITextContentType.OneTimeCode;

            adultCheckButton.SetSelectableImageStyleWithTint(ImagePathProvider.IconUnchecked, ImagePathProvider.IconChecked, Theme.Color.White);
            privacyCheckButton.SetSelectableImageStyleWithTint(ImagePathProvider.IconUnchecked, ImagePathProvider.IconChecked, Theme.Color.White);

            privacyCheckButton.AddGestureRecognizer(new UITapGestureRecognizer(SwitchPolicyState));
            adultCheckButton.AddGestureRecognizer(new UITapGestureRecognizer(SwitchAdultState));

            privacyContainerView.AddGestureRecognizer(new UITapGestureRecognizer(SwitchPolicyState));
            adultContainerView.AddGestureRecognizer(new UITapGestureRecognizer(SwitchAdultState));

            adultLabel.SetSmallSubtitleStyle(Resources.Registration_Confirm_Adult);
            adultLabel.TextColor = Theme.Color.White;

            agreeWithLabel.SetSmallSubtitleStyle(Resources.Registration_Agree_With);
            agreeWithLabel.TextColor = Theme.Color.White;

            termsLabel.SetSmallSubtitleStyle(Resources.Registration_Terms_And_Rules);
            termsBottomLineView.BackgroundColor = termsLabel.TextColor;

            registerButton.SetLightStyle(Resources.RegistrationView_Register_Button);
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