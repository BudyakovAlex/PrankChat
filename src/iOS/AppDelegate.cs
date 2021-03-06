using Firebase.CloudMessaging;
using Firebase.InstanceID;
using Foundation;
using Microsoft.AppCenter.Crashes;
using MvvmCross.Platforms.Ios.Core;
using PrankChat.Mobile.Core;
using PrankChat.Mobile.iOS.PlatformBusinessServices.Notifications;
using System;
using UIKit;
using UserNotifications;
using VKontakte;

namespace PrankChat.Mobile.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : MvxApplicationDelegate<Setup, App>, IUNUserNotificationCenterDelegate, IMessagingDelegate
    {
        //TODO: move it to config
        public const string VkAppId = "7343996";

        private int? _orderId;

        public AppDelegate()
        {
            Instance = this;
        }

        public static AppDelegate Instance { get; private set; }

        public override void OnActivated(UIApplication application)
        {
            // Restart any tasks that were paused (or not yet started) while the application was inactive.
            // If the application was previously in the background, optionally refresh the user interface.
            Facebook.CoreKit.AppEvents.ActivateApp();
        }

        public override bool WillFinishLaunching(UIApplication application, NSDictionary launchOptions)
        {
            try
            {
                InitializeFirebase();
                InitializePushNotification();
            }
            catch (Exception exception)
            {
                Crashes.TrackError(exception);
            }

            return true;
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            try
            {
                Facebook.CoreKit.Profile.EnableUpdatesOnAccessTokenChange(true);
                Facebook.CoreKit.ApplicationDelegate.SharedInstance.FinishedLaunching(application, launchOptions);
            }
            catch (Exception exception)
            {
                Crashes.TrackError(exception);
            }

            return base.FinishedLaunching(application, launchOptions);
        }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            try
            {
                return VKSdk.ProcessOpenUrl(url, string.Empty)
                   || Facebook.CoreKit.ApplicationDelegate.SharedInstance.OpenUrl(app, url, options)
                   || base.OpenUrl(app, url, options);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                return base.OpenUrl(app, url, options);
            }
        }

        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            try
            {
                var sourceApp = sourceApplication ?? string.Empty;
                return VKSdk.ProcessOpenUrl(url, sourceApp)
                   || Facebook.CoreKit.ApplicationDelegate.SharedInstance.OpenUrl(application, url, sourceApp, annotation)
                   || base.OpenUrl(application, url, sourceApplication, annotation);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                return false;
            }
        }

        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            NotificationWrapper.Instance.HandleBackgroundNotification(userInfo);
        }

        [Export("userNotificationCenter:willPresentNotification:withCompletionHandler:")]
        public void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            NotificationWrapper.Instance.HandleForegroundNotification(notification.Request.Content.UserInfo);
        }

        protected override object GetAppStartHint(object hint = null)
        {
            if (_orderId != null)
            {
                return _orderId;
            }

            return hint;
        }

        private void InitializeFirebase()
        {
            if (Firebase.Core.App.DefaultInstance == null)
            {
                Firebase.Core.App.Configure();
            }
        }

        private void InitializePushNotification()
        {
            Messaging.SharedInstance.AutoInitEnabled = true;
            Messaging.SharedInstance.Delegate = this;
            Messaging.SharedInstance.ShouldEstablishDirectChannel = true;

            InstanceId.Notifications.ObserveTokenRefresh(NotificationWrapper.Instance.TokenRefreshNotification);

            // Register your app for remote notifications.
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // iOS 10 or later
                var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;

                // For iOS 10 display notification (sent via APNS)
                UNUserNotificationCenter.Current.Delegate = this;

                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) =>
                {
                    if (granted)
                    {
                        InvokeOnMainThread(() => UIApplication.SharedApplication.RegisterForRemoteNotifications());
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
    }
}