using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.PasswordRecovery;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;

namespace PrankChat.Mobile.iOS.Presentation.Views.PasswordRecoveryView
{
    [MvxModalPresentation(WrapInNavigationController = true)]
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

            emailEditText.SetLightStyle(Resources.Password_Recovery_View_Email_Placeholder);

            recoverPasswordButton.SetLightStyle(Resources.Password_Recovery_View_Recovery_Button);
        }
	}
}

