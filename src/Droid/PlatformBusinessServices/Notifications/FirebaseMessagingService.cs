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
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Infrastructure.Extensions;

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
            message.Data.TryGetValue("key", out string key);
            message.Data.TryGetValue("value", out var value);

            var notificationType = key?.ToEnum<NotificationType>();
            if (notificationType == null)
            {
                // TODO: Implement the logic for showing push notification when the key is empty.
            }
            else
            {
                var shouldShowPush = ShouldShowPush(notificationType.Value);
                if (!shouldShowPush)
                    return;
            }

            var notificationDataModel = JsonConvert.DeserializeObject<DataApiModel<NotificationApiModel>>(value);
            if (notificationDataModel?.Data == null)
            {
                if (notificationType != null)
                {
                    // TODO: Implement the logic for showing push notification when the key is empty.
                }

                // TODO: Implement the logic for showing push notification when the key is empty.
                NotificationWrapper.Instance.ScheduleLocalNotification("empty", "empty");
            }

            NotificationWrapper.Instance.ScheduleLocalNotification(notificationDataModel?.Data?.Title, notificationDataModel?.Data?.Description);
        }

        private bool ShouldShowPush(NotificationType notificationType)
        {
            switch (notificationType)
            {
                case NotificationType.OrderEvent:
                    return true;

                case NotificationType.WalletEvent:
                    return true;

                case NotificationType.SubscriptionEvent:
                    return true;

                case NotificationType.LikeEvent:
                    return true;

                case NotificationType.CommentEvent:
                    return true;

                case NotificationType.ExecutorEvent:
                    return true;

                default:
                    // TODO: Add the error log. We dosen`t check type of notification.
                    return true;

            }
        }
    }
}
