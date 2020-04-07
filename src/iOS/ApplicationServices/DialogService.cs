using System;
using System.Linq;
using System.Threading.Tasks;
using CoreFoundation;
using CoreGraphics;
using MvvmCross.Base;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.iOS.Controls;
using PrankChat.Mobile.iOS.Presentation.Dialogs.DatePicker;
using UIKit;

namespace PrankChat.Mobile.iOS.ApplicationServices
{
    public class DialogService : BaseDialogService
    {
        private const double ToastAnimationDuration = 0.5d;
        private const double ToastDuration = 2d;

        private readonly IMvxMainThreadAsyncDispatcher _dispatcher;

        public DialogService(INavigationService navigationService, IMvxMainThreadAsyncDispatcher dispatcher)
            : base(navigationService)
        {
            _dispatcher = dispatcher;
        }

        public override Task<DateTime?> ShowDateDialogAsync(DateTime? initialDateTime = null)
        {
            var keyWindow = UIApplication.SharedApplication.KeyWindow;
            if (keyWindow == null)
            {
                return null;
            }
            var topViewController = GetTopViewController(keyWindow);

            var tcs = new TaskCompletionSource<DateTime?>();
            _dispatcher.ExecuteOnMainThreadAsync(() =>
            {
                var resultAction = new Action<DateTime?>((selectedDate) =>
                {
                    topViewController.DismissModalViewController(true);
                    tcs.TrySetResult(selectedDate);
                });

                var datePicker = new DatePickerController(initialDateTime ?? DateTime.Now, resultAction)
                {
                    ModalTransitionStyle = UIModalTransitionStyle.CrossDissolve,
                    ModalPresentationStyle = UIModalPresentationStyle.Custom,
                };
                topViewController.PresentViewController(datePicker, true, null);
            });
            return tcs.Task;
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