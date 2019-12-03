using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.LoginView
{
    [MvxRootPresentation(WrapInNavigationController = true)]
    public partial class LoginView : BaseView<LoginViewModel>
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
            var tapGesture = new UITapGestureRecognizer(DismissKeyboard);
            View.AddGestureRecognizer(tapGesture);
        }

        private void DismissKeyboard()
        {
            emailTextField.ResignFirstResponder();
            passwordTextField.ResignFirstResponder();
        }
    }
}
