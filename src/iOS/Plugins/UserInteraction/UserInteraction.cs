using CoreFoundation;
using CoreGraphics;
using MvvmCross;
using MvvmCross.Base;
using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.Managers.Navigation;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Plugins.UserInteraction;
using PrankChat.Mobile.Core.Services.ErrorHandling;
using PrankChat.Mobile.iOS.Controls;
using PrankChat.Mobile.iOS.Dialogs.DatePicker;
using System;
using System.Linq;
using System.Threading.Tasks;
using UIKit;

namespace PrankChat.Mobile.iOS.Plugins.UserInteraction
{
    public class UserInteraction : BaseUserInteraction
    {
        private const double ToastAnimationDuration = 0.5d;
        private const double ToastDuration = 4d;

        private readonly IMvxMainThreadAsyncDispatcher _dispatcher;
        private readonly Lazy<IErrorHandleService> _lazyErrorHandleService =
            new Lazy<IErrorHandleService>(() => Mvx.IoCProvider.Resolve<IErrorHandleService>());

        public override bool IsToastShown { get; protected set; }

        public UserInteraction(INavigationManager navigationManager, IMvxMainThreadAsyncDispatcher dispatcher)
            : base(navigationManager)
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
                    ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext,
                };
                topViewController.PresentViewController(datePicker, true, null);
            });

            return tcs.Task;
        }

        public override void ShowToast(string text, ToastType toastType)
        {
            try
            {
                IsToastShown = true;

                var keyWindow = UIApplication.SharedApplication.KeyWindow;
                if (keyWindow == null)
                {
                    IsToastShown = false;
                    return;
                }

                var topViewController = GetTopViewController(keyWindow);
                if (topViewController is UIAlertController)
                {
                    IsToastShown = false;
                    return;
                }

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

                toast.Frame = new CGRect(0f, -toast.Frame.Height, toast.Frame.Width, toast.Frame.Height);
                UIView.Animate(
                    ToastAnimationDuration,
                    () => toast.Frame = new CGRect(0f, 0f, toast.Frame.Width, toast.Frame.Height));

                DispatchQueue.MainQueue.DispatchAfter(
                    new DispatchTime(DispatchTime.Now, TimeSpan.FromSeconds(ToastDuration)),
                    () =>
                    {
                        toast.RemoveFromSuperview();
                        IsToastShown = false;
                    });
            }
            catch (Exception ex)
            {
                _lazyErrorHandleService.Value.LogError(this, "Failed to show toast", ex);
            }
        }

        public static UIViewController GetTopViewController(UIWindow window)
        {
            var topViewController = window.RootViewController;

            if (topViewController.PresentedViewController is null &&
                topViewController is MvxTabBarViewController tabBarController)
            {
                topViewController = tabBarController.VisibleUIViewController;
            }
            else
            {
                while (topViewController.PresentedViewController != null)
                {
                    topViewController = topViewController.PresentedViewController;
                }
            }

            return topViewController is UINavigationController navigationController
                ? navigationController.ViewControllers.LastOrDefault()
                : topViewController;
        }
    }
}