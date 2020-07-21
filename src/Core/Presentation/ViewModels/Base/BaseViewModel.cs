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
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Base
{
    public abstract class BaseViewModel : MvxViewModel
    {
        private int _timerThicksCount;
        private MvxSubscriptionToken _refreshNotificationsSubscriptionToken;
        private MvxSubscriptionToken _timerTickMessageToken;

        #region Services

        public INavigationService NavigationService { get; }

        public IErrorHandleService ErrorHandleService { get; }

        public IApiService ApiService { get; }

        public IDialogService DialogService { get; }

        public ISettingsService SettingsService { get; }

        public IMvxMessenger Messenger { get; }

        public ILogger Logger { get; }

        public INotificationBageViewModel NotificationBageViewModel { get; }

        #endregion

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

        public BaseViewModel(INavigationService navigationService,
                             IErrorHandleService errorHandleService,
                             IApiService apiService,
                             IDialogService dialogService,
                             ISettingsService settingsService)
        {
            NavigationService = navigationService;
            ErrorHandleService = errorHandleService;
            ApiService = apiService;
            DialogService = dialogService;
            SettingsService = settingsService;

            Mvx.IoCProvider.TryResolve<IMvxMessenger>(out var messenger);
            Mvx.IoCProvider.TryResolve<ILogger>(out var logger);
            Mvx.IoCProvider.TryResolve<INotificationBageViewModel>(out var notificationBageViewModel);

            Messenger = messenger;
            Logger = logger;
            NotificationBageViewModel = notificationBageViewModel;

            ShowNotificationCommand = new MvxRestrictedAsyncCommand(NavigationService.ShowNotificationView, restrictedCanExecute: () => IsUserSessionInitialized, handleFunc: NavigationService.ShowLoginView);
            ShowSearchCommand = new MvxAsyncCommand(NavigationService.ShowSearchView);
            GoBackCommand = new MvxAsyncCommand(GoBackAsync);
        }

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

        private void OnTimerTick(TimerTickMessage msg)
        {
            _timerThicksCount++;
            if (_timerThicksCount >= 10)
            {
                _timerThicksCount = 0;
                NotificationBageViewModel.RefreshDataCommand.ExecuteAsync(null).FireAndForget();
            }
        }
    }
}
