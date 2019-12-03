using System.Collections.Generic;
using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Registration
{
    public partial class RegistrationSecondStepView : BaseTransparentBarView<RegistrationSecondStepViewModel>
    {
        private enum Sex
        {
            Male,
            Female
        }

        protected override void SetupBinding()
        {
            var set = this.CreateBindingSet<RegistrationSecondStepView, RegistrationSecondStepViewModel>();

            set.Bind(nicknameTextField)
                .To(vm => vm.Nickname);

            set.Bind(nameTextField)
                .To(vm => vm.Name);

            //set.Bind(birthdayTextField)
            //    .To(vm => vm.Birthday);

            set.Bind(passwordTextField)
                .To(vm => vm.Password);

            set.Bind(passwordRepeatTextField)
                .To(vm => vm.RepeatedPassword);

            set.Bind(nextStepButton)
                .To(vm => vm.ShowThirdStepCommand);

            set.Apply();
        }

        protected override void SetupControls()
        {
            Title = Resources.RegistrationView_StepTwo_Title;

            nicknameTextField.Placeholder = Resources.RegistrationView_Login_Placeholder;
            nicknameTextField.SetLightStyle();

            nameTextField.Placeholder = Resources.RegistrationView_Name_Placeholder;
            nameTextField.SetLightStyle();

            birthdayTextField.Placeholder = Resources.RegistrationView_Birthday_Placeholder;
            birthdayTextField.SetLightStyle();
            var imageView = new UIImageView(UIImage.FromBundle("ic_calendar"));
            var imageContainer = new UIView(new CGRect(0, 0, 35, 22));
            imageContainer.ContentMode = UIViewContentMode.Center;
            imageContainer.AddSubview(imageView);
            birthdayTextField.RightView = imageContainer;
            birthdayTextField.RightViewMode = UITextFieldViewMode.Always;
            birthdayTextField.AddGestureRecognizer(new UITapGestureRecognizer(HandleCalendarTap));

            passwordTextField.Placeholder = Resources.RegistrationView_Password_Placeholder;
            passwordTextField.SetLightStyle();
            passwordTextField.SecureTextEntry = true;

            passwordRepeatTextField.Placeholder = Resources.RegistrationView_PasswordRepeat_Placeholder;
            passwordRepeatTextField.SetLightStyle();
            passwordRepeatTextField.SecureTextEntry = true;

            sexSelectTitleLabel.Text = Resources.RegistrationView_GenderSelect_Title;
            sexSelectTitleLabel.TextColor = Theme.Color.White;
            sexSelectTitleLabel.Font = Theme.Font.RegularFontOfSize(14);

            maleTitleButton.SetTitle(Resources.RegistrationView_Male_Button, UIControlState.Normal);
            maleTitleButton.AddTarget((s, e) => HandleRadioTap(Sex.Male), UIControlEvent.TouchUpInside);
            maleTitleButton.SetRadioTitleStyle();

            maleIconButton.SetRadioInactiveStyle();
            maleIconButton.AddTarget((s, e) => HandleRadioTap(Sex.Male), UIControlEvent.TouchUpInside);

            maleButtonsContainerView.AddGestureRecognizer(new UITapGestureRecognizer(s => HandleRadioTap(Sex.Male)));

            femaleTitleButton.SetTitle(Resources.RegistrationView_Female_Button, UIControlState.Normal);
            femaleTitleButton.SetRadioTitleStyle();
            femaleTitleButton.AddTarget((s, e) => HandleRadioTap(Sex.Female), UIControlEvent.TouchUpInside);

            femaleIconButton.SetRadioInactiveStyle();
            femaleIconButton.AddTarget((s, e) => HandleRadioTap(Sex.Female), UIControlEvent.TouchUpInside);

            femaleButtonsContainerView.AddGestureRecognizer(new UITapGestureRecognizer(s => HandleRadioTap(Sex.Female)));

            registerButton.SetTitle(Resources.RegistrationView_Register_Button, UIControlState.Normal);
            registerButton.SetLightStyle();
        }

        protected override void RegisterKeyboardDismissResponders(List<UIView> views)
        {
            views.Add(scrollView);
            base.RegisterKeyboardDismissResponders(views);
        }

        protected override void RegisterKeyboardDismissViews(List<UIView> viewList)
        {
            viewList.AddRange(new[] {
                nicknameTextField,
                nameTextField,
                birthdayTextField,
                passwordTextField,
                passwordRepeatTextField
            });

            base.RegisterKeyboardDismissViews(viewList);
        }

        private void HandleRadioTap(Sex sex)
        {
            switch (sex)
            {
                case Sex.Male:
                    // Set male.
                    femaleIconButton.SetRadioInactiveStyle();
                    maleIconButton.SetRadioActiveStyle();
                    // TODO: Command invoke here;
                    break;

                case Sex.Female:
                    // Set female.
                    femaleIconButton.SetRadioActiveStyle();
                    maleIconButton.SetRadioInactiveStyle();
                    // TODO: Command invoke here;
                    break;

            }
        }

        private void HandleCalendarTap(object sender)
        {

        }
    }
}

