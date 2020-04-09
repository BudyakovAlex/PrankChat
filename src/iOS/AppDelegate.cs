using System.Threading.Tasks;
using Firebase.Crashlytics;
using Foundation;
using MvvmCross;
using MvvmCross.Platforms.Ios.Core;
using PrankChat.Mobile.Core;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using UIKit;
using UserNotifications;
using VKontakte;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.iOS.Delegates;
using System.Runtime.InteropServices;
using System;
using PrankChat.Mobile.Core.ApplicationServices.Notifications;
using MvvmCross.Logging;
using Firebase.InstanceID;
using Firebase.CloudMessaging;
using PrankChat.Mobile.iOS.PlatformBusinessServices;
using PrankChat.Mobile.iOS.PlatformBusinessServices.Notifications;

namespace PrankChat.Mobile.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : MvxApplicationDelegate<Setup, App>, IUNUserNotificationCenterDelegate, IMessagingDelegate
    {
        //TODO: move it to config
        public const string VkAppId = "7343996";

        private int? _orderId;

        public override void OnActivated(UIApplication application)
        {
            // Restart any tasks that were paused (or not yet started) while the application was inactive.
            // If the application was previously in the background, optionally refresh the user interface.
            Facebook.CoreKit.AppEvents.ActivateApp();
        }

        public override bool WillFinishLaunching(UIApplication application, NSDictionary launchOptions)
        {
            InitializeFirebase();
            InitializePushNotification();
            return true;
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            Facebook.CoreKit.Profile.EnableUpdatesOnAccessTokenChange(true);
            Facebook.CoreKit.ApplicationDelegate.SharedInstance.FinishedLaunching(application, launchOptions);

            return base.FinishedLaunching(application, launchOptions);
        }

        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            return VKSdk.ProcessOpenUrl(url, sourceApplication ?? string.Empty)
               || Facebook.CoreKit.ApplicationDelegate.SharedInstance.OpenUrl(application, url, sourceApplication ?? string.Empty, annotation)
               || base.OpenUrl(application, url, sourceApplication, annotation);
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
                return _orderId;

            return hint;
        }

        private void InitializeFirebase()
        {
            if (Firebase.Core.App.DefaultInstance == null)
                Firebase.Core.App.Configure();

            Crashlytics.Configure();
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
                        InvokeOnMainThread(() => UIApplication.SharedApplication.RegisterForRemoteNotifications());

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