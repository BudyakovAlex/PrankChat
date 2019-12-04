using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.iOS.AppTheme;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Base
{
    public abstract class BaseTransparentBarView<TMvxViewModel> : BaseView<TMvxViewModel> where TMvxViewModel : BaseViewModel
    {
        protected virtual bool HasLogoInNavigationBar => true;

        protected override void SetCommonStyles()
        {
            NavigationController?.NavigationBar.SetTransparentStyle();
            View.SetGradientBackground();
            SetNeedsStatusBarAppearanceUpdate();

            if (HasLogoInNavigationBar)
            {
                var logoImageView = new UIImageView(UIImage.FromBundle("ic_logo"), null);
                NavigationItem.TitleView = logoImageView;
            }
            else
            {
                NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes()
                {
                    ForegroundColor = UIColor.White
                };
            }

            base.SetCommonStyles();
        }
    }
}
