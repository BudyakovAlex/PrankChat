using System;
using MvvmCross.Binding.BindingContext;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.Core.Presentation.ViewModels.PasswordRecovery;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.PasswordRecoveryView
{
	public partial class PasswordRecoveryView : BaseTransparentBarView<PasswordRecoveryViewModel>
	{
		protected override void SetupBinding()
		{
            var set = this.CreateBindingSet<PasswordRecoveryView, PasswordRecoveryViewModel>();

            set.Bind(emailEditText)
                .To(vm => vm.Email);

            set.Bind(recoverPasswordButton)
                .To(vm => vm.RecoverPasswordCommand);

            set.Apply();
        }

		protected override void SetupControls()
		{
            titleLabel.Text = Resources.Password_Recovery_View_Title; ;
            titleLabel.TextColor = Theme.Color.White;
            titleLabel.Font = Theme.Font.RegularFontOfSize(20);

            emailEditText.Placeholder = Resources.Password_Recovery_View_Email_Placeholder;
            emailEditText.SetLightStyle();

            recoverPasswordButton.SetLightStyle(Resources.RegistrationView_GoToFeed_Button);
        }
	}
}

