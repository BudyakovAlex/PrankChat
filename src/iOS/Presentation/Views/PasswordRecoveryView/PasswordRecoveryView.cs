using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.ViewModels.PasswordRecovery;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;

namespace PrankChat.Mobile.iOS.Presentation.Views.PasswordRecoveryView
{
    [MvxModalPresentation(WrapInNavigationController = true)]
	public partial class PasswordRecoveryView : BaseTransparentBarView<PasswordRecoveryViewModel>
	{
		protected override void Bind()
		{
            using var bindingSet = this.CreateBindingSet<PasswordRecoveryView, PasswordRecoveryViewModel>();

            bindingSet.Bind(emailEditText).To(vm => vm.Email);
            bindingSet.Bind(recoverPasswordButton).To(vm => vm.RecoverPasswordCommand);
            bindingSet.Bind(progresBar).For(v => v.BindHidden()).To(vm => vm.IsBusy).WithConversion<MvxInvertedBooleanConverter>();
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

