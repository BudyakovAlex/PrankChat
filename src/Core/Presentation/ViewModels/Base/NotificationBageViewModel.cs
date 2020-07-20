using Badge.Plugin;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Base
{
    public class NotificationBageViewModel : BaseItemViewModel, INotificationBageViewModel
    {
        private readonly IApiService _apiService;
        private readonly ISettingsService _settingsService;

        public NotificationBageViewModel(IApiService apiService, ISettingsService settingsService)
        {
            _apiService = apiService;
            _settingsService = settingsService;

            RefreshDataCommand = new MvxAsyncCommand(RefreshDataAsync);
        }

        public IMvxAsyncCommand RefreshDataCommand { get; }

        public bool HasUnreadNotifications { get; private set; }

        private async Task RefreshDataAsync()
        {
            try
            {
                IsBusy = true;

                if (_settingsService.User is null)
                {
                    return;
                }

                var unreadNotifications = await _apiService.GetUnreadNotificationsCountAsync();
                HasUnreadNotifications = unreadNotifications > 0;
                await RaisePropertyChanged(nameof(HasUnreadNotifications));

                if (HasUnreadNotifications)
                {
                    MainThread.BeginInvokeOnMainThread(() => CrossBadge.Current.SetBadge(unreadNotifications));
                }
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}