﻿using System.Collections.Generic;
using MvvmCross.Binding.BindingContext;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Registration
{
    public partial class RegistrationView : BaseTransparentBarView<RegistrationViewModel>
    {
		protected override void SetupBinding()
		{
			var set = this.CreateBindingSet<RegistrationView, RegistrationViewModel>();

			set.Bind(nextStepButton)
                .To(vm => vm.ShowSecondStepCommand);

            set.Bind(emailTextField)
                .To(vm => vm.Email);

            set.Apply();
		}

		protected override void SetupControls()
		{
            registrationTitleLabel.Text = Resources.RegistrationView_Title;
            registrationTitleLabel.TextColor = Theme.Color.White;
            registrationTitleLabel.Font = Theme.Font.RegularFontOfSize(20);

            emailTextField.SetLightStyle(Resources.RegistrationView_Email_Placeholder);

            socialNetworksLabel.Text = Resources.LoginView_AltLogin_Title;
            socialNetworksLabel.Font = Theme.Font.RegularFontOfSize(12);
            socialNetworksLabel.TextColor = Theme.Color.White;

            vkButton.SetImage(UIImage.FromBundle("ic_vk").ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal), UIControlState.Normal);
            okButton.SetImage(UIImage.FromBundle("ic_ok").ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal), UIControlState.Normal);
            facebookButton.SetImage(UIImage.FromBundle("ic_facebook").ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal), UIControlState.Normal);
            gmailButton.SetImage(UIImage.FromBundle("ic_gmail").ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal), UIControlState.Normal);

            yetRegisteredLabel.Text = Resources.RegistrationView_RegisteredYet_Label;
            yetRegisteredLabel.Font = Theme.Font.RegularFontOfSize(16);
            yetRegisteredLabel.TextColor = Theme.Color.White;

            showLoginButton.SetTitle(Resources.RegistrationView_Login_Button, UIControlState.Normal);
            showLoginButton.SetTitleColor(Theme.Color.White, UIControlState.Normal);
            showLoginButton.SetLinkStyle(Theme.Font.RegularFontOfSize(16));

            nextStepButton.SetLightStyle(Resources.LoginView_Continue_Button);
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

