using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.ViewModels.PasswordRecovery;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Views.PasswordRecoveryView
{
    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class FinishPasswordRecoveryView : BaseTransparentBarViewController<FinishPasswordRecoveryViewModel>
    {
        protected override void Bind()
        {
            using var bindingSet = this.CreateBindingSet<FinishPasswordRecoveryView, FinishPasswordRecoveryViewModel>();

            bindingSet.Bind(confirmButton).To(vm => vm.ShowLoginCommand);
            bindingSet.Bind(showPublicationButton).To(vm => vm.ShowPublicationCommand);
        }

        protected override void SetupControls()
        {
            Title = Resources.PasswordRecovery;

            titleLabel.Text = Resources.Attention;
            titleLabel.TextColor = Theme.Color.White;
            titleLabel.Font = Theme.Font.RegularFontOfSize(14);

            messageLabel.Text = Resources.PasswordRecoveryOnMail;
            messageLabel.TextColor = Theme.Color.White;
            messageLabel.Font = Theme.Font.RegularFontOfSize(14);

            confirmButton.SetLightStyle(Resources.LoginToAccount);

            showPublicationButton.SetTitle(Resources.GoToFeed, UIControlState.Normal);
            showPublicationButton.SetTitleColor(Theme.Color.White, UIControlState.Normal);
            showPublicationButton.SetLinkStyle(Theme.Font.RegularFontOfSize(16));
        }

        protected override void SetCommonStyles()
        {
            View.SetGradientBackground();
            base.SetCommonStyles();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            NavigationItem.LeftBarButtonItem = null;
        }
    }
}

