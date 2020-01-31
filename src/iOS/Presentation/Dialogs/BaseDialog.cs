using System;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Dialogs
{
    public class BaseDialog<TMvxViewModel> : BaseView<TMvxViewModel> where TMvxViewModel : BaseViewModel
    {
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            SetCommonBackground(true);
        }

        public override void ViewWillDisappear(bool animated)
        {
            SetCommonBackground(false);

            base.ViewWillDisappear(animated);
        }

        private void SetCommonBackground(bool enable)
        {
            UIView.Animate(enable ? 0.4f : 0.1f, 0, UIViewAnimationOptions.AllowAnimatedContent, () => View.BackgroundColor = enable ? Theme.Color.BlackTransparentBackground : UIColor.Clear, null);
        }
    }
}
