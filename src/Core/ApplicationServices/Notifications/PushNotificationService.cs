using System;
using System.Threading.Tasks;
using MvvmCross.Logging;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;

namespace PrankChat.Mobile.Core.ApplicationServices.Notifications
{
    public class PushNotificationService : IPushNotificationService
    {
        protected IApiService ApiService { get; }

        protected ISettingsService SettingsService { get; }

        protected IMvxLog MvxLog { get; }

        public PushNotificationService(IApiService apiService,
                                       ISettingsService settingsService,
                                       IMvxLog mvxLog)
        {
            ApiService = apiService;
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
                await ApiService.SendNotificationTokenAsync(SettingsService.PushToken);
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
           return ApiService.UnregisterNotificationsAsync();
        }
    }
}
