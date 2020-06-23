using System;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Notifications;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.ApplicationServices.Timer;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile;
using PrankChat.Mobile.Core.Providers;

namespace PrankChat.Mobile.Core.Presentation.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly ISettingsService _settingsService;
        private readonly IPushNotificationService _notificationService;
        private readonly IWalkthroughsProvider _walkthroughsProvider;

        private readonly int[] _skipTabIndexesInDemoMode = new[] { 2, 4 };

        public MvxAsyncCommand ShowContentCommand { get; }

        public MvxAsyncCommand ShowLoginCommand { get; }

        public IMvxAsyncCommand<int> CheckDemoCommand { get; }

        public IMvxAsyncCommand<int> ShowWalkthrouthCommand { get; set; }

        public IMvxAsyncCommand<int> ShowWalkthrouthIfNeedCommand { get; set; }

        public IMvxCommand<int> SendTabChangedCommand { get; }

        public MainViewModel(INavigationService navigationService,
                             ISettingsService settingsService,
                             IErrorHandleService errorHandleService,
                             IApiService apiService,
                             IDialogService dialogService,
                             IPushNotificationService notificationService,
                             IWalkthroughsProvider walkthroughsProvider)
            : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
            _settingsService = settingsService;
            _notificationService = notificationService;
            _walkthroughsProvider = walkthroughsProvider;

            ShowContentCommand = new MvxAsyncCommand(NavigationService.ShowMainViewContent);
            ShowLoginCommand = new MvxAsyncCommand(NavigationService.ShowLoginView);
            CheckDemoCommand = new MvxAsyncCommand<int>(CheckDemoModeAsync);
            ShowWalkthrouthCommand = new MvxAsyncCommand<int>(ShowWalthroughAsync);
            ShowWalkthrouthIfNeedCommand = new MvxAsyncCommand<int>(ShowWalthroughIfNeedAsync);
            SendTabChangedCommand = new MvxCommand<int>(SendTabChanged);
        }

        public override void ViewCreated()
        {
            base.ViewCreated();
            _notificationService.RegisterToNotifications();
            _notificationService.TryUpdateTokenAsync().FireAndForget();
            Subscription();
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            Unsubscription();
            base.ViewDestroy(viewFinishing);
        }

        private void Subscription()
        {
            SubscribeToNotificationsUpdates();
        }

        private void Unsubscription()
        {
            UnsubscribeFromNotificationsUpdates();
        }

        public bool CanSwitchTabs(int position)
        {
            if (!IsUserSessionInitialized &&
                _skipTabIndexesInDemoMode.Contains(position))
            {
                return false;
            }

            return true;
        }

        private void SendTabChanged(int position)
        {
            Messenger.Publish(new TabChangedMessage(this, (MainTabType)position));
        }

        private Task ShowWalthroughIfNeedAsync(int position)
        {
            switch (position)
            {
                case 1 when _walkthroughsProvider.CheckCanShowOnFirstLoad<CompetitionsViewModel>():
                    return _walkthroughsProvider.ShowWalthroughAsync<CompetitionsViewModel>();
                case 2 when _walkthroughsProvider.CheckCanShowOnFirstLoad<CreateOrderViewModel>():
                    return _walkthroughsProvider.ShowWalthroughAsync<CreateOrderViewModel>();
                case 3 when _walkthroughsProvider.CheckCanShowOnFirstLoad<OrdersViewModel>():
                    return _walkthroughsProvider.ShowWalthroughAsync<OrdersViewModel>();
                case 4 when _walkthroughsProvider.CheckCanShowOnFirstLoad<ProfileViewModel>():
                    return _walkthroughsProvider.ShowWalthroughAsync<ProfileViewModel>();
                default:
                    return Task.FromResult(false);
            }
        }

        private Task ShowWalthroughAsync(int position)
        {
            switch (position)
            {
                case 1:
                    return _walkthroughsProvider.ShowWalthroughAsync<CompetitionsViewModel>();
                case 2:
                    return _walkthroughsProvider.ShowWalthroughAsync<CreateOrderViewModel>();
                case 3:
                    return _walkthroughsProvider.ShowWalthroughAsync<OrdersViewModel>();
                case 4:
                    return _walkthroughsProvider.ShowWalthroughAsync<ProfileViewModel>();
                default:
                    return Task.FromResult(false);
            }
        }

        private async Task CheckDemoModeAsync(int position)
        {
            if (!CanSwitchTabs(position))
            {
                await NavigationService.ShowLoginView();
            }
        }
    }
}