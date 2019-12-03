using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.iOS.AppTheme;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Base
{
    public abstract class BaseView<TMvxViewModel> : MvxViewController<TMvxViewModel> where TMvxViewModel : BaseViewModel
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetCommonStyles();
            SetupControls();
            SetupBinding();
        }

        public override UIStatusBarStyle PreferredStatusBarStyle()
        {
            return UIStatusBarStyle.BlackOpaque;
        }

        protected abstract void SetupBinding();

        protected abstract void SetupControls();

        protected virtual void SetCommonStyles()
        {
            NavigationController?.NavigationBar.SetGradientStyle();
            SetNeedsStatusBarAppearanceUpdate();
        }
    }
}
