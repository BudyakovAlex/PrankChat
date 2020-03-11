using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Firebase.Messaging;
using MvvmCross;
using PrankChat.Mobile.Core.ApplicationServices.Notifications;
using PrankChat.Mobile.Core.ApplicationServices.Settings;

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
            //var title = string.Empty;
            //var body = string.Empty;
            //var url = string.Empty;

            //if (_intercomPushClient.IsIntercomPush(remoteMessage.Data))
            //{
            //	_intercomPushClient.HandlePush(Application, remoteMessage.Data);

            //	remoteMessage.Data.TryGetValue("title", out title);
            //	remoteMessage.Data.TryGetValue("message", out body);
            //	remoteMessage.Data.TryGetValue("uri", out url);
            //}
            //else
            //{
            //	remoteMessage.Data.TryGetValue("pinpoint.notification.title", out title);
            //	remoteMessage.Data.TryGetValue("pinpoint.notification.body", out body);
            //	remoteMessage.Data.TryGetValue("pinpoint.deeplink", out url);
            //}

            //ScheduleLocalNotification();

            //NotificationWrapper.ScheduleLocalNotification();
        }
	}
}
