﻿using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Firebase.Messaging;
using MvvmCross;
using Newtonsoft.Json;
using PrankChat.Mobile.Core.ApplicationServices.Notifications;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using MvvmCross.Logging;
using NotificationManager = PrankChat.Mobile.Core.ApplicationServices.Notifications.NotificationManager;

namespace PrankChat.Mobile.Droid.PlatformBusinessServices.Notifications
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class FirebaseMessagingService : Firebase.Messaging.FirebaseMessagingService
    {
        protected FirebaseMessagingService(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public FirebaseMessagingService()
        {
        }

        public override void OnNewToken(string token)
        {
            var settingService = Mvx.IoCProvider.Resolve<ISettingsService>();
            settingService.PushToken = token;

            try
            {
                var pushNotificationService = Mvx.IoCProvider.Resolve<IPushNotificationService>();
                pushNotificationService.TryUpdateTokenAsync().FireAndForget();
            }
            catch (Exception ex)
            {
                var log = Mvx.IoCProvider.Resolve<IMvxLog>();
                log.ErrorException("Can not resolve IPushNotificationService", ex);
            }
        }

        public override void OnMessageReceived(RemoteMessage message)
        {
            message.Data.TryGetValue("key", out string key);
            message.Data.TryGetValue("value", out var value);

            var title = message.GetNotification().Title;
            var body = message.GetNotification().Body;
            var pushNotificationData = NotificationManager.Instance.GenerateNotificationData(key, value, title, body);
            NotificationWrapper.Instance.ScheduleLocalNotification(pushNotificationData);
        }
    }
}
