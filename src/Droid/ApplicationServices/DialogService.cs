using System;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Droid.Extensions;
using PrankChat.Mobile.Droid.Presentation.Views.Base;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace PrankChat.Mobile.Droid.ApplicationServices
{
    public class DialogService : BaseDialogService
    {
        private readonly IMvxAndroidCurrentTopActivity _topActivity;

        public DialogService(INavigationService navigationService, IMvxAndroidCurrentTopActivity topActivity)
             : base(navigationService)
        {
            _topActivity = topActivity;
        }

        public override void ShowToast(string text, ToastType toastType)
        {
            var activity = (MvxAppCompatActivity) _topActivity.Activity;
            var yOffset = GetToastYOffset(activity);

            var inflater = activity.LayoutInflater;
            var toastView = inflater.Inflate(Resource.Layout.toast_view, null);
            var textView = toastView.FindViewById<TextView>(Resource.Id.text_view);
            textView.Text = text;
            textView.SetBackgroundResource(GetBackgroundId(toastType));

            var toast = new Toast(activity.ApplicationContext);
            toast.SetGravity(GravityFlags.Top | GravityFlags.FillHorizontal, 0, yOffset);
            toast.Duration = ToastLength.Short;
            toast.View = toastView;
            toast.Show();
        }

        private int GetToastYOffset(MvxAppCompatActivity activity)
        {
            var toolbar = GetToolbar(activity);
            if (toolbar == null)
            {
                return 0;
            }

            (_, var y) = toolbar.GetLocationInWindow();
            var coordinator = toolbar.Parent.Parent as CoordinatorLayout;
            var yOffset = coordinator != null ? coordinator.Height - y : y;
            return yOffset;
        }

        private Toolbar GetToolbar(MvxAppCompatActivity activity)
        {
            if (activity is IToolbarOwner toolbarOwner)
            {
                return toolbarOwner.Toolbar;
            }

            return null;
        }

        private int GetBackgroundId(ToastType toastType)
        {
            switch (toastType)
            {
                case ToastType.Positive: return Resource.Color.successful;
                case ToastType.Negative: return Resource.Color.unsuccessful;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}