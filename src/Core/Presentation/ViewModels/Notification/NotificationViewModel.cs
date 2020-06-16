using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Presentation.ViewModels.Notification.Items;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Notification
{
    public class NotificationViewModel : BaseViewModel
    {
        public MvxObservableCollection<NotificationItemViewModel> Items { get; } = new MvxObservableCollection<NotificationItemViewModel>();

        public MvxAsyncCommand UpdateNotificationsCommand => new MvxAsyncCommand(OnUpdateNotificationsAsync);

        public NotificationViewModel(INavigationService navigationService,
                                     IErrorHandleService errorHandleService,
                                     IApiService apiService,
                                     IDialogService dialogService,
                                     ISettingsService settingsService)
            : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
        }

        public override Task Initialize()
        {
            base.Initialize();
            return UpdateNotificationsCommand.ExecuteAsync();
        }

        private async Task OnUpdateNotificationsAsync()
        {
            try
            {
                IsBusy = true;

                var notifications = await ApiService.GetNotificationsAsync();
                if (notifications == null)
                {
                    return;
                }

                var notificationItems = notifications.Select(norification =>
                    new NotificationItemViewModel(NavigationService,
                                                  norification.RelatedUser,
                                                  norification.RelatedOrder,
                                                  norification.Title,
                                                  norification.Text,
                                                  norification.CreatedAt,
                                                  norification.IsDelivered,
                                                  norification.Type));

                Items.SwitchTo(notificationItems);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
