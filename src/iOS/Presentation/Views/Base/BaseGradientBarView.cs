using System;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.iOS.AppTheme;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Base
{
    public abstract class BaseGradientBarView<TMvxViewModel> : BaseView<TMvxViewModel> where TMvxViewModel : BaseViewModel
    {
        protected override void SetCommonStyles()
        {
            NavigationController?.NavigationBar.SetGradientStyle();
            SetNeedsStatusBarAppearanceUpdate();

            base.SetCommonStyles();
        }
    }
}
