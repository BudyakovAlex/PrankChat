﻿using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Api.Base;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Notifications
{
    public interface INotificationsService
    {
        Task<BaseBundleApiModel<NotificationApiModel>> GetNotificationsAsync();

        Task MarkNotificationsAsReadedAsync();

        Task<int> GetUnreadNotificationsCountAsync();

        Task SendNotificationTokenAsync(string token);

        Task UnregisterNotificationsAsync();
    }
}