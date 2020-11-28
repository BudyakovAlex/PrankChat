using Badge.Plugin;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Managers.Notifications;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Base
{
    public class NotificationBageViewModel : BaseViewModel, INotificationBageViewModel
    {
        private readonly INotificationsManager _notificationManager;
        private readonly ISettingsService _settingsService;

        public NotificationBageViewModel(INotificationsManager notificationManager, ISettingsService settingsService)
        {
            _notificationManager = notificationManager;
            _settingsService = settingsService;

            RefreshDataCommand = new MvxAsyncCommand(() => ExecutionStateWrapper.WrapAsync(RefreshDataAsync));
        }

        public IMvxAsyncCommand RefreshDataCommand { get; }

        public bool HasUnreadNotifications { get; private set; }

        private async Task RefreshDataAsync()
        {
            if (_settingsService.User is null)
            {
                return;
            }

            var unreadNotifications = await _notificationManager.GetUnreadNotificationsCountAsync();
            HasUnreadNotifications = unreadNotifications > 0;
            await RaisePropertyChanged(nameof(HasUnreadNotifications));

            if (HasUnreadNotifications)
            {
                MainThread.BeginInvokeOnMainThread(() => CrossBadge.Current.SetBadge(unreadNotifications));
            }
        }
    }
}