using System;
using Android.App;
using Android.Content;
using Android.Gms.Common;
using Android.Media;
using Android.OS;
using Android.Support.V4.App;
using Firebase.Messaging;
using MvvmCross;
using MvvmCross.Logging;
using PrankChat.Mobile.Core.Infrastructure;

namespace PrankChat.Mobile.Droid.PlatformBusinessServices.Notifications
{
    public class NotificationWrapper
    {
        private NotificationCompat.Builder _notificationBuilder;

        public static NotificationWrapper Instance { get; } = new NotificationWrapper();

        public void Initialize()
        {
            if (IsPlayServicesAvailable())
            {
                CreateNotificationChannel();
            }
        }

        private bool IsPlayServicesAvailable()
        {
            var resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(Application.Context);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                {
                    Mvx.IoCProvider.Resolve<IMvxLog>().Error(GoogleApiAvailability.Instance.GetErrorString(resultCode), nameof(NotificationWrapper));
                }
                else
                {
                    Mvx.IoCProvider.Resolve<IMvxLog>().Error("This device is not supported", nameof(NotificationWrapper));
                }
                return false;
            }
            return true;
        }

        public void ScheduleLocalNotification(string title, string message)
        {
            //var pendingIntent = GetPendingIntent(data);

            var defaultSoundUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);

            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                _notificationBuilder = new NotificationCompat.Builder(Application.Context);
            }
            else
            {
                _notificationBuilder = new NotificationCompat.Builder(Application.Context, CreateNotificationChannel());
            }

            _notificationBuilder.SetSmallIcon(Resource.Mipmap.ic_launcher);
            _notificationBuilder.SetContentTitle(title);
            _notificationBuilder.SetContentText(message);
            _notificationBuilder.SetAutoCancel(true);
            _notificationBuilder.SetSound(defaultSoundUri);
            //_notificationBuilder.SetContentIntent(pendingIntent);
            var notificationManager = NotificationManager.FromContext(Application.Context);
            notificationManager.Notify(0, _notificationBuilder.Build());
        }

        /// <summary>
        /// Creates the notification channel.
        /// </summary>
        // https://docs.microsoft.com/en-us/xamarin/android/app-fundamentals/notifications/local-notifications-walkthrough
        private string CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
                return null;

            // DEVNOTE: android 8+ use different notificationmanager from appcompat and create a required channel
            var notificationManager = (NotificationManager)Application.Context.GetSystemService(Context.NotificationService);

            if (notificationManager == null)
                return null;

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
    }
}
