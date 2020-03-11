using System;
using Acr.UserDialogs;
using Android.App;
using Android.Runtime;
using Firebase;
using PrankChat.Mobile.Droid.PlatformBusinessServices.Notifications;
using VKontakte;
using VKontakte.Utils;

namespace PrankChat.Mobile.Droid
{
    [Application(UsesCleartextTraffic = true)]
    public class MainApplication : Application
    {
        protected MainApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

            VKUtil.GetCertificateFingerprint(this, PackageName);
            VKSdk.Initialize(this);

            UserDialogs.Init(this);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this);
            InitializeFirebase();
        }

        private void InitializeFirebase()
        {
            Fabric.Fabric.With(this, new Crashlytics.Crashlytics());
            FirebaseApp.InitializeApp(this);
            Crashlytics.Crashlytics.HandleManagedExceptions();
            NotificationWrapper.Instance.Initialize();
        }
    }
}