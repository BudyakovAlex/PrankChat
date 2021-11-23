using Android.App;
using Android.Content;
using Android.Gms.Common;
using Android.Media;
using Android.OS;
using AndroidX.Core.App;
using MvvmCross;
using MvvmCross.Logging;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Droid.PlatformBusinessServices.Notifications
{
    public class NotificationWrapper
    {
        private PendingIntent _pendingIntent;
        private NotificationCompat.Builder _notificationBuilder;

        public static NotificationWrapper Instance { get; } = new NotificationWrapper();

        public void Initialize()
        {
            if (CheckIsPlayServicesAvailable())
            {
                CreateNotificationChannel();
            }
        }

        private bool CheckIsPlayServicesAvailable()
        {
            var resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(Application.Context);
            if (resultCode != ConnectionResult.Success)
            {
                var errorMessage = GoogleApiAvailability.Instance.IsUserResolvableError(resultCode)
                    ? GoogleApiAvailability.Instance.GetErrorString(resultCode)
                    : "Firebase is not supported for this device";

                System.Diagnostics.Debug.WriteLine(errorMessage);

                return false;
            }

            return true;
        }

        public void ScheduleLocalNotification(PushNotification pushNotificationData)
        {
            if (pushNotificationData.Type == null)
            {
                var pm = Application.Context.PackageManager;
                var launchIntent = pm.GetLaunchIntentForPackage(Application.Context.PackageName);
                _pendingIntent = PendingIntent.GetActivities(Application.Context, 0, new[] { launchIntent }, 0);
            }
            else
            {
                _pendingIntent = GetPendingIntent(pushNotificationData);
            }

            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                _notificationBuilder = new NotificationCompat.Builder(Application.Context);
            }
            else
            {
                _notificationBuilder = new NotificationCompat.Builder(Application.Context, CreateNotificationChannel());
            }

            _notificationBuilder.SetSmallIcon(Resource.Mipmap.ic_launcher);
            _notificationBuilder.SetContentTitle(pushNotificationData.Title);
            _notificationBuilder.SetContentText(pushNotificationData.Body);
            _notificationBuilder.SetAutoCancel(true);
            var defaultSoundUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
            _notificationBuilder.SetSound(defaultSoundUri);
            _notificationBuilder.SetContentIntent(_pendingIntent);
            var notificationManager = NotificationManager.FromContext(Application.Context);
            notificationManager.Notify(0, _notificationBuilder.Build());
        }

        public static int? GetOrderId(Intent intent)
        {
            if (intent == null)
                return null;

            var bundle = intent.Extras;
            var bundleCollection = bundle?.KeySet();
            if (bundle == null ||
                bundleCollection == null ||
                bundleCollection.Count == 0)
                return null;

            var orderIdString = bundle.Get(Common.Constants.PushNotificationKey.OrderId)?.ToString();
            if (!string.IsNullOrEmpty(orderIdString))
            {
                int.TryParse(orderIdString, out var orderId);
                return orderId;
            }

            var key = bundle.Get("key")?.ToString();
            var value = bundle.Get("value")?.ToString();
            var title = bundle.Get("title")?.ToString();
            var body = bundle.Get("body")?.ToString();

            var pushNotificationData = Core.Services.Notifications.NotificationHandler.Instance.GenerateNotificationData(key, value, title, body);
            if (pushNotificationData?.Type == NotificationType.OrderEvent)
            {
                return pushNotificationData.OrderId;
            }

            return null;
        }

        /// <summary>
        /// Creates the notification channel.
        /// </summary>
        // https://docs.microsoft.com/en-us/xamarin/android/app-fundamentals/notifications/local-notifications-walkthrough
        private string CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                return null;
            }

            // DEVNOTE: android 8+ use different notificationmanager from appcompat and create a required channel
            var notificationManager = (NotificationManager)Application.Context.GetSystemService(Context.NotificationService);

            if (notificationManager == null)
            {
                return null;
            }

            var channelLabel = new Java.Lang.String(Resource.String.fcm_fallback_notification_channel_label.ToString());
            var importance = NotificationImportance.High;
              
            var channel = new NotificationChannel(Constants.Notification.ChannelId, channelLabel, importance)
            {
                Description = Constants.Notification.ChannelId,
                LockscreenVisibility = NotificationVisibility.Public
            };

            channel.EnableLights(false);
            channel.EnableVibration(true);

            notificationManager.CreateNotificationChannel(channel);

            return Constants.Notification.ChannelId;
        }

        private static PendingIntent GetPendingIntent(PushNotification pushNotificationData)
        {
            var intent = new Intent(Application.Context, typeof(NotificationActionService));

            var extras = new Bundle();
            extras.PutString(Common.Constants.PushNotificationKey.OrderId, pushNotificationData.OrderId.ToString());

            intent.PutExtras(extras);
            var requestCode = new Java.Util.Random().NextInt();
            var pendingIntent = PendingIntent.GetService(Application.Context, requestCode, intent, PendingIntentFlags.UpdateCurrent);
            return pendingIntent;
        }
    }
}
