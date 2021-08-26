using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.PasswordRecovery;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.PasswordRecoveryView
{
    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class FinishPasswordRecoveryView : BaseTransparentBarView<FinishPasswordRecoveryViewModel>
    {
        protected override void Bind()
        {
            using var bindingSet = this.CreateBindingSet<FinishPasswordRecoveryView, FinishPasswordRecoveryViewModel>();

            bindingSet.Bind(confirmButton).To(vm => vm.ShowLoginCommand);
            bindingSet.Bind(showPublicationButton).To(vm => vm.ShowPublicationCommand);
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

            confirmButton.SetLightStyle(Resources.FinishPasswordRecoveryView_GoToLogin_Button);

            showPublicationButton.SetTitle(Resources.FinishPasswordRecoveryView_GoToFeed_Button, UIControlState.Normal);
            showPublicationButton.SetTitleColor(Theme.Color.White, UIControlState.Normal);
            showPublicationButton.SetLinkStyle(Theme.Font.RegularFontOfSize(16));
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.NavigationItem.LeftBarButtonItem = null;
        }
    }
}

