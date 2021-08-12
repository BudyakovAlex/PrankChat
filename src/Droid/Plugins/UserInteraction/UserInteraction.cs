﻿using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using AndroidX.CoordinatorLayout.Widget;
using AndroidX.Core.Content;
using MvvmCross.Base;
using MvvmCross.Platforms.Android;
using MvvmCross.Platforms.Android.Views;
using PrankChat.Mobile.Core.Managers.Navigation;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Droid.Extensions;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

using PrankChat.Mobile.Core.Plugins.UserInteraction;
using System;
using System.Threading.Tasks;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace PrankChat.Mobile.Droid.Plugins.UserInteraction
{
    public class UserInteraction : BaseUserInteraction
    {
        private const int ViewAppearingMillisecondsDelay = 500;

        private readonly IMvxAndroidCurrentTopActivity _topActivity;
        private readonly IMvxMainThreadAsyncDispatcher _mvxMainThreadAsyncDispatcher;

        public override bool IsToastShown { get; protected set; }

        public UserInteraction(INavigationManager navigationManager, IMvxAndroidCurrentTopActivity topActivity, IMvxMainThreadAsyncDispatcher mvxMainThreadAsyncDispatcher)
             : base(navigationManager)
        {
            _topActivity = topActivity;
            _mvxMainThreadAsyncDispatcher = mvxMainThreadAsyncDispatcher;
        }

        public override void ShowToast(string text, ToastType toastType)
        {
            _ = _mvxMainThreadAsyncDispatcher.ExecuteOnMainThreadAsync(() =>
            {
                IsToastShown = true;

                var activity = (MvxActivity)_topActivity.Activity;
                var yOffset = GetToastYOffset(activity);

                var inflater = activity.LayoutInflater;
                var toastView = inflater.Inflate(Resource.Layout.toast_view, null);
                var textView = toastView.FindViewById<TextView>(Resource.Id.text_view);
                textView.Text = text;
                textView.SetBackgroundResource(GetBackgroundId(toastType));

                var toast = new Toast(activity.ApplicationContext);
                toast.SetGravity(GravityFlags.Top | GravityFlags.FillHorizontal, 0, yOffset);
                toast.Duration = ToastLength.Long;
                toast.View = toastView;
                toast.Show();
            });

            Task.Run(async () =>
            {
                await Task.Delay(3500);
                IsToastShown = false;
            });
        }

        public override async Task<DateTime?> ShowDateDialogAsync(DateTime? initialDateTime = null)
        {
            if (_topActivity.Activity is null)
            {
                await Task.Delay(ViewAppearingMillisecondsDelay);
            }

            var activity = _topActivity.Activity ?? Xamarin.Essentials.Platform.CurrentActivity;
            if (activity is null)
            {
                return null;
            }

            activity.HideKeyboard();
            var selectedDate = initialDateTime ?? DateTime.Now;

            var taskCompletionSource = new TaskCompletionSource<DateTime?>();
            _ = _mvxMainThreadAsyncDispatcher.ExecuteOnMainThreadAsync(() =>
            {
                var dateEvent = new EventHandler<DatePickerDialog.DateSetEventArgs>((s, e) =>
                {
                    taskCompletionSource.TrySetResult(e.Date);
                });

                var datePicker = new DatePickerDialog(
                    activity,
                    Resource.Style.Theme_PrankChat_DateDialog,
                    dateEvent,
                    selectedDate.Year,
                    selectedDate.Month,
                    selectedDate.Day);

                datePicker.CancelEvent += (s, e) =>
                {
                    taskCompletionSource.TrySetResult(null);
                };
                datePicker.Show();

                var positiveButton = datePicker.GetButton((int)DialogButtonType.Positive);
                var negativeButton = datePicker.GetButton((int)DialogButtonType.Negative);
                var neutralButton = datePicker.GetButton((int)DialogButtonType.Neutral);

                var textColor = ContextCompat.GetColorStateList(activity, Resource.Color.accent);
                positiveButton.SetTextColor(textColor);
                negativeButton.SetTextColor(textColor);
                neutralButton.SetTextColor(textColor);
            });

            return await taskCompletionSource.Task;
        }

        private int GetToastYOffset(MvxActivity activity)
        {
            var toolbar = GetToolbar(activity);
            if (toolbar == null)
            {
                return 0;
            }

            if (toolbar.Parent.Parent is CoordinatorLayout coordinatorLayout)
            {
                (_, var y) = toolbar.GetLocationInWindow();
                return coordinatorLayout.Height - y;
            }

            return toolbar.Height;
        }

        private Toolbar GetToolbar(MvxActivity activity)
        {
            if (activity is IToolbarOwner toolbarOwner)
            {
                return toolbarOwner.Toolbar;
            }

            return null;
        }

        private int GetBackgroundId(ToastType toastType)
        {
            return toastType switch
            {
                ToastType.Positive => Resource.Color.successful,
                ToastType.Negative => Resource.Color.unsuccessful,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
    }
}