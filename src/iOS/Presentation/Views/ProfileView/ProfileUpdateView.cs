using System;
using System.Collections.Generic;
using Foundation;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Binding.Views.Gestures;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Infrastructure.Helpers;
using PrankChat.Mobile.iOS.Presentation.Binding;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.ProfileView
{
    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class ProfileUpdateView : BaseTransparentBarView<ProfileUpdateViewModel>
    {
        protected override void SetupBinding()
        {
            var bindingSet = this.CreateBindingSet<ProfileUpdateView, ProfileUpdateViewModel>();

            bindingSet.Bind(emailTextField)
                      .For(v => v.Text)
                      .To(vm => vm.Email)
                      .Mode(MvxBindingMode.TwoWay);

            bindingSet.Bind(loginTextField)
                      .For(v => v.Text)
                      .To(vm => vm.Login)
                      .Mode(MvxBindingMode.TwoWay);
                
            bindingSet.Bind(nameTextField)
                      .For(v => v.Text)
                      .To(vm => vm.Name)
                      .Mode(MvxBindingMode.TwoWay);

            bindingSet.Bind(birthdayTextField)
                      .For(v => v.Text)
                      .To(vm => vm.BirthdayText);

            bindingSet.Bind(birthdayTextField.Tap())
                      .For(v => v.Command)
                      .To(vm => vm.SelectBirthdayCommand);

            bindingSet.Bind(progressBar)
                      .For(v => v.BindHidden())
                      .To(vm => vm.IsBusy)
                      .WithConversion<MvxInvertedBooleanConverter>();

            bindingSet.Bind(saveButton)
                      .To(vm => vm.SaveProfileCommand);

            bindingSet.Bind(changePasswordLabel.Tap())
                      .For(tap => tap.Command)
                      .To(vm => vm.ChangePasswordCommand);

            bindingSet.Bind(profileImage)
                      .For(v => v.ImagePath)
                      .To(vm => vm.ProfilePhotoUrl)
                      .Mode(MvxBindingMode.OneWay);

            bindingSet.Bind(profileImage.Tap())
                      .For(v => v.Command)
                      .To(vm => vm.ChangeProfilePhotoCommand);

            bindingSet.Bind(profileImage)
                      .For(v => v.PlaceholderText)
                      .To(vm => vm.ProfileShortName);

            bindingSet.Bind(changeProfilePhotoLabel.Tap())
                      .For(v => v.Command)
                      .To(vm => vm.ChangeProfilePhotoCommand);

            bindingSet.Bind(femaleIconButton)
                      .To(vm => vm.SelectGenderCommand)
                      .CommandParameter(GenderType.Female);

            bindingSet.Bind(femaleIconButton)
                      .For(UIButtonSelectedTargetBinding.TargetBinding)
                      .To(vm => vm.IsGenderFemale);

            bindingSet.Bind(maleIconButton)
                      .To(vm => vm.SelectGenderCommand)
                      .CommandParameter(GenderType.Male);

            bindingSet.Bind(maleIconButton)
                      .For(UIButtonSelectedTargetBinding.TargetBinding)
                      .To(vm => vm.IsGenderMale);

            bindingSet.Bind(descriptionTextField)
                      .For(v => v.Text)
                      .To(vm => vm.Description);

            bindingSet.Bind(emailValidationImageView)
                     .For(v => v.BindHidden())
                     .To(vm => vm.IsEmailVerified);

            bindingSet.Bind(resndEmailContainerView)
                      .For(v => v.BindHidden())
                      .To(vm => vm.IsEmailVerified)
                      .OneWay();

            bindingSet.Bind(emailValidationImageView)
                      .For(v => v.BindTap())
                      .To(vm => vm.ShowValidationWarningCommand);

            bindingSet.Bind(resendEmailLabel)
                      .For(v => v.BindTap())
                      .To(vm => vm.ResendEmailValidationCommand);

            bindingSet.Bind(emailTextField)
                      .For(FloatPlaceholderTextFieldPaddingTargetBinding.EndPadding)
                      .To(vm => vm.IsEmailVerified)
                      .WithConversion(BoolToIntConverter.Name, Tuple.Create(0, 45));

            bindingSet.Bind(resendEmailLabel)
                      .For(v => v.Alpha)
                      .To(vm => vm.CanResendEmailValidation)
                      .WithConversion(BoolToFloatConverter.Name, Tuple.Create(1f, 0.5f));

            bindingSet.Apply();
        }

        protected override void SetupControls()
        {
            Title = Resources.ProfileUpdateView_Title;

            NavigationItem?.SetRightBarButtonItem(NavigationItemHelper.CreateBarButton("ic_logout", ViewModel.ShowMenuCommand), true);

            emailTextField.SetLightStyle(Resources.ProfileUpdateView_Email_Placeholder);

            loginTextField.SetLightStyle(Resources.ProfileUpdateView_Login_Placeholder);

            nameTextField.SetLightStyle(Resources.ProfileUpdateView_Name_Placeholder);

            birthdayTextField.SetLightStyle(Resources.ProfileUpdateView_Birthday_Placeholder, rightImage: UIImage.FromBundle("ic_calendar"));

            sexSelectTitleLabel.Text = Resources.ProfileUpdateView_GenderSelect_Title;
            sexSelectTitleLabel.TextColor = Theme.Color.White;
            sexSelectTitleLabel.Font = Theme.Font.RegularFontOfSize(14);

            maleTitleButton.SetTitle(Resources.RegistrationView_Male_Button, UIControlState.Normal);
            maleTitleButton.SetRadioTitleStyle();

            femaleTitleButton.SetTitle(Resources.RegistrationView_Female_Button, UIControlState.Normal);
            femaleTitleButton.SetRadioTitleStyle();

            femaleIconButton.SetSelectableImageStyle("ic_radio_button_inactive", "ic_radio_button_active");
            maleIconButton.SetSelectableImageStyle("ic_radio_button_inactive", "ic_radio_button_active");

            saveButton.SetLightStyle(Resources.ProfileUpdateView_Button_Save);
            resendEmailLabel.SetLinkStyle(Theme.Color.White, Resources.Profile_Resend_Confirmation, 14);

            changePasswordLabel.AttributedText = new NSAttributedString(Resources.ProfileUpdateView_ChangePassword, underlineStyle: NSUnderlineStyle.Single);
            changePasswordLabel.TextColor = UIColor.White;

            changeProfilePhotoLabel.AttributedText = new NSAttributedString(Resources.ProfileUpdateView_PhotoChange_Title, underlineStyle: NSUnderlineStyle.Single);
            changeProfilePhotoLabel.TextColor = UIColor.White;

            descriptionTextField.SetLightStyle(Resources.ProfileUpdateView_Description_Placeholder);

            stackView.SetCustomSpacing(8, stackView.ArrangedSubviews[2]);
        }

        protected override void RegisterKeyboardDismissResponders(List<UIView> views)
        {
            views.Add(scrollView);
            base.RegisterKeyboardDismissResponders(views);
        }

        protected override void RegisterKeyboardDismissTextFields(List<UIView> viewList)
        {
            viewList.AddRange(new[] {
                emailTextField,
                loginTextField,
                nameTextField,
                birthdayTextField
            });

            base.RegisterKeyboardDismissTextFields(viewList);
        }
    }
}

