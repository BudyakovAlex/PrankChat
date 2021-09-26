using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.App;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.Platforms.Android.Views;
using PrankChat.Mobile.Core.Providers.Configuration;
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
        private TextureView _layerList;

        public SplashScreen() : base(0)
        {
        }

        protected override void OnCreate(Bundle bundle)
        {
            AppCompatDelegate.CompatVectorFromResourcesEnabled = true;

            RequestWindowFeature(WindowFeatures.NoTitle);
            var decorView = Window.DecorView;
            decorView.SystemUiVisibility = (StatusBarVisibility)(SystemUiFlags.Fullscreen
                | SystemUiFlags.HideNavigation
                | SystemUiFlags.Immersive
                | SystemUiFlags.ImmersiveSticky);

            base.OnCreate(bundle);
            RequestedOrientation = ScreenOrientation.FullSensor;

            Mvx.IoCProvider.CallbackWhenRegistered<IEnvironmentConfigurationProvider>(provider =>
            {
                AppCenter.Start(provider.Environment.AppCenterDroidId, typeof(Analytics), typeof(Crashes));
            });

            if (bundle == null &&                Intent != null)            {
                _orderId = NotificationWrapper.GetOrderId(Intent);
            }

            if (IsTaskRoot)
            {
                return;
            }

            Finish();
            Core.Services.Notifications.NotificationHandler.Instance.TryNavigateToView(_orderId);
        }

        protected override object GetAppStartHint(object hint = null)
        {
            return base.GetAppStartHint(_orderId);
        }
    }
}
