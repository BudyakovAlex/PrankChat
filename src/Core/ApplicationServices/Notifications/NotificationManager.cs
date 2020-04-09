﻿using Newtonsoft.Json;
using PrankChat.Mobile.Core.ApplicationServices.Network.JsonSerializers;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.ApplicationServices.Notifications
{
    public class NotificationManager
    {
        public static NotificationManager Instance { get; } = new NotificationManager();

        public PushNotificationData GenerateNotificationData(string key, string value, string title, string body)
        {
            var notificationType = key?.ToEnum<NotificationType>();
            if (notificationType == null)
            {
                return new PushNotificationData(title, body);
            }

            switch (notificationType)
            {
                case NotificationType.OrderEvent:
                    var orderDataModel = JsonConvert.DeserializeObject<DataApiModel<OrderApiModel>>(value, JsonNetSerializer.Settings);
                    return new PushNotificationData(title, body, notificationType.Value, orderDataModel.Data.Id);

                case NotificationType.LikeEvent:
                case NotificationType.ExecutorEvent:
                case NotificationType.CommentEvent:
                case NotificationType.SubscriptionEvent:
                default:
                    return new PushNotificationData(title, body, notificationType.Value);
            }
        }
    }
}
