using System;
using System.Threading.Tasks;
using Android.App;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross.Base;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Droid.Extensions;
using PrankChat.Mobile.Droid.Presentation.Views.Base;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using PrankChat.Mobile.Droid.Extensions;

namespace PrankChat.Mobile.Droid.ApplicationServices
{
    public class DialogService : BaseDialogService
    {
        private readonly IMvxAndroidCurrentTopActivity _topActivity;
        private readonly IMvxMainThreadAsyncDispatcher _mvxMainThreadAsyncDispatcher;

        public DialogService(INavigationService navigationService, IMvxAndroidCurrentTopActivity topActivity, IMvxMainThreadAsyncDispatcher mvxMainThreadAsyncDispatcher)
             : base(navigationService)
        {
            _topActivity = topActivity;
            _mvxMainThreadAsyncDispatcher = mvxMainThreadAsyncDispatcher;
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

        public override Task<DateTime?> ShowDateDialogAsync(DateTime? initialDateTime = null)
        {
            _topActivity.Activity.HideKeyboard();
            var selectedDate = initialDateTime ?? DateTime.Now;

            var taskCompletionSource = new TaskCompletionSource<DateTime?>();
            _mvxMainThreadAsyncDispatcher.ExecuteOnMainThreadAsync(() =>
            {
                var dateEvent = new EventHandler<DatePickerDialog.DateSetEventArgs>((s, e) =>
                {
                    taskCompletionSource.TrySetResult(e.Date);
                });

                var datePicker = new DatePickerDialog(_topActivity.Activity, Resource.Style.Theme_PrankChat_DateDialog, dateEvent, selectedDate.Year, selectedDate.Month, selectedDate.Day);
                datePicker.CancelEvent += (s, e) =>
                {
                    taskCompletionSource.TrySetResult(null);
                };
                datePicker.Show();

            });
            return taskCompletionSource.Task;
        }

        private int GetToastYOffset(MvxAppCompatActivity activity)
        {
            var toolbar = GetToolbar(activity);
            if (toolbar == null)
            {
                return 0;
            }

            (_, var y) = toolbar.GetLocationInWindow();
            var yOffset = toolbar.Parent.Parent is CoordinatorLayout coordinator ? coordinator.Height - y : y;
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