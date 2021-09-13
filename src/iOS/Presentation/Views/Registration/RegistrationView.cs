using System.Collections.Generic;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.ViewModels.Registration;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using PrankChat.Mobile.iOS.Common;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Registration
{
    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class RegistrationView : BaseTransparentBarView<RegistrationViewModel>
    {
		protected override void Bind()
		{
			using var bindingSet = this.CreateBindingSet<RegistrationView, RegistrationViewModel>();

			bindingSet.Bind(nextStepButton).To(vm => vm.ShowSecondStepCommand);
            bindingSet.Bind(emailTextField).To(vm => vm.Email);
            bindingSet.Bind(showLoginButton).To(vm => vm.CloseCommand);
            bindingSet.Bind(vkButton).To(vm => vm.LoginCommand)
                      .CommandParameter(nameof(LoginType.Vk));
            bindingSet.Bind(okButton).To(vm => vm.LoginCommand)
                .CommandParameter(nameof(LoginType.Ok));
            bindingSet.Bind(facebookButton).To(vm => vm.LoginCommand)
                .CommandParameter(nameof(LoginType.Facebook));
		}

		protected override void SetupControls()
		{
            registrationTitleLabel.Text = Resources.Registration;
            registrationTitleLabel.TextColor = Theme.Color.White;
            registrationTitleLabel.Font = Theme.Font.RegularFontOfSize(20);

            emailTextField.SetLightStyle(Resources.EnterEmail);

            socialNetworksLabel.Text = Resources.EnterSocialNetworks;
            socialNetworksLabel.Font = Theme.Font.RegularFontOfSize(12);
            socialNetworksLabel.TextColor = Theme.Color.White;

            vkButton.SetImage(UIImage.FromBundle(ImageNames.IconVk).ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal), UIControlState.Normal);
            okButton.SetImage(UIImage.FromBundle(ImageNames.IconOk).ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal), UIControlState.Normal);
            facebookButton.SetImage(UIImage.FromBundle(ImageNames.IconFacebook).ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal), UIControlState.Normal);
            gmailButton.SetImage(UIImage.FromBundle(ImageNames.IconGmail).ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal), UIControlState.Normal);

            yetRegisteredLabel.Text = Resources.AlreadyRegistered;
            yetRegisteredLabel.Font = Theme.Font.RegularFontOfSize(16);
            yetRegisteredLabel.TextColor = Theme.Color.White;

            showLoginButton.SetTitle(Resources.LoginToAccount, UIControlState.Normal);
            showLoginButton.SetTitleColor(Theme.Color.White, UIControlState.Normal);
            showLoginButton.SetLinkStyle(Theme.Font.RegularFontOfSize(16));

            nextStepButton.SetLightStyle(Resources.Continue);

            //TODO: uncomment when will be provided logic on vm
            okButton.Hidden = true;
            gmailButton.Hidden = true;
        }

        protected override void RegisterKeyboardDismissResponders(List<UIView> views)
        {
            views.Add(scrollView);

            base.RegisterKeyboardDismissResponders(views);
        }

        protected override void RegisterKeyboardDismissTextFields(List<UIView> viewList)
        {
            viewList.Add(emailTextField);

            base.RegisterKeyboardDismissTextFields(viewList);
        }
    }
}

