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

namespace PrankChat.Mobile.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : MvxApplicationDelegate<Setup, App>, IUNUserNotificationCenterDelegate, IMessagingDelegate
    {
        //TODO: move it to config
        public const string VkAppId = "7343996";

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
            HandleBackgroundNotification(userInfo);
        }

        [Export("userNotificationCenter:willPresentNotification:withCompletionHandler:")]
        public void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            HandleForegroundNotification(notification.Request.Content.UserInfo);
        }

        private void InitializeFirebase()
        {
            Firebase.Core.App.Configure();
            Crashlytics.Configure();
        }

        private void InitializePushNotification()
        {
            UNUserNotificationCenter.Current.Delegate = this;
            Messaging.SharedInstance.Delegate = this;

            InstanceId.Notifications.ObserveTokenRefresh(TokenRefreshNotification);

            // iOS 10 or later
            var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
            UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) =>
            {
                if (granted)
                    InvokeOnMainThread(() => UIApplication.SharedApplication.RegisterForRemoteNotifications());
            });

            var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
            var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
            UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            UIApplication.SharedApplication.RegisterForRemoteNotifications();
        }

        private void TokenRefreshNotification(object sender, NSNotificationEventArgs e)
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

        /// <summary>
        /// Handles foreground notifications.
        /// </summary>
        private void HandleForegroundNotification(NSDictionary userInfo)
        {
            var payload = HandleNotificationPayload(userInfo);
            ShowLocalNotification(payload.title, payload.body);
        }

        private void HandleBackgroundNotification(NSDictionary userInfo)
        {
            var payload = HandleNotificationPayload(userInfo);
            //TryNavigateToSignalDetails(payload.signalId);
        }

        private void ShowLocalNotification(string title, string body)
        {
            NotificationWrapper.Instance.ScheduleLocalNotification(title, body);
        }

        private (string body, string title) HandleNotificationPayload(NSDictionary userInfo)
        {
            if (!(userInfo["aps"] is NSDictionary apsDictionary))
            {
                return (string.Empty, string.Empty);
            }

            var body = string.Empty;
            var title = string.Empty;
            if (apsDictionary["alert"] is NSDictionary)
            {
                var alertDictionary = apsDictionary["alert"] as NSDictionary;

                if (alertDictionary.ContainsKey(new NSString("title")))
                {
                    title = alertDictionary["title"].ToString();
                }

                if (alertDictionary.ContainsKey(new NSString("body")))
                {
                    body = alertDictionary["body"].ToString();
                }
            }
            else
            {
                body = apsDictionary["alert"].ToString();
            }

            return (title, body);
        }
    }
}