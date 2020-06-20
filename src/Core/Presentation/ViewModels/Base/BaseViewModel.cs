using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.ApplicationServices.Timer;
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

        #endregion

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public bool IsUserSessionInitialized => SettingsService.User != null;

        public MvxAsyncCommand GoBackCommand
        {
            get
            {
                return new MvxAsyncCommand(() => NavigationService.CloseView(this));
            }
        }

        public MvxAsyncCommand ShowSearchCommand
        {
            get { return new MvxAsyncCommand(() => NavigationService.ShowSearchView()); }
        }

        public IMvxAsyncCommand ShowNotificationCommand { get; }

        public bool HasUnreadNotifications { get; private set; }

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
            Messenger = messenger;

            ShowNotificationCommand = new MvxRestrictedAsyncCommand(NavigationService.ShowNotificationView, restrictedCanExecute: () => IsUserSessionInitialized, handleFunc: NavigationService.ShowLoginView);
        }

        public override async Task Initialize()
        {
            await base.Initialize();
            ValidateNotificationsStateAsync().FireAndForget();
        }

        protected void SubscribeToNotificationsUpdates()
        {
            _refreshNotificationsSubscriptionToken = Messenger.SubscribeOnMainThread<RefreshNotificationsMessage>(async (msg) => await ValidateNotificationsStateAsync());
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
                ValidateNotificationsStateAsync().FireAndForget();
            }
        }

        protected async Task ValidateNotificationsStateAsync()
        {
            if (!IsUserSessionInitialized)
            {
                return;
            }

            var unreadNotifications = await ApiService.GetUnreadNotificationsCountAsync();
            HasUnreadNotifications = unreadNotifications > 0;
            await RaisePropertyChanged(nameof(HasUnreadNotifications));
        }
    }
}
