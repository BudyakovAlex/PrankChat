using Android.App;
using Android.Content.PM;
using Android.Runtime;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.Platforms.Android.Views;
using PrankChat.Mobile.Core;

namespace PrankChat.Mobile.Droid
{
    [Activity(Label = "PrankChat",
        MainLauncher = true,
        NoHistory = true,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxSplashScreenActivity<MvxAndroidSetup<App>, App>
    {
        public SplashScreen() : base(Resource.Layout.splash_screen_layout)
        {
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
