using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Notification.Items;
using PrankChat.Mobile.Core.Presentation.ViewModels.Shared;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Notification
{
    public class NotificationViewModel : PaginationViewModel
    {
        private const int MillisecondsDelayBeforeMarkAsReaded = 3000;

        private readonly IMvxMessenger _mvxMessenger;

        public NotificationViewModel(INavigationService navigationService,
                                     IErrorHandleService errorHandleService,
                                     IApiService apiService,
                                     IDialogService dialogService,
                                     ISettingsService settingsService,
                                     IMvxMessenger mvxMessenger)
            : base(Constants.Pagination.DefaultPaginationSize, navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
            Items = new MvxObservableCollection<NotificationItemViewModel>();
            _mvxMessenger = mvxMessenger;
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

        protected override int SetList<TDataModel, TApiModel>(PaginationModel<TApiModel> dataModel, int page, Func<TApiModel, TDataModel> produceItemViewModel, MvxObservableCollection<TDataModel> items)
        {
            SetTotalItemsCount(dataModel?.TotalCount ?? 0);
            var orderViewModels = dataModel?.Items?.Select(produceItemViewModel).ToList();

            items.AddRange(orderViewModels);
            return orderViewModels.Count;
        }

        private NotificationItemViewModel ProduceNotificationItem(NotificationDataModel notificationDataModel)
        {
            return new NotificationItemViewModel(NavigationService, notificationDataModel);
        }

        private async Task MarkReadedNotificationsAsync()
        {
            await Task.Delay(MillisecondsDelayBeforeMarkAsReaded);

            Items.ForEach(item => item.IsDelivered = true);
            await ApiService.MarkNotificationsAsReadedAsync();
            _mvxMessenger.Publish(new RefreshNotificationsMessage(this));
        }
    }
}