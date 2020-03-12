using System.Threading.Tasks;
using Firebase.CloudMessaging;
using Firebase.Crashlytics;
using Firebase.InstanceID;
using Foundation;
using MvvmCross;
using MvvmCross.Platforms.Ios.Core;
using PrankChat.Mobile.Core;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using UIKit;
using UserNotifications;
using VKontakte;
using PrankChat.Mobile.Core.Infrastructure.Extensions;

namespace PrankChat.Mobile.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : MvxApplicationDelegate<Setup, App>, IUNUserNotificationCenterDelegate, IMessagingDelegate
    {
        //TODO: move it to config
        public const string VkAppId = "7343996";

        //public override bool WillFinishLaunching(UIApplication application, NSDictionary launchOptions)
        //{
        //    RegisterForPushNotifications();

        //    InstanceId.Notifications.ObserveTokenRefresh(TokenRefreshNotification);

        //    return base.WillFinishLaunching(application, launchOptions);
        //}

        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message)
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
        }

        public override void DidEnterBackground(UIApplication application)
        {
            // Use this method to release shared resources, save user data, invalidate timers and store the application state.
            // If your application supports background execution this method is called instead of WillTerminate when the user quits.
        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transition from background to active state.
            // Here you can undo many of the changes made on entering the background.
        }

        public override void OnActivated(UIApplication application)
        {
            // Restart any tasks that were paused (or not yet started) while the application was inactive.
            // If the application was previously in the background, optionally refresh the user interface.
            Facebook.CoreKit.AppEvents.ActivateApp();
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }

        public override void FinishedLaunching(UIApplication application)
        {
            InitializeFirebase();
            base.FinishedLaunching(application);
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            Facebook.CoreKit.Profile.EnableUpdatesOnAccessTokenChange(true);
            Facebook.CoreKit.ApplicationDelegate.SharedInstance.FinishedLaunching(application, launchOptions);
            InitializeFirebase();

            return base.FinishedLaunching(application, launchOptions);
        }

        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            return VKSdk.ProcessOpenUrl(url, sourceApplication ?? string.Empty)
               || Facebook.CoreKit.ApplicationDelegate.SharedInstance.OpenUrl(application, url, sourceApplication ?? string.Empty, annotation)
               || base.OpenUrl(application, url, sourceApplication, annotation);
        }

        private void InitializeFirebase()
        {
            Firebase.Core.App.Configure();
            Crashlytics.Configure();
        }

        private void RegisterForPushNotifications()
        {
            Messaging.SharedInstance.AutoInitEnabled = true;

            Messaging.SharedInstance.Delegate = this;

            Messaging.SharedInstance.ShouldEstablishDirectChannel = true;

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

        private void TokenRefreshNotification(object sender, NSNotificationEventArgs e)
        {
            SetPushToken().FireAndForget();
        }

        private async Task SetPushToken()
        {
            var token = await InstanceId.SharedInstance.GetInstanceIdAsync();
            if (token == null)
                return;

            var settingService = Mvx.IoCProvider.Resolve<ISettingsService>();
            settingService.PushToken = token.Token;
        }
    }
}