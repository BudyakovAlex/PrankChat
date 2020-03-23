﻿using System;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Dialogs
{
    [MvxModalPresentation(
        ModalPresentationStyle = UIModalPresentationStyle.Custom,
        ModalTransitionStyle = UIModalTransitionStyle.CrossDissolve)]
    public class BaseDialog<TMvxViewModel> : BaseView<TMvxViewModel> where TMvxViewModel : BaseViewModel
    {
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            SetCommonBackground(true);
        }

        public override void ViewWillDisappear(bool animated)
        {
            SetCommonBackground(false);
            ViewModel.ViewDestroy(true);
            base.ViewWillDisappear(animated);
        }

        private void SetCommonBackground(bool enable)
        {
            UIView.Animate(enable ? 0.4f : 0.1f, 0, UIViewAnimationOptions.AllowAnimatedContent, () => View.BackgroundColor = enable ? Theme.Color.BlackTransparentBackground : UIColor.Clear, null);
        }
    }
}
