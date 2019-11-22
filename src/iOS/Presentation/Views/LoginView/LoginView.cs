using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels;
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
			set.Apply();
		}

		protected override void SetupControls()
		{
		}
	}
}

