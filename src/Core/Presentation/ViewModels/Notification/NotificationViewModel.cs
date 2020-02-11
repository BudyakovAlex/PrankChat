using System;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.Presentation.Navigation;
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
                                     IDialogService dialogService)
            : base(navigationService, errorHandleService, apiService, dialogService)
        {
        }

        public override Task Initialize()
        {
            return UpdateNotificationsCommand.ExecuteAsync();
        }

        private async Task OnUpdateNotificationsAsync()
        {
            try
            {
                IsBusy = true;

                var notificationsDataModel = await ApiService.GetNotificationsAsync();

                if (notificationsDataModel == null)
                    return;

                var notifications = notificationsDataModel.Data.Select(x => new NotificationItemViewModel(NavigationService, x));

                Items.SwitchTo(notifications);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
