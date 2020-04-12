using System;
using Android.App;
using Android.Runtime;
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

            Xamarin.Essentials.Platform.Init(this);
            VKUtil.GetCertificateFingerprint(this, PackageName);
            VKSdk.Initialize(this);
        }
    }
}