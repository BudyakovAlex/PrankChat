using System.Collections.Generic;
using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Binding.Views.Gestures;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Plugin.Visibility;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Binding;
using PrankChat.Mobile.iOS.Presentation.Converters;
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

            set.Bind(femaleIconButton)
                .To(vm => vm.SelectGenderCommand)
                .CommandParameter(GenderType.Female);

            set.Bind(femaleIconButton)
                .For(UIButtonSelectedTargetBinding.TargetBinding)
                .To(vm => vm.IsGenderFemale);

            set.Bind(maleIconButton)
                .To(vm => vm.SelectGenderCommand)
                .CommandParameter(GenderType.Male);

            set.Bind(maleIconButton)
                .For(UIButtonSelectedTargetBinding.TargetBinding)
                .To(vm => vm.IsGenderMale);

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

            birthdayTextField.SetLightStyle(Resources.RegistrationView_Birthday_Placeholder);
            var imageView = new UIImageView(UIImage.FromBundle("ic_calendar"));
            var imageContainer = new UIView(new CGRect(0, 0, 35, 22));
            imageContainer.ContentMode = UIViewContentMode.Center;
            imageContainer.AddSubview(imageView);
            birthdayTextField.RightView = imageContainer;
            birthdayTextField.RightViewMode = UITextFieldViewMode.Always;

            passwordTextField.SetLightStyle(Resources.RegistrationView_Password_Placeholder);
            passwordTextField.SecureTextEntry = true;
            passwordTextField.TextContentType = UITextContentType.OneTimeCode;

            passwordRepeatTextField.SetLightStyle(Resources.RegistrationView_PasswordRepeat_Placeholder);
            passwordRepeatTextField.SecureTextEntry = true;
            passwordTextField.TextContentType = UITextContentType.OneTimeCode;

            sexSelectTitleLabel.Text = Resources.RegistrationView_GenderSelect_Title;
            sexSelectTitleLabel.TextColor = Theme.Color.White;
            sexSelectTitleLabel.Font = Theme.Font.RegularFontOfSize(14);

            maleTitleButton.SetTitle(Resources.RegistrationView_Male_Button, UIControlState.Normal);
            maleTitleButton.SetRadioTitleStyle();

            femaleTitleButton.SetTitle(Resources.RegistrationView_Female_Button, UIControlState.Normal);
            femaleTitleButton.SetRadioTitleStyle();

            femaleIconButton.SetSelectableImageStyle("ic_radio_button_inactive", "ic_radio_button_active");
            maleIconButton.SetSelectableImageStyle("ic_radio_button_inactive", "ic_radio_button_active");

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

