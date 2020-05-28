using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using MvvmCross;
using MvvmCross.Droid.Support.V7.AppCompat;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Droid.PlatformBusinessServices.Notifications;

namespace PrankChat.Mobile.Droid
{
    [Activity(Label = "PrankChat",
        MainLauncher = true,
        NoHistory = true,
        Theme = "@style/Theme.PrankChat.Splash",
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxSplashScreenAppCompatActivity
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
                return;

            Finish();
            Core.ApplicationServices.Notifications.NotificationManager.Instance.TryNavigateToView(_orderId);
        }

        protected override object GetAppStartHint(object hint = null)
        {
            return base.GetAppStartHint(_orderId);
        }
    }
}
