using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Firebase.Messaging;
using MvvmCross;
using Newtonsoft.Json;
using PrankChat.Mobile.Core.ApplicationServices.Notifications;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;

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
        }

        public override void OnMessageReceived(RemoteMessage message)
        {
            message.Data.TryGetValue("key", out var key);
            message.Data.TryGetValue("value", out var value);

            value = null;

            //if (string.IsNullOrWhiteSpace(value))
            //{
            //    NotificationWrapper.Instance.ScheduleLocalNotification("empty", "empty");
            //}

            var notificationDataModel = JsonConvert.DeserializeObject<DataApiModel<NotificationApiModel>>(value);
            if (notificationDataModel?.Data == null)
            {
                NotificationWrapper.Instance.ScheduleLocalNotification("empty", "empty");
            }

            NotificationWrapper.Instance.ScheduleLocalNotification(notificationDataModel?.Data?.Title, notificationDataModel?.Data?.Description);
        }
	}
}
