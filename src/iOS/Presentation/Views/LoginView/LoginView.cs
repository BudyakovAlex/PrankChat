using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Infrastructure.Helpers;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.LoginView
{
    [MvxRootPresentation(WrapInNavigationController = true)]
    public partial class LoginView : BaseTransparentBarView<LoginViewModel>
    {
        protected override void SetupBinding()
		{
			var set = this.CreateBindingSet<LoginView, LoginViewModel>();
			set.Bind(loginButton).To(vm => vm.LoginCommand);
			set.Bind(registrationButton).To(vm => vm.RegistrationCommand);
			set.Bind(resetPasswordButton).To(vm => vm.ResetPasswordCommand);
            set.Bind(emailTextField).To(vm => vm.EmailText);
            set.Bind(passwordTextField).To(vm => vm.PasswordText);
            set.Bind(vkButton).To(vm => vm.LoginCommand).CommandParameter("Vk");
            set.Bind(okButton).To(vm => vm.LoginCommand).CommandParameter("Ok");
            set.Bind(facebookButton).To(vm => vm.LoginCommand).CommandParameter("Facebook");
            set.Bind(gmailButton).To(vm => vm.LoginCommand).CommandParameter("Gmail");
            set.Apply();
		}

		protected override void SetupControls()
		{
            NavigationController?.NavigationBar.SetTransparentStyle();

            var tapGesture = new UITapGestureRecognizer(DismissKeyboard);
            View.AddGestureRecognizer(tapGesture);

            var logoImageView = new UIImageView(UIImage.FromBundle("ic_logo"), null);
            var backButton = NavigationItemHelper.CreateBarButton("ic_back", ViewModel.GoBackCommand);
            NavigationItem.LeftBarButtonItems = new UIBarButtonItem[]
            {
                //backButton,
            };

            NavigationItem.TitleView = logoImageView;

            loginTitleLabel.Text = Resources.LoginView_Login_Title;
            loginTitleLabel.TextColor = Theme.Color.White;
            loginTitleLabel.Font = Theme.Font.RegularFontOfSize(20);

            emailTextField.Placeholder = Resources.LoginView_Email_Placeholder;
            emailTextField.SetLightStyle();

            passwordTextField.Placeholder = Resources.LoginView_Password_Placeholder;
            passwordTextField.SecureTextEntry = true;
            passwordTextField.SetLightStyle();

            forgotPasswordTitleLabel.Text = Resources.LoginView_ForgotPassword_Title;
            forgotPasswordTitleLabel.Font = Theme.Font.RegularFontOfSize(12);
            forgotPasswordTitleLabel.TextColor = Theme.Color.White;

            resetPasswordButton.SetTitle(Resources.LoginView_ForgotPassword_Button, UIControlState.Normal);
            resetPasswordButton.SetTitleColor(Theme.Color.White, UIControlState.Normal);
            resetPasswordButton.SetLinkStyle(Theme.Font.RegularFontOfSize(12));

            loginButton.SetTitle(Resources.LoginView_Continue_Button, UIControlState.Normal);
            loginButton.SetLightStyle();

            socialNetworksTitleLabel.Text = Resources.LoginView_AltLogin_Title;
            socialNetworksTitleLabel.Font = Theme.Font.RegularFontOfSize(12);
            socialNetworksTitleLabel.TextColor = Theme.Color.White;

            vkButton.SetImage(UIImage.FromBundle("ic_vk").ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal), UIControlState.Normal);
            okButton.SetImage(UIImage.FromBundle("ic_ok").ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal), UIControlState.Normal);
            facebookButton.SetImage(UIImage.FromBundle("ic_facebook").ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal), UIControlState.Normal);
            gmailButton.SetImage(UIImage.FromBundle("ic_gmail").ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal), UIControlState.Normal);

            registrationButton.SetTitle(Resources.LoginView_CreateAccount_Button, UIControlState.Normal);
            registrationButton.SetTitleColor(Theme.Color.White, UIControlState.Normal);
            registrationButton.SetLinkStyle(Theme.Font.RegularFontOfSize(16));
        }

        private void DismissKeyboard()
        {
            emailTextField.ResignFirstResponder();
            passwordTextField.ResignFirstResponder();
        }
    }
}

