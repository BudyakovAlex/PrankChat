using PrankChat.Mobile.Core.ViewModels;
using PrankChat.Mobile.Core.ViewModels.Abstract;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Common;
using UIKit;

namespace PrankChat.Mobile.iOS.Views.Base
{
    public abstract class BaseTransparentBarView<TMvxViewModel> : BaseView<TMvxViewModel> where TMvxViewModel : BasePageViewModel
    {
        protected override void SetCommonStyles()
        {
            base.SetCommonStyles();

            NavigationController?.NavigationBar.SetTransparentStyle();
            View.SetGradientBackground();
            SetNeedsStatusBarAppearanceUpdate();

            View.LayoutMargins = new UIEdgeInsets(24, View.LayoutMargins.Left, View.LayoutMargins.Bottom, View.LayoutMargins.Right);

            if (string.IsNullOrWhiteSpace(Title))
            {
                var logoImageView = new UIImageView(UIImage.FromBundle(ImageNames.IconLogo), null);
                NavigationItem.TitleView = logoImageView;
            }
        }
    }
}
