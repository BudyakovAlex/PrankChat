using PrankChat.Mobile.Core.ViewModels.Abstract;
using PrankChat.Mobile.iOS.AppTheme;

namespace PrankChat.Mobile.iOS.Presentation.Views.Base
{
    public abstract class BaseGradientBarView<TMvxViewModel> : BaseView<TMvxViewModel> where TMvxViewModel : BasePageViewModel
    {
        protected override void SetCommonStyles()
        {
            NavigationController?.NavigationBar.SetGradientStyle();
            SetNeedsStatusBarAppearanceUpdate();

            base.SetCommonStyles();
        }
    }
}
