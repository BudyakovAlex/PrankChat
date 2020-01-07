using System;
using System.Drawing;
using CoreGraphics;
using Foundation;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.Presentation.ViewModels.Dialogs;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Share
{
    [MvxModalPresentation(
        ModalPresentationStyle = UIModalPresentationStyle.BlurOverFullScreen,
        ModalTransitionStyle = UIModalTransitionStyle.CoverVertical
    )]
    public partial class ShareController : MvxViewController<ShareDialogViewModel>
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewWillLayoutSubviews()
        {
            base.ViewWillLayoutSubviews();
            this.View.Superview.Bounds = new RectangleF(0, 0, (float)UIScreen.MainScreen.Bounds.Width, 178);
        }
    }
}

