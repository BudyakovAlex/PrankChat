using System.Collections.Generic;
using CoreGraphics;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Commands;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.ProfileView
{
    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class ProfileUpdateView : BaseTransparentBarView<ProfileUpdateViewModel>
    {
        public IMvxAsyncCommand OpenCalendarCommand { get; set; }

        public IMvxCommand<GenderType> SelectGenderCommand { get; set; }

        private GenderType _gender;
        public GenderType Gender
        {
            get => _gender;
            set
            {
               
            }
        }

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

            set.Bind(this)
                .For(v => v.OpenCalendarCommand)
                .To(vm => vm.SelectBirthdayCommand);

            set.Bind(this)
                .For(v => v.SelectGenderCommand)
                .To(vm => vm.SelectGenderCommand);

            set.Bind(progressBar)
                .For(v => v.BindHidden())
                .To(vm => vm.IsBusy)
                .WithConversion<MvxInvertedBooleanConverter>();

            set.Bind(saveButton)
                .To(vm => vm.ProfileUpdateCommand);

            set.Apply();
        }

        protected override void SetupControls()
        {
            this.View.BackgroundColor = UIColor.Black;

            Title = Resources.ProfileUpdateView_Title;

            emailTextField.SetLightStyle(Resources.ProfileUpdateView_Email_Placeholder);

            loginTextField.SetLightStyle(Resources.ProfileUpdateView_Login_Placeholder);

            nameTextField.SetLightStyle(Resources.ProfileUpdateView_Name_Placeholder);

            birthdayTextField.SetLightStyle(Resources.ProfileUpdateView_Birthday_Placeholder);
            var imageView = new UIImageView(UIImage.FromBundle("ic_calendar"));
            var imageContainer = new UIView(new CGRect(0, 0, 35, 22));
            imageContainer.ContentMode = UIViewContentMode.Center;
            imageContainer.AddSubview(imageView);
            birthdayTextField.RightView = imageContainer;
            birthdayTextField.RightViewMode = UITextFieldViewMode.Always;
            birthdayTextField.AddGestureRecognizer(new UITapGestureRecognizer(HandleCalendarTap));


            sexSelectTitleLabel.Text = Resources.ProfileUpdateView_GenderSelect_Title;
            sexSelectTitleLabel.TextColor = Theme.Color.White;
            sexSelectTitleLabel.Font = Theme.Font.RegularFontOfSize(14);

            maleTitleButton.SetTitle(Resources.RegistrationView_Male_Button, UIControlState.Normal);
            maleTitleButton.AddTarget((s, e) => HandleRadioTap(GenderType.Male), UIControlEvent.TouchUpInside);
            maleTitleButton.SetRadioTitleStyle();

            maleIconButton.SetRadioInactiveStyle();
            maleIconButton.AddTarget((s, e) => HandleRadioTap(GenderType.Male), UIControlEvent.TouchUpInside);

            maleButtonsContainerView.AddGestureRecognizer(new UITapGestureRecognizer(s => HandleRadioTap(GenderType.Male)));

            femaleTitleButton.SetTitle(Resources.RegistrationView_Female_Button, UIControlState.Normal);
            femaleTitleButton.SetRadioTitleStyle();
            femaleTitleButton.AddTarget((s, e) => HandleRadioTap(GenderType.Female), UIControlEvent.TouchUpInside);

            femaleIconButton.SetRadioInactiveStyle();
            femaleIconButton.AddTarget((s, e) => HandleRadioTap(GenderType.Female), UIControlEvent.TouchUpInside);

            femaleButtonsContainerView.AddGestureRecognizer(new UITapGestureRecognizer(s => HandleRadioTap(GenderType.Female)));

            saveButton.SetLightStyle(Resources.ProfileUpdateView_Button_Save);
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

        private void HandleRadioTap(GenderType sex)
        {
            switch (sex)
            {
                case GenderType.Male:
                    // Set male.
                    femaleIconButton.SetRadioInactiveStyle();
                    maleIconButton.SetRadioActiveStyle();
                    break;

                case GenderType.Female:
                    // Set female.
                    femaleIconButton.SetRadioActiveStyle();
                    maleIconButton.SetRadioInactiveStyle();
                    break;
            }
            SelectGenderCommand?.Execute(sex);
        }

        private void HandleCalendarTap(object sender)
        {
            OpenCalendarCommand?.Execute();
        }
    }
}

