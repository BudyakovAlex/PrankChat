using System.Collections.Generic;
using CoreGraphics;
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
using PrankChat.Mobile.iOS.Presentation.Binding;
using PrankChat.Mobile.iOS.Presentation.Converters;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.ProfileView
{
    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class ProfileUpdateView : BaseTransparentBarView<ProfileUpdateViewModel>
    {
        protected override void SetupBinding()
        {
            var set = this.CreateBindingSet<ProfileUpdateView, ProfileUpdateViewModel>();

            set.Bind(emailTextField)
                .For(v => v.Text)
                .To(vm => vm.Email)
                .Mode(MvxBindingMode.TwoWay);

            set.Bind(loginTextField)
                .For(v => v.Text)
                .To(vm => vm.Login)
                .Mode(MvxBindingMode.TwoWay);

            set.Bind(nameTextField)
                .For(v => v.Text)
                .To(vm => vm.Name)
                .Mode(MvxBindingMode.TwoWay);

            set.Bind(birthdayTextField)
                .For(v => v.Text)
                .To(vm => vm.BirthdayText);

            set.Bind(birthdayTextField.Tap())
                .For(v => v.Command)
                .To(vm => vm.SelectBirthdayCommand);

            set.Bind(progressBar)
                .For(v => v.BindHidden())
                .To(vm => vm.IsBusy)
                .WithConversion<MvxInvertedBooleanConverter>();

            set.Bind(saveButton)
                .To(vm => vm.SaveProfileCommand);

            set.Bind(changePasswordLabel.Tap())
                .For(tap => tap.Command)
                .To(vm => vm.ChangePasswordCommand);

            set.Bind(profileImage)
                .For(v => v.ImagePath)
                .To(vm => vm.ProfilePhotoUrl)
                .Mode(MvxBindingMode.OneWay);

            set.Bind(profileImage.Tap())
                .For(v => v.Command)
                .To(vm => vm.ChangeProfilePhotoCommand);

            set.Bind(profileImage)
                .For(v => v.PlaceholderText)
                .To(vm => vm.ProfileShortName);

            set.Bind(changeProfilePhotoLabel.Tap())
                .For(v => v.Command)
                .To(vm => vm.ChangeProfilePhotoCommand);

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

            set.Bind(descriptionTextField)
                .For(v => v.Text)
                .To(vm => vm.Description);

            set.Apply();
        }

        protected override void SetupControls()
        {
            Title = Resources.ProfileUpdateView_Title;

            emailTextField.SetLightStyle(Resources.ProfileUpdateView_Email_Placeholder);

            loginTextField.SetLightStyle(Resources.ProfileUpdateView_Login_Placeholder);

            nameTextField.SetLightStyle(Resources.ProfileUpdateView_Name_Placeholder);

            birthdayTextField.SetLightStyle(Resources.ProfileUpdateView_Birthday_Placeholder);
            var imageView = new UIImageView(UIImage.FromBundle("ic_calendar"));
            var imageContainer = new UIView(new CGRect(0, 0, 35, 22))
            {
                ContentMode = UIViewContentMode.Center
            };
            imageContainer.AddSubview(imageView);
            birthdayTextField.RightView = imageContainer;
            birthdayTextField.RightViewMode = UITextFieldViewMode.Always;

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

            changePasswordLabel.AttributedText = new NSAttributedString(Resources.ProfileUpdateView_ChangePassword, underlineStyle: NSUnderlineStyle.Single);
            changePasswordLabel.TextColor = UIColor.White;

            changeProfilePhotoLabel.AttributedText = new NSAttributedString(Resources.ProfileUpdateView_PhotoChange_Title, underlineStyle: NSUnderlineStyle.Single);
            changeProfilePhotoLabel.TextColor = UIColor.White;

            descriptionTextField.SetLightStyle(Resources.ProfileUpdateView_Description_Placeholder);
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

