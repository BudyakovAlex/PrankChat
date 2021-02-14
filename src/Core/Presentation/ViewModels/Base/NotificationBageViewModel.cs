using Badge.Plugin;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Managers.Notifications;
using PrankChat.Mobile.Core.Providers.UserSession;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Base
{
    public class NotificationBageViewModel : BaseViewModel, INotificationBageViewModel
    {
        private readonly INotificationsManager _notificationManager;
        private readonly IUserSessionProvider _userSessionProvider;

        public NotificationBageViewModel(INotificationsManager notificationManager, IUserSessionProvider userSessionProvider)
        {
            _notificationManager = notificationManager;
            _userSessionProvider = userSessionProvider;

            RefreshDataCommand = new MvxAsyncCommand(() => ExecutionStateWrapper.WrapAsync(RefreshDataAsync));
        }

        public IMvxAsyncCommand RefreshDataCommand { get; }

        public bool HasUnreadNotifications { get; private set; }

        private async Task RefreshDataAsync()
        {
            if (_userSessionProvider.User is null)
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