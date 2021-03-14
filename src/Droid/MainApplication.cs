using Acr.UserDialogs;
using Android.App;
using Android.OS;
using Android.Runtime;
using Firebase;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using PrankChat.Mobile.Droid.PlatformBusinessServices.Notifications;
using System;
using System.Net;
using VKontakte;
using VKontakte.Utils;
using static Android.App.Application;

namespace PrankChat.Mobile.Droid
{
    [Application(UsesCleartextTraffic = true)]
    public class MainApplication : Application, IActivityLifecycleCallbacks
    {     
        protected MainApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
        }

        public void OnActivityStopped(Activity activity)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

            AppCenter.Start("0682507a-46fd-4b63-a5dd-003ddfc079bd", typeof(Analytics), typeof(Crashes));

            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            Xamarin.Essentials.Platform.Init(this);
            VKUtil.GetCertificateFingerprint(this, PackageName);
            VKSdk.Initialize(this);
           
            UserDialogs.Init(this);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this);
            InitializeFirebase();
            RegisterActivityLifecycleCallbacks(this);
        }

        private void InitializeFirebase()
        {
            FirebaseApp.InitializeApp(this);
            NotificationWrapper.Instance.Initialize();
        }
    }
}