﻿using Badge.Plugin;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Managers.Notifications;
using PrankChat.Mobile.Core.Presentation.ViewModels.Notification.Items;
using PrankChat.Mobile.Core.Presentation.ViewModels.Common;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Messages;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Notification
{
    public class NotificationViewModel : PaginationViewModel
    {
        private readonly INotificationsManager _notificationsManager;

        public NotificationViewModel(INotificationsManager notificationsManager) : base(Constants.Pagination.DefaultPaginationSize)
        {
            Items = new MvxObservableCollection<NotificationItemViewModel>();
            _notificationsManager = notificationsManager;
        }

        public MvxObservableCollection<NotificationItemViewModel> Items { get; }

        public override async Task InitializeAsync()
        {
            await LoadMoreItemsCommand.ExecuteAsync();
            _ = MarkReadedNotificationsAsync();
        }

        protected override async Task<int> LoadMoreItemsAsync(int page = 1, int pageSize = 20)
        {
            var pageContainer = await _notificationsManager.GetNotificationsAsync();
            var count = SetList(pageContainer, page, ProduceNotificationItem, Items);
            return count;
        }

        private NotificationItemViewModel ProduceNotificationItem(Models.Data.Notification notification)
        {
            return new NotificationItemViewModel(UserSessionProvider, notification);
        }

        private async Task MarkReadedNotificationsAsync()
        {
            await Task.Delay(Constants.Delays.MillisecondsDelayBeforeMarkAsReaded);

            Items.ForEach(item => item.IsDelivered = true);
            await _notificationsManager.MarkNotificationsAsReadedAsync();

            Messenger.Publish(new RefreshNotificationsMessage(this));
            MainThread.BeginInvokeOnMainThread(CrossBadge.Current.ClearBadge);
        }
    }
}