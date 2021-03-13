using Acr.UserDialogs;
using Android.App;
using Android.OS;
using Android.Runtime;
using Firebase;
using MvvmCross;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Droid.PlatformBusinessServices.Notifications;
using Sentry;
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
        private Lazy<IMvxMessenger> MvxMessenger => new Lazy<IMvxMessenger>(Mvx.IoCProvider.Resolve<IMvxMessenger>());

        private Activity _activityInBackground;

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
            _activityInBackground = activity;
        }

        public void OnActivityResumed(Activity activity)
        {
            if (activity is SplashScreen)
            {
                return;
            }

            if (_activityInBackground != activity)
            {
                return;
            }

            _activityInBackground = null;
            MvxMessenger.Value.Publish(new EnterForegroundMessage(this));
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

            using (SentrySdk.Init("https://077bd6b9706f4247a2bf8cab4ac3bf44@o453054.ingest.sentry.io/5454419"))
            {

            }

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