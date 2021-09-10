using MvvmCross;
using Newtonsoft.Json;
using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Managers.Navigation;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.ViewModels.Order;
using PrankChat.Mobile.Core.ViewModels.Parameters;
using PrankChat.Mobile.Core.Services.Network.JsonSerializers;
using System;

namespace PrankChat.Mobile.Core.Services.Notifications
{
    public class NotificationHandler
    {
        public static NotificationHandler Instance { get; } = new NotificationHandler();

        public PushNotification GenerateNotificationData(string key, string value, string title, string body)
        {
            var notificationType = key?.ToEnum<NotificationType>();
            if (notificationType == null)
            {
                return new PushNotification(title, body);
            }

            switch (notificationType)
            {
                case NotificationType.OrderEvent:
                    var response = JsonConvert.DeserializeObject<ResponseDto<OrderDto>>(value, JsonNetSerializer.Settings);
                    if (response.Data.OrderCategory == OrderCategory.Standard)
                    {
                        return new PushNotification(title, body, notificationType.Value, response.Data.Id);
                    }

                    return new PushNotification(title, body, notificationType.Value);
                case NotificationType.LikeEvent:
                case NotificationType.ExecutorEvent:
                case NotificationType.CommentEvent:
                case NotificationType.SubscriptionEvent:
                default:
                    return new PushNotification(title, body, notificationType.Value);
            }
        }

        public void TryNavigateToView(int? orderId)
        {
            if (orderId == null)
            {
                return;
            }

            Mvx.IoCProvider.CallbackWhenRegistered<INavigationManager>(() =>
            {
                try
                {
                    if (!Mvx.IoCProvider.TryResolve<INavigationManager>(out var navigationManager))
                    {
                        return;
                    }

                    var parameter = new OrderDetailsNavigationParameter(orderId.Value, null, 0);
                    navigationManager.NavigateAsync<OrderDetailsViewModel, OrderDetailsNavigationParameter>(parameter).FireAndForget();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
        }
    }
}
