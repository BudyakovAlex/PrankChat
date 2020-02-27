using System;
using System.Linq;
using CoreFoundation;
using CoreGraphics;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.iOS.Controls;
using UIKit;

namespace PrankChat.Mobile.iOS.ApplicationServices
{
    public class DialogService : BaseDialogService
    {
        private const double ToastAnimationDuration = 0.5d;
        private const double ToastDuration = 2d;

        public DialogService(INavigationService navigationService)
            : base(navigationService)
        {
        }

        public override void ShowToast(string text, ToastType toastType)
        {
            var keyWindow = UIApplication.SharedApplication.KeyWindow;
            if (keyWindow == null)
            {
                return;
            }

            var topViewController = GetTopViewController(keyWindow);

            var toast = ToastView.Create(text, toastType);
            toast.TranslatesAutoresizingMaskIntoConstraints = false;

            var superview = topViewController.View;
            superview.AddSubview(toast);

            NSLayoutConstraint.ActivateConstraints(new[]
            {
                toast.LeadingAnchor.ConstraintEqualTo(superview.LeadingAnchor),
                toast.TopAnchor.ConstraintEqualTo(superview.SafeAreaLayoutGuide.TopAnchor),
                toast.TrailingAnchor.ConstraintEqualTo(superview.TrailingAnchor)
            });

            var y = toast.Frame.Y;
            toast.Frame = new CGRect(toast.Frame.X, y - toast.Frame.Height, toast.Frame.Width, toast.Frame.Height);

            UIView.Animate(ToastAnimationDuration, () =>
            {
                toast.Frame = new CGRect(toast.Frame.X, y, toast.Frame.Width, toast.Frame.Height);
            });

            var time = new DispatchTime(DispatchTime.Now, TimeSpan.FromSeconds(ToastDuration));
            DispatchQueue.MainQueue.DispatchAfter(time, () => toast.RemoveFromSuperview());
        }

        public static UIViewController GetTopViewController(UIWindow window)
        {
            var rootViewController = window.RootViewController;
            while (rootViewController.PresentedViewController != null)
            {
                rootViewController = rootViewController.PresentedViewController;
            }

            if (rootViewController is UINavigationController navController)
            {
                rootViewController = navController.ViewControllers.LastOrDefault();
            }
            return rootViewController;
        }
    }
}