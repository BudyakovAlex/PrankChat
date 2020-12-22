﻿using Android.App;
using Android.Content.PM;
using Android.OS;
using AndroidX.AppCompat.App;
using MvvmCross.Platforms.Android.Views;
using PrankChat.Mobile.Droid.PlatformBusinessServices.Notifications;

namespace PrankChat.Mobile.Droid
{
    [Activity(Label = "PrankChat",
        MainLauncher = true,
        NoHistory = true,
        Theme = "@style/Theme.PrankChat.Splash",
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxSplashScreenActivity
    {
        private int? _orderId;

        public SplashScreen() : base(Resource.Layout.splash_screen_layout)
        {
        }

        protected override void OnCreate(Bundle bundle)
        {
            AppCompatDelegate.CompatVectorFromResourcesEnabled = true;

            base.OnCreate(bundle);

            if (bundle == null &&                Intent != null)            {
                _orderId = NotificationWrapper.GetOrderId(Intent);
            }

            if (IsTaskRoot)
            {
                return;
            }

            Finish();
            Core.ApplicationServices.Notifications.NotificationManager.Instance.TryNavigateToView(_orderId);
        }

        protected override object GetAppStartHint(object hint = null)
        {
            return base.GetAppStartHint(_orderId);
        }
    }
}
