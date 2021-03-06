using AuthenticationServices;
using MvvmCross;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.ViewModels.Registration;
using PrankChat.Mobile.iOS.Services.ExternalAuth.AppleSignIn;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Views.Base;
using System;
using System.Collections.Generic;
using UIKit;
using PrankChat.Mobile.iOS.Common;

namespace PrankChat.Mobile.iOS.Views.LoginView
{
    [MvxRootPresentation]
    public partial class LoginView : BaseTransparentBarViewController<LoginViewModel>
    {
        private readonly Lazy<IAppleSignInService> _lazyAppleSignInService = new Lazy<IAppleSignInService>(() => Mvx.IoCProvider.Resolve<IAppleSignInService>());

        private ASAuthorizationAppleIdButton _appleIdButton;
        private UITapGestureRecognizer _appleButtonTapGesture;

        protected override void Bind()
        {
            using var bindingSet = CreateBindingSet();

            bindingSet.Bind(loginButton).To(vm => vm.LoginCommand)
                      .CommandParameter(LoginType.UsernameAndPassword);
            bindingSet.Bind(registrationButton).To(vm => vm.RegistrationCommand);
            bindingSet.Bind(demoButton).To(vm => vm.ShowDemoModeCommand);
            bindingSet.Bind(resetPasswordButton).To(vm => vm.ResetPasswordCommand);
            bindingSet.Bind(emailTextField).To(vm => vm.EmailText);
            bindingSet.Bind(passwordTextField).To(vm => vm.PasswordText);
            bindingSet.Bind(progressBar).For(v => v.BindHidden()).To(vm => vm.IsBusy)
                      .WithConversion<MvxInvertedBooleanConverter>();

            bindingSet.Bind(vkButton).To(vm => vm.LoginCommand).CommandParameter(LoginType.Vk);
            bindingSet.Bind(okButton).To(vm => vm.LoginCommand).CommandParameter(LoginType.Ok);
            bindingSet.Bind(facebookButton).To(vm => vm.LoginCommand).CommandParameter(LoginType.Facebook);
            bindingSet.Bind(gmailButton).To(vm => vm.LoginCommand).CommandParameter(LoginType.Gmail);
        }

        protected override void SetupControls()
        {
            _appleIdButton = new ASAuthorizationAppleIdButton(ASAuthorizationAppleIdButtonType.Default, ASAuthorizationAppleIdButtonStyle.White)
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                UserInteractionEnabled = true,
                CornerRadius = 2
            };

            appleButtonContainer.AddSubview(_appleIdButton);
            NSLayoutConstraint.ActivateConstraints(new []
            {
                _appleIdButton.TopAnchor.ConstraintEqualTo(appleButtonContainer.TopAnchor),
                _appleIdButton.BottomAnchor.ConstraintEqualTo(appleButtonContainer.BottomAnchor),
                _appleIdButton.LeadingAnchor.ConstraintEqualTo(appleButtonContainer.LeadingAnchor),
                _appleIdButton.TrailingAnchor.ConstraintEqualTo(appleButtonContainer.TrailingAnchor),
            });

            loginTitleLabel.Text = Resources.LoginIn;
            loginTitleLabel.TextColor = Theme.Color.White;
            loginTitleLabel.Font = Theme.Font.RegularFontOfSize(20);

            emailTextField.SetLightStyle(Resources.EnterEmail);

            passwordTextField.SecureTextEntry = true;
            passwordTextField.SetLightStyle(Resources.EnterPassword);

            forgotPasswordTitleLabel.Text = Resources.ForgotPassword;
            forgotPasswordTitleLabel.Font = Theme.Font.RegularFontOfSize(12);
            forgotPasswordTitleLabel.TextColor = Theme.Color.White;

            resetPasswordButton.SetTitle(Resources.Restore, UIControlState.Normal);
            resetPasswordButton.SetTitleColor(Theme.Color.White, UIControlState.Normal);
            resetPasswordButton.SetLinkStyle(Theme.Font.RegularFontOfSize(12));

            loginButton.SetLightStyle(Resources.Continue);
            demoButton.SetLightStyle(Resources.LoginWithoutRegistration);

            socialNetworksTitleLabel.Text = Resources.EnterSocialNetworks;
            socialNetworksTitleLabel.Font = Theme.Font.RegularFontOfSize(12);
            socialNetworksTitleLabel.TextColor = Theme.Color.White;

            vkButton.SetImage(UIImage.FromBundle(ImageNames.IconVk).ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal), UIControlState.Normal);
            okButton.SetImage(UIImage.FromBundle(ImageNames.IconOk).ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal), UIControlState.Normal);
            facebookButton.SetImage(UIImage.FromBundle(ImageNames.IconFacebook).ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal), UIControlState.Normal);
            gmailButton.SetImage(UIImage.FromBundle(ImageNames.IconGmail).ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal), UIControlState.Normal);

            _appleButtonTapGesture = new UITapGestureRecognizer(OnAppleIdButtonTouched);
            _appleIdButton.AddGestureRecognizer(_appleButtonTapGesture);

            registrationButton.SetTitle(Resources.CreateNewOneAccount, UIControlState.Normal);
            registrationButton.SetTitleColor(Theme.Color.White, UIControlState.Normal);
            registrationButton.SetLinkStyle(Theme.Font.RegularFontOfSize(16));

            //TODO: uncomment when will be provided logic on vm
            okButton.Hidden = true;
            gmailButton.Hidden = true;
        }

        protected override void SetCommonStyles()
        {
            View.SetGradientBackground();
            base.SetCommonStyles();
        }

        public override void ViewDidUnload()
        {
            _appleIdButton.RemoveGestureRecognizer(_appleButtonTapGesture);
            base.ViewDidUnload();
        }

        protected override void RegisterKeyboardDismissResponders(List<UIView> views)
        {
            views.Add(scrollView);

            base.RegisterKeyboardDismissResponders(views);
        }

        protected override void RegisterKeyboardDismissTextFields(List<UIView> viewList)
        {
            viewList.Add(emailTextField);
            viewList.Add(passwordTextField);
            viewList.Add(scrollView);

            base.RegisterKeyboardDismissTextFields(viewList);
        }

        private async void OnAppleIdButtonTouched()
        {
            var appleAuthData = await _lazyAppleSignInService.Value.LoginAsync();
            if (appleAuthData is null)
            {
                return;
            }

            ViewModel.LoginWithAppleCommand?.Execute(appleAuthData);
        }
    }
}
