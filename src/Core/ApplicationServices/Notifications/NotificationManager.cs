using System;
using MvvmCross;
using Newtonsoft.Json;
using PrankChat.Mobile.Core.ApplicationServices.Network.JsonSerializers;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.ApplicationServices.Notifications
{
    public class NotificationManager
    {
        public static NotificationManager Instance { get; } = new NotificationManager();

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
                    var orderDataModel = JsonConvert.DeserializeObject<ResponseDto<OrderDto>>(value, JsonNetSerializer.Settings);
                    if (orderDataModel.Data.OrderCategory == OrderCategory.Standard)
                    {
                        return new PushNotification(title, body, notificationType.Value, orderDataModel.Data.Id);
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
                return;

            Mvx.IoCProvider.CallbackWhenRegistered<INavigationService>(() =>
            {
                try
                {
                    if (!Mvx.IoCProvider.CanResolve<INavigationService>())
                        return;

                    var navigationService = Mvx.IoCProvider.Resolve<INavigationService>();
                    navigationService.ShowOrderDetailsView(orderId.Value, null, 0);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
        }
    }
}
