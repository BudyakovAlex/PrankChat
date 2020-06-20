﻿using Firebase.CloudMessaging;
using Firebase.InstanceID;
using Foundation;
using MvvmCross;
using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.Notifications;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.iOS.Delegates;
using System;
using System.Diagnostics;
using UIKit;
using UserNotifications;

namespace PrankChat.Mobile.iOS.PlatformBusinessServices.Notifications
{
    public class NotificationWrapper
    {
        public static NotificationWrapper Instance { get; } = new NotificationWrapper();

        public void TokenRefreshNotification(object sender, NSNotificationEventArgs e)
        {
            var settingService = Mvx.IoCProvider.Resolve<ISettingsService>();
            settingService.PushToken = Messaging.SharedInstance.FcmToken;

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

        public void AttachNotifications()
        {
            Messaging.SharedInstance.AutoInitEnabled = true;
            Messaging.SharedInstance.Delegate = AppDelegate.Instance;
            Messaging.SharedInstance.ShouldEstablishDirectChannel = true;

            InstanceId.Notifications.ObserveTokenRefresh(Instance.TokenRefreshNotification);

            // Register your app for remote notifications.
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // iOS 10 or later
                var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;

                // For iOS 10 display notification (sent via APNS)
                UNUserNotificationCenter.Current.Delegate = AppDelegate.Instance;

                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) =>
                {
                    if (granted)
                    {
                        Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() => UIApplication.SharedApplication.RegisterForRemoteNotifications());
                    }

                });
            }
            else
            {
                // iOS 9 or before
                var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
                var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }
        }

        public void DetachNotifications()
        {
            UNUserNotificationCenter.Current.Delegate = null;
            Messaging.SharedInstance.Delegate = null;
        }

        /// <summary>
        /// Handles foreground notifications.
        /// </summary>
        public void HandleForegroundNotification(NSDictionary userInfo)
        {
            try
            {
                var pushNotificationData = HandleNotificationPayload(userInfo);
                ScheduleLocalNotification(pushNotificationData?.Title, pushNotificationData?.Body);

                if(Mvx.IoCProvider.TryResolve<IMvxMessenger>(out var mvxMessenger))
                {
                    mvxMessenger.Publish(new RefreshNotificationsMessage(this));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public void HandleBackgroundNotification(NSDictionary userInfo)
        {
            var pushNotificationData = HandleNotificationPayload(userInfo);
            NotificationManager.Instance.TryNavigateToView(pushNotificationData?.OrderId);
        }

        public PushNotificationData HandleNotificationPayload(NSDictionary userInfo)
        {
            if (userInfo == null)
            {
                return null;
            }

            userInfo.TryGetValue(new NSString("value"), out var value);
            userInfo.TryGetValue(new NSString("key"), out var key);

            if (!(userInfo["aps"] is NSDictionary apsDictionary))
            {
                return NotificationManager.Instance.GenerateNotificationData(key?.ToString(), value?.ToString(), string.Empty, string.Empty);
            }

            var alertDictionary = apsDictionary["alert"] as NSDictionary;
            alertDictionary.TryGetValue(new NSString("title"), out var title);
            alertDictionary.TryGetValue(new NSString("body"), out var body);

            return NotificationManager.Instance.GenerateNotificationData(key?.ToString(), value?.ToString(), title?.ToString(), body?.ToString());
        }

        private void ScheduleLocalNotification(string title, string message)
        {
            var notificationCenter = UNUserNotificationCenter.Current;
            var content = new UNMutableNotificationContent
            {
                Title = title,
                Body = message,
                Sound = UNNotificationSound.Default
            };

            var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(1, false);
            var identifier = $"local_notification_{Guid.NewGuid()}";

            var request = UNNotificationRequest.FromIdentifier(identifier, content, trigger);
            UNUserNotificationCenter.Current.Delegate = new PrankChatNotificationDelegate();
            notificationCenter.AddNotificationRequest(request, (err) =>
            {
                if (err != null)
                {
                    // Report error
                    Console.WriteLine("Notification Error: {0}", err);
                }
                else
                {
                    // Report Success
                    Console.WriteLine("Notification Scheduled: {0}", request);
                }
            });
        }
    }
}
