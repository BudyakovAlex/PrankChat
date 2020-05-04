using System.Collections.Generic;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Binding.Views.Gestures;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Binding;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Registration
{

    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class RegistrationSecondStepView : BaseTransparentBarView<RegistrationSecondStepViewModel>
    {
        protected override void SetupBinding()
        {
            var set = this.CreateBindingSet<RegistrationSecondStepView, RegistrationSecondStepViewModel>();

            set.Bind(nicknameTextField)
                .To(vm => vm.Login);

            set.Bind(nameTextField)
                .To(vm => vm.Name);

            set.Bind(birthdayTextField)
                .To(vm => vm.BirthdayText);

            set.Bind(birthdayTextField.Tap())
                .For(v => v.Command)
                .To(vm => vm.SelectBirthdayCommand);

            set.Bind(passwordTextField)
                .To(vm => vm.Password);

            set.Bind(passwordRepeatTextField)
                .To(vm => vm.RepeatedPassword);

            set.Bind(nextStepButton)
                .To(vm => vm.UserRegistrationCommand);

            set.Bind(termsLabel)
               .For(v => v.BindTap())
               .To(vm => vm.ShowTermsAndRulesCommand);

            set.Bind(privacyCheckButton)
               .For(UIButtonSelectedTargetBinding.TargetBinding)
               .To(vm => vm.IsPolicyChecked)
               .OneWay();

            set.Bind(adultCheckButton)
               .For(UIButtonSelectedTargetBinding.TargetBinding)
               .To(vm => vm.IsAdultChecked)
               .OneWay();

            set.Bind(progressBar)
                .For(v => v.BindHidden())
                .To(vm => vm.IsBusy)
                .WithConversion<MvxInvertedBooleanConverter>();

            set.Apply();
        }

        protected override void SetupControls()
        {
            Title = Resources.RegistrationView_StepTwo_Title;

            nicknameTextField.SetLightStyle(Resources.RegistrationView_Login_Placeholder);

            nameTextField.SetLightStyle(Resources.RegistrationView_Name_Placeholder);

            birthdayTextField.SetLightStyle(Resources.RegistrationView_Birthday_Placeholder, rightImage: UIImage.FromBundle("ic_calendar"));

            passwordTextField.SetLightStyle(Resources.RegistrationView_Password_Placeholder);
            passwordTextField.SecureTextEntry = true;
            passwordTextField.TextContentType = UITextContentType.OneTimeCode;

            passwordRepeatTextField.SetLightStyle(Resources.RegistrationView_PasswordRepeat_Placeholder);
            passwordRepeatTextField.SecureTextEntry = true;
            passwordTextField.TextContentType = UITextContentType.OneTimeCode;

            adultCheckButton.SetSelectableImageStyleWithTint("ic_checkbox_unchecked", "ic_checkbox_checked", Theme.Color.White);
            privacyCheckButton.SetSelectableImageStyleWithTint("ic_checkbox_unchecked", "ic_checkbox_checked", Theme.Color.White);

            privacyCheckButton.AddGestureRecognizer(new UITapGestureRecognizer(() => ViewModel.IsPolicyChecked = !ViewModel.IsPolicyChecked));
            adultCheckButton.AddGestureRecognizer(new UITapGestureRecognizer(() => ViewModel.IsAdultChecked = !ViewModel.IsAdultChecked));

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
    }
}

