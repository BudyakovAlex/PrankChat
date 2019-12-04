using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.iOS.AppTheme;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Base
{
    public abstract class BaseTransparentBarView<TMvxViewModel> : BaseView<TMvxViewModel> where TMvxViewModel : BaseViewModel
    {
        protected override void SetCommonStyles()
        {
            NavigationController?.NavigationBar.SetTransparentStyle();
            View.SetGradientBackground();
            SetNeedsStatusBarAppearanceUpdate();

            View.LayoutMargins = new UIEdgeInsets(24, View.LayoutMargins.Left, View.LayoutMargins.Bottom, View.LayoutMargins.Right);

            var logoImageView = new UIImageView(UIImage.FromBundle("ic_logo"), null);
            NavigationItem.TitleView = logoImageView;

            base.SetCommonStyles();
        }
    }
}
