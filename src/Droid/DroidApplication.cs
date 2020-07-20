﻿using System;
using System.Net;
using Acr.UserDialogs;
using Android.App;
using Android.Runtime;
using Firebase;
using MvvmCross;
using PrankChat.Mobile.Core.BusinessServices.TaskSchedulers;
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

            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            Xamarin.Essentials.Platform.Init(this);
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