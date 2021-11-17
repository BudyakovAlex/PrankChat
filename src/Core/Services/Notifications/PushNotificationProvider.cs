using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Managers.Notifications;
using PrankChat.Mobile.Core.Providers.UserSession;
using Serilog;
using System;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Services.Notifications
{
    public class PushNotificationProvider : IPushNotificationProvider
    {
        private readonly INotificationsManager _notificationsManager;

        protected IUserSessionProvider UserSessionProvider { get; }

        protected ILogger Logger { get; }

        public PushNotificationProvider(INotificationsManager notificationsManager,
                                        IUserSessionProvider userSessionProvider)
        {
            _notificationsManager = notificationsManager;
            UserSessionProvider = userSessionProvider;
            Logger = this.Logger();
        }

        public async Task<bool> TryUpdateTokenAsync()
        {
            if (string.IsNullOrWhiteSpace(UserSessionProvider.PushToken))
            {
                Logger.Error("Push Token can't be null", new ArgumentNullException());
                return false;
            }

            if (UserSessionProvider.User == null)
            {
                Logger.Error("User can't be null", new ArgumentNullException());
                return false;
            }

            try
            {
                await _notificationsManager.SendNotificationTokenAsync(UserSessionProvider.PushToken);
                UserSessionProvider.IsPushTokenSend = true;
                return true;
            }
            catch (Exception)
            {
                UserSessionProvider.IsPushTokenSend = false;
                return false;
            }
        }

        public Task UnregisterNotificationsAsync()
        {
           return _notificationsManager.UnregisterNotificationsAsync();
        }
    }
}
