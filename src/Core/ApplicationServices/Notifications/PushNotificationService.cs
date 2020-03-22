using System;
using System.Threading.Tasks;
using MvvmCross.Logging;
using Newtonsoft.Json;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Network.JsonSerializers;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;

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

        public async Task<bool> TryUpdateTokenAsync()
        {
            if (SettingsService.IsPushTokenSend)
            {
                return false;
            }

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

        public static PushNotificationData GenerateNotificationData(string key, string value)
        {
            var notificationDataModel = JsonConvert.DeserializeObject<DataApiModel<NotificationApiModel>>(value, JsonNetSerializer.Settings);
            if (notificationDataModel?.Data != null)
            {
                return new PushNotificationData(notificationDataModel?.Data?.Title, notificationDataModel?.Data?.Description);
            }

            var notificationType = key?.ToEnum<NotificationType>();
            if (notificationType == null)
            {
                return new PushNotificationData(string.Empty, "Вам пришло новое уведомление");
            }

            string body;
            switch (notificationType)
            {
                case NotificationType.OrderEvent:
                    body = "У Вас новое уведомление о заказе";
                    break;

                case NotificationType.WalletEvent:
                    body = " У Вас новое уведомление по платежным операциям";
                    break;

                case NotificationType.SubscriptionEvent:
                    body = "На Вас подписались";
                    break;

                case NotificationType.LikeEvent:
                    body = "Вашу публикацию оценили";
                    break;

                case NotificationType.CommentEvent:
                    body = "Вам оставили комментарий";
                    break;

                case NotificationType.ExecutorEvent:
                    body = "У Вас новое уведомление о заказе";
                    break;

                default:
                    body = "Вам пришло новое уведомление";
                    break;
            }

            return new PushNotificationData(string.Empty, body);
        }
    }
}
