using Badge.Plugin;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Managers.Notifications;
using PrankChat.Mobile.Core.Messages;
using PrankChat.Mobile.Core.Plugins.Timer;
using PrankChat.Mobile.Core.Providers.UserSession;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.ViewModels.Abstract
{
    public class NotificationBadgeViewModel : BaseViewModel
    {
        private const int RefreshAfterSeconds = 15;

        private int _timerThicksCount;

        private readonly INotificationsManager _notificationManager;
        private readonly IUserSessionProvider _userSessionProvider;

        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

        public NotificationBadgeViewModel(INotificationsManager notificationManager, IUserSessionProvider userSessionProvider)
        {
            _notificationManager = notificationManager;
            _userSessionProvider = userSessionProvider;

            SystemTimer.SubscribeToEvent(
                OnTimerTick,
                (timer, handler) => timer.TimerElapsed += handler,
                (timer, handler) => timer.TimerElapsed -= handler).DisposeWith(Disposables);

            Messenger.SubscribeOnMainThread<RefreshNotificationsMessage>((msg) => RefreshDataCommand.Execute()).DisposeWith(Disposables);

            RefreshDataCommand = this.CreateCommand(RefreshDataAsync);
        }

        public IMvxAsyncCommand RefreshDataCommand { get; }

        public bool HasUnreadNotifications { get; private set; }

        private async Task RefreshDataAsync()
        {
            if (_userSessionProvider.User is null)
            {
                return;
            }

            await _semaphoreSlim.WrapAsync(async () =>
            {
                var unreadNotifications = await _notificationManager.GetUnreadNotificationsCountAsync();
                HasUnreadNotifications = unreadNotifications > 0;
                await RaisePropertyChanged(nameof(HasUnreadNotifications));

                if (HasUnreadNotifications)
                {
                    MainThread.BeginInvokeOnMainThread(() => CrossBadge.Current.SetBadge(unreadNotifications));
                }
            });
        }

        private void OnTimerTick(object _, EventArgs __)
        {
            _timerThicksCount++;
            if (_timerThicksCount >= RefreshAfterSeconds)
            {
                _timerThicksCount = 0;
                RefreshDataCommand.ExecuteAsync(null).FireAndForget();
            }
        }
    }
}