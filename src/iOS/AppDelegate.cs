using Firebase.CloudMessaging;
using Firebase.Crashlytics;
using Foundation;
using MvvmCross;
using MvvmCross.Platforms.Ios.Core;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core;
using PrankChat.Mobile.Core.BusinessServices.CrashlyticService;
using PrankChat.Mobile.Core.Presentation.Messages;
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

        private Lazy<IMvxMessenger> MvxMessenger => new Lazy<IMvxMessenger>(Mvx.IoCProvider.Resolve<IMvxMessenger>());

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
            InitializeFirebase();
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
            try
            {
                var sourceApp = sourceApplication ?? string.Empty;
                return VKSdk.ProcessOpenUrl(url, sourceApp)
                   || Facebook.CoreKit.ApplicationDelegate.SharedInstance.OpenUrl(application, url, sourceApp, annotation)
                   || base.OpenUrl(application, url, sourceApplication, annotation);
            }
            catch (Exception ex)
            {
                if (Mvx.IoCProvider.TryResolve<ICrashlyticsService>(out var crashlyticsService))
                {
                    crashlyticsService.TrackError(ex);
                }

                return false;
            }
        }

        public override void WillEnterForeground(UIApplication application)
        {
            base.WillEnterForeground(application);

            MvxMessenger.Value.Publish(new EnterForegroundMessage(this));
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
            {
                Firebase.Core.App.Configure();
            }

            Crashlytics.Configure();
        }
    }
}