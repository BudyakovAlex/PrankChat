﻿using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using MvvmCross.Droid.Support.V7.AppCompat;

namespace PrankChat.Mobile.Droid
{
    [Activity(Label = "PrankChat",
        MainLauncher = true,
        NoHistory = true,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxSplashScreenAppCompatActivity
    {
        public SplashScreen() : base(Resource.Layout.splash_screen_layout)
        {
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            UserDialogs.Init(this);
        }
    }
}
