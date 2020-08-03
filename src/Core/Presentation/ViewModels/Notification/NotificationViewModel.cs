using Badge.Plugin;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.ViewModels.Notification.Items;
using PrankChat.Mobile.Core.Presentation.ViewModels.Shared;
using System.Linq;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Notification
{
    public class NotificationViewModel : PaginationViewModel
    {
        public NotificationViewModel() : base(Constants.Pagination.DefaultPaginationSize)
        {
            Items = new MvxObservableCollection<NotificationItemViewModel>();
        }

        public MvxObservableCollection<NotificationItemViewModel> Items { get; }

        public override async Task Initialize()
        {
            await LoadMoreItemsCommand.ExecuteAsync();
            _ = MarkReadedNotificationsAsync();
        }

        protected override async Task<int> LoadMoreItemsAsync(int page = 1, int pageSize = 20)
        {
            var pageContainer = await ApiService.GetNotificationsAsync();
            var count = SetList(pageContainer, page, ProduceNotificationItem, Items);
            return count;
        }

        private NotificationItemViewModel ProduceNotificationItem(NotificationDataModel notificationDataModel)
        {
            return new NotificationItemViewModel(NavigationService, SettingsService, notificationDataModel);
        }

        private async Task MarkReadedNotificationsAsync()
        {
            await Task.Delay(Constants.Delays.MillisecondsDelayBeforeMarkAsReaded);

            Items.ForEach(item => item.IsDelivered = true);
            await ApiService.MarkNotificationsAsReadedAsync();

            Messenger.Publish(new RefreshNotificationsMessage(this));
            CrossBadge.Current.ClearBadge();
        }
    }
}