using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Firebase;
using MvvmCross.Droid.Support.V7.AppCompat;

namespace PrankChat.Mobile.Droid
{
    [Activity(Label = "PrankChat",
        MainLauncher = true,
        NoHistory = true,
        Theme = "@style/Theme.PrankChat.Splash",
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxSplashScreenAppCompatActivity
    {
        public SplashScreen() : base(Resource.Layout.splash_screen_layout)
        {
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            UserDialogs.Init(this);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, bundle);
            InitializeFirebase();
        }

        private void InitializeFirebase()
        {
            Fabric.Fabric.With(this, new Crashlytics.Crashlytics());
            FirebaseApp.InitializeApp(this);
            Crashlytics.Crashlytics.HandleManagedExceptions();
        }
    }
}
