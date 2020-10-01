using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.ApplicationServices.Timer;
using PrankChat.Mobile.Core.BusinessServices.Logger;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.Navigation;
using System.Linq;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Base
{
    public abstract class BaseViewModel : MvxViewModel
    {
        private int _timerThicksCount;
        private MvxSubscriptionToken _refreshNotificationsSubscriptionToken;
        private MvxSubscriptionToken _timerTickMessageToken;

        public BaseViewModel()
        {
            ShowNotificationCommand = new MvxRestrictedAsyncCommand(NavigationService.ShowNotificationView, restrictedCanExecute: () => IsUserSessionInitialized, handleFunc: NavigationService.ShowLoginView);
            ShowSearchCommand = new MvxAsyncCommand(NavigationService.ShowSearchView);
            GoBackCommand = new MvxAsyncCommand(GoBackAsync);
        }

        public INavigationService NavigationService => Mvx.IoCProvider.Resolve<INavigationService>();

        public IErrorHandleService ErrorHandleService => Mvx.IoCProvider.Resolve<IErrorHandleService>();

        public IApiService ApiService => Mvx.IoCProvider.Resolve<IApiService>();

        public IDialogService DialogService => Mvx.IoCProvider.Resolve<IDialogService>();

        public ISettingsService SettingsService => Mvx.IoCProvider.Resolve<ISettingsService>();

        public IMvxMessenger Messenger => Mvx.IoCProvider.Resolve<IMvxMessenger>();

        public ILogger Logger => Mvx.IoCProvider.Resolve<ILogger>();

        public INotificationBageViewModel NotificationBageViewModel => Mvx.IoCProvider.Resolve<INotificationBageViewModel>();

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public bool IsUserSessionInitialized => SettingsService.User != null;

        public MvxAsyncCommand GoBackCommand { get; }
       
        public MvxAsyncCommand ShowSearchCommand { get; }

        public IMvxAsyncCommand ShowNotificationCommand { get; }

        private Task GoBackAsync()
        {
            return NavigationService.CloseView(this);
        }

        public override async Task Initialize()
        {
            await base.Initialize();
            NotificationBageViewModel.RefreshDataCommand.ExecuteAsync(null).FireAndForget();
        }

        protected void SubscribeToNotificationsUpdates()
        {
            _refreshNotificationsSubscriptionToken = Messenger.SubscribeOnMainThread<RefreshNotificationsMessage>(async (msg) => await NotificationBageViewModel.RefreshDataCommand.ExecuteAsync(null));
            _timerTickMessageToken = Messenger.Subscribe<TimerTickMessage>(OnTimerTick, MvxReference.Strong);
        }

        protected void UnsubscribeFromNotificationsUpdates()
        {
            _refreshNotificationsSubscriptionToken?.Dispose();
            _timerTickMessageToken?.Dispose();
        }

        public virtual Task RaisePropertiesChanged(params string[] propertiesNames)
        {
            var raisePropertiesTasks = propertiesNames.Select(propertyName => RaisePropertyChanged(propertyName));
            return Task.WhenAll(raisePropertiesTasks);
        }

        private void OnTimerTick(TimerTickMessage msg)
        {
            _timerThicksCount++;
            if (_timerThicksCount >= 15)
            {
                _timerThicksCount = 0;
                NotificationBageViewModel.RefreshDataCommand.ExecuteAsync(null).FireAndForget();
            }
        }
    }
}
