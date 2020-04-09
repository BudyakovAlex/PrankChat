using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
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
            base.OnCreate(bundle);

            if (bundle == null)            {
                _orderId = NotificationWrapper.GetOrderId(Intent);
            }

            if (IsTaskRoot)
                return;

            Finish();

            TryNavigateToView();
        }

        protected override object GetAppStartHint(object hint = null)
        {
            return base.GetAppStartHint(_orderId);
        }

        private void TryNavigateToView()
        {
            if (_orderId == null)
                return;

            Mvx.IoCProvider.CallbackWhenRegistered<INavigationService>(() =>
            {
                try
                {
                    if (!Mvx.IoCProvider.CanResolve<INavigationService>())
                        return;

                    var navigationService = Mvx.IoCProvider.Resolve<INavigationService>();
                    navigationService.ShowOrderDetailsView(_orderId.Value);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
        }
    }
}
