﻿using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages;
using PrankChat.Mobile.Core.ApplicationServices.Notifications;
using PrankChat.Mobile.Core.ApplicationServices.Timer;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Managers.Common;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile;
using PrankChat.Mobile.Core.Providers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels
{
    public class MainViewModel : BasePageViewModel
    {
        private readonly IVersionManager _versionManager;
        private readonly IPushNotificationProvider _notificationService;
        private readonly IWalkthroughsProvider _walkthroughsProvider;

        private readonly int[] _skipTabIndexesInDemoMode = new[] { 2, 4 };

        private int _lastSelectedTab;

        public MvxAsyncCommand ShowContentCommand { get; }

        public MvxAsyncCommand ShowLoginCommand { get; }

        public IMvxAsyncCommand<int> CheckDemoCommand { get; }

        public IMvxAsyncCommand<int> ShowWalkthrouthCommand { get; set; }

        public IMvxAsyncCommand<int> ShowWalkthrouthIfNeedCommand { get; set; }

        public IMvxCommand<int> SendTabChangedCommand { get; }

        public IMvxAsyncCommand CheckActualAppVersionCommand { get; }

        private IDisposable _refreshTokenExpiredMessageSubscription;

        public MainViewModel(IVersionManager versionManager,
                             IPushNotificationProvider notificationService,
                             IWalkthroughsProvider walkthroughsProvider)
        {
            _versionManager = versionManager;
            _notificationService = notificationService;
            _walkthroughsProvider = walkthroughsProvider;

            ShowContentCommand = new MvxAsyncCommand(NavigationService.ShowMainViewContent);
            ShowLoginCommand = new MvxAsyncCommand(NavigationService.ShowLoginView);
            CheckDemoCommand = new MvxAsyncCommand<int>(CheckDemoModeAsync);
            ShowWalkthrouthCommand = new MvxAsyncCommand<int>(ShowWalthroughAsync);
            ShowWalkthrouthIfNeedCommand = new MvxAsyncCommand<int>(ShowWalthroughIfNeedAsync);
            SendTabChangedCommand = new MvxCommand<int>(SendTabChanged);
            CheckActualAppVersionCommand = new MvxAsyncCommand(CheckActualAppVersionAsync);

            _refreshTokenExpiredMessageSubscription = Messenger.Subscribe<RefreshTokenExpiredMessage>(RefreshTokenExpired, MvxReference.Strong).DisposeWith(Disposables);
            Messenger.SubscribeOnMainThread<RefreshNotificationsMessage>(async (msg) => await NotificationBageViewModel.RefreshDataCommand.ExecuteAsync(null)).DisposeWith(Disposables);
            Messenger.Subscribe<TimerTickMessage>(OnTimerTick, MvxReference.Strong).DisposeWith(Disposables);
        }

        private async Task CheckActualAppVersionAsync()
        {
            if (SettingsService.IsDebugMode)
            {
                return;
            }

            var newActualVersion = await _versionManager.CheckAppVersionAsync();
            if (!string.IsNullOrEmpty(newActualVersion?.Link))
            {
                await NavigationService.ShowMaintananceView(newActualVersion.Link);
                return;
            }
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            CheckActualAppVersionCommand.Execute();
        }

        public override void ViewCreated()
        {
            base.ViewCreated();
            _notificationService.TryUpdateTokenAsync().FireAndForget();
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
            if (_lastSelectedTab == position)
            {
                return;
            }

            _lastSelectedTab = position;
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

        private void RefreshTokenExpired(RefreshTokenExpiredMessage _)
        {
            Disposables.Remove(_refreshTokenExpiredMessageSubscription);
            _refreshTokenExpiredMessageSubscription.Dispose();
            NavigationService.Logout().FireAndForget();
        }
    }
}