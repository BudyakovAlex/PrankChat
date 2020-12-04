using MvvmCross.Logging;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Managers.Notifications;
using System;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Notifications
{
    public class PushNotificationProvider : IPushNotificationProvider
    {
        private readonly INotificationsManager _notificationsManager;

        protected ISettingsService SettingsService { get; }

        protected IMvxLog MvxLog { get; }

        public PushNotificationProvider(INotificationsManager notificationsManager,
                                        ISettingsService settingsService,
                                        IMvxLog mvxLog)
        {
            _notificationsManager = notificationsManager;
            SettingsService = settingsService;
            MvxLog = mvxLog;
        }

        public async Task<bool> TryUpdateTokenAsync()
        {
            if (string.IsNullOrWhiteSpace(SettingsService.PushToken))
            {
                MvxLog.ErrorException("Push Token can't be null", new ArgumentNullException());
                return false;
            }

            if (SettingsService.User == null)
            {
                MvxLog.ErrorException("User can't be null", new ArgumentNullException());
                return false;
            }

            try
            {
                await _notificationsManager.SendNotificationTokenAsync(SettingsService.PushToken);
                SettingsService.IsPushTokenSend = true;
                return true;
            }
            catch (Exception)
            {
                SettingsService.IsPushTokenSend = false;
                return false;
            }
        }

        public Task UnregisterNotificationsAsync()
        {
           return _notificationsManager.UnregisterNotificationsAsync();
        }
    }
}
