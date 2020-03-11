using System;
using System.Threading.Tasks;
using MvvmCross.Logging;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Infrastructure.Extensions;

namespace PrankChat.Mobile.Core.ApplicationServices.Notifications
{
    public class PushNotificationService : IPushNotificationService
    {
        protected IApiService ApiService { get; }

        protected ISettingsService SettingsService { get; }

        protected IMvxLog MvxLog { get; }

        public PushNotificationService(IApiService apiService, ISettingsService settingsService, IMvxLog mvxLog)
        {
            ApiService = apiService;
            SettingsService = settingsService;
            MvxLog = mvxLog;
        }

        public async Task UpdateToken()
        {
            if (string.IsNullOrWhiteSpace(SettingsService.PushToken))
            {
                MvxLog.ErrorException("Push Token can't be null", new ArgumentNullException());
                return;
            }

            await ApiService.SendNotificationTokenAsync(SettingsService.PushToken);
            SettingsService.IsPushnTokenSend = true;
        }
    }
}
