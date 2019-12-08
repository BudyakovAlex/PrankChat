using System;
using MvvmCross.Binding.BindingContext;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.PasswordRecovery;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.PasswordRecoveryView
{
    public partial class FinishPasswordRecoveryView : BaseTransparentBarView<FinishPasswordRecoveryViewModel>
    {
        protected override void SetupBinding()
        {
            var set = this.CreateBindingSet<FinishPasswordRecoveryView, FinishPasswordRecoveryViewModel>();

            set.Bind(confirmButton)
                .To(vm => vm.FinishRecoveringPasswordCommand);

            set.Apply();
        }

        protected override void SetupControls()
        {
            Title = Resources.Password_Recovery_View_Title;

            titleLabel.Text = Resources.Password_Recovery_View_Attention;
            titleLabel.TextColor = Theme.Color.White;
            titleLabel.Font = Theme.Font.RegularFontOfSize(14);

            messageLabel.Text = Resources.Password_Recovery_View_Finish_Text;
            messageLabel.TextColor = Theme.Color.White;
            messageLabel.Font = Theme.Font.RegularFontOfSize(14);

            confirmButton.SetLightStyle(Resources.RegistrationView_GoToFeed_Button);
        }
    }
}

