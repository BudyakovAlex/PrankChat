﻿using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Timer;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.FilterTypes;
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items;
using PrankChat.Mobile.Core.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Profile
{
    public class ProfileViewModel : BaseProfileViewModel
    {
        private readonly IVideoPlayerService _videoPlayerService;
        private readonly IWalkthroughsProvider _walkthroughsProvider;

        public ProfileViewModel(IVideoPlayerService videoPlayerService, IWalkthroughsProvider walkthroughsProvider)
        {
            _videoPlayerService = videoPlayerService;
            _walkthroughsProvider = walkthroughsProvider;

            Items = new MvxObservableCollection<OrderItemViewModel>();

            ShowWithdrawalCommand = new MvxAsyncCommand(ShowWithdrawalAsync);
            ShowRefillCommand = new MvxAsyncCommand(ShowRefillAsync);
            ShowSubscriptionsCommand = new MvxAsyncCommand(ShowSubscriptionsAsync);
            ShowSubscribersCommand = new MvxAsyncCommand(ShowSubscribersAsync);
            ShowWalkthrouthCommand = new MvxAsyncCommand(ShowWalkthrouthAsync);
            LoadProfileCommand = new MvxAsyncCommand(() => ExecutionStateWrapper.WrapAsync(LoadProfileAsync));
            ShowUpdateProfileCommand = new MvxAsyncCommand(ShowUpdateProfileAsync);

            Messenger.SubscribeOnMainThread<RefreshNotificationsMessage>(async (msg) => await NotificationBageViewModel.RefreshDataCommand.ExecuteAsync(null)).DisposeWith(Disposables);
            Messenger.Subscribe<TimerTickMessage>(OnTimerTick, MvxReference.Strong).DisposeWith(Disposables);
            Messenger.SubscribeOnMainThread<OrderChangedMessage>((msg) => ReloadItemsCommand?.Execute()).DisposeWith(Disposables);
            Messenger.SubscribeOnMainThread<TabChangedMessage>(TabChanged).DisposeWith(Disposables);
            Messenger.SubscribeOnMainThread<SubscriptionChangedMessage>((msg) => LoadProfileCommand.Execute()).DisposeWith(Disposables);
            Messenger.SubscribeOnMainThread<EnterForegroundMessage>((msg) => ReloadItemsCommand?.Execute()).DisposeWith(Disposables);
        }

        private ProfileOrderType _selectedOrderType;
        public ProfileOrderType SelectedOrderType
        {
            get => _selectedOrderType;
            set => SetProperty(ref _selectedOrderType, value, OnSelectedOrderTypeChanged);
        }

        private string _price;
        public string Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }

        private string _ordersValue;
        public string OrdersValue
        {
            get => _ordersValue;
            set => SetProperty(ref _ordersValue, value);
        }

        private string _completedOrdersValue;
        public string CompletedOrdersValue
        {
            get => _completedOrdersValue;
            set => SetProperty(ref _completedOrdersValue, value);
        }

        private string _subscribersValue;
        public string SubscribersValue
        {
            get => _subscribersValue;
            set => SetProperty(ref _subscribersValue, value);
        }

        private string _subscriptionsValue;
        public string SubscriptionsValue
        {
            get => _subscriptionsValue;
            set => SetProperty(ref _subscriptionsValue, value);
        }

        public MvxObservableCollection<OrderItemViewModel> Items { get; }

        public IMvxAsyncCommand ShowWalkthrouthCommand { get; }

        public IMvxAsyncCommand ShowRefillCommand { get; }

        public IMvxAsyncCommand ShowSubscriptionsCommand { get; }

        public IMvxAsyncCommand ShowSubscribersCommand { get; }

        public IMvxAsyncCommand ShowWithdrawalCommand { get; }

        public IMvxAsyncCommand LoadProfileCommand { get; }

        public IMvxAsyncCommand ShowUpdateProfileCommand { get; }

        public override async Task InitializeAsync()
        {
            SelectedOrderType = ProfileOrderType.MyOrdered;
            await base.InitializeAsync();
            await LoadProfileCommand.ExecuteAsync();
        }

        public override void ViewDisappearing()
        {
            _videoPlayerService.Pause();
            base.ViewDisappearing();
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            _videoPlayerService.Play();
        }

        private void OnSelectedOrderTypeChanged()
        {
            Items.Clear();
            LoadProfileCommand.Cancel();
            LoadProfileCommand.Execute();
        }

        private async Task ShowSubscribersAsync()
        {
            var navigationParameters = new SubscriptionsNavigationParameter(SubscriptionTabType.Subscribers, SettingsService.User.Id, SettingsService.User.Name);
            var shouldRefresh = await NavigationService.ShowSubscriptionsView(navigationParameters);
            if (!shouldRefresh)
            {
                return;
            }

            await LoadProfileCommand.ExecuteAsync();
        }

        private async Task ShowSubscriptionsAsync()
        {
            var navigationParameters = new SubscriptionsNavigationParameter(SubscriptionTabType.Subscriptions, SettingsService.User.Id, SettingsService.User.Name);
            var shouldRefresh = await NavigationService.ShowSubscriptionsView(navigationParameters);
            if (!shouldRefresh)
            {
                return;
            }

            await LoadProfileCommand.ExecuteAsync();
        }

        private async Task ShowRefillAsync()
        {
            var isReloadNeeded = await NavigationService.ShowRefillView();
            if (!isReloadNeeded)
            {
                return;
            }

            await LoadProfileCommand.ExecuteAsync();
        }

        private async Task ShowWithdrawalAsync()
        {
            var isReloadNeeded = await NavigationService.ShowWithdrawalView();
            if (!isReloadNeeded)
            {
                return;
            }

            await LoadProfileCommand.ExecuteAsync();
        }

        private void TabChanged(TabChangedMessage msg)
        {
            if (msg.TabType != MainTabType.Profile)
            {
                return;
            }

            LoadProfileCommand.Execute();
        }

        private Task ShowWalkthrouthAsync()
        {
            return _walkthroughsProvider.ShowWalthroughAsync<ProfileViewModel>();
        }

        private async Task LoadProfileAsync()
        {
            await ApiService.GetCurrentUserAsync();
            Reset();

            await InitializeProfileData();
        }

        private async Task ShowUpdateProfileAsync()
        {
            var isUpdated = await NavigationService.ShowUpdateProfileView();
            if (!isUpdated)
            {
                return;
            }

            await LoadProfileCommand.ExecuteAsync();
        }

        protected override async Task InitializeProfileData()
        {
            await base.InitializeProfileData();

            if (!IsUserSessionInitialized)
            {
                return;
            }

            var user = SettingsService.User;
            ProfilePhotoUrl = user.Avatar;
            Price = user.Balance.ToPriceString();
            OrdersValue = user.OrdersExecuteCount.ToCountString();
            CompletedOrdersValue = user.OrdersExecuteFinishedCount.ToCountString();
            SubscribersValue = user.SubscribersCount.ToCountString();
            SubscriptionsValue = user.SubscriptionsCount.ToCountString();

            LoadMoreItemsAsync().FireAndForget();
        }

        protected override async Task<int> LoadMoreItemsAsync(int page = 1, int pageSize = Constants.Pagination.DefaultPaginationSize)
        {
            var items = await GetOrdersAsync(page, pageSize);
            return SetList(items, page, ProduceOrderItemViewModel, Items);
        }

        protected virtual async Task<PaginationModel<OrderDataModel>> GetOrdersAsync(int page, int pageSize)
        {
            switch (SelectedOrderType)
            {
                case ProfileOrderType.MyOrdered:
                    return await ApiService.GetOrdersAsync(OrderFilterType.MyOrdered, page, pageSize);

                case ProfileOrderType.OrdersCompletedByMe:
                    return await ApiService.GetOrdersAsync(OrderFilterType.MyCompletion, page, pageSize);
            }

            return new PaginationModel<OrderDataModel>();
        }

        private OrderItemViewModel ProduceOrderItemViewModel(OrderDataModel order)
        {
            return new OrderItemViewModel(NavigationService,
                                          SettingsService,
                                          Messenger,
                                          order,
                                          GetFullScreenVideoDataModels);
        }

        protected override int SetList<TDataModel, TApiModel>(PaginationModel<TApiModel> dataModel, int page, Func<TApiModel, TDataModel> produceItemViewModel, MvxObservableCollection<TDataModel> items)
        {
            SetTotalItemsCount(dataModel.TotalCount);
            var viewModels = dataModel.Items.Select(produceItemViewModel).ToList();

            if (page > 1)
            {
                items.AddRange(viewModels);
            }
            else
            {
                items.SwitchTo(viewModels);
            }

            return viewModels.Count;
        }

        private List<FullScreenVideoDataModel> GetFullScreenVideoDataModels()
        {
            return Items.Where(item => item.CanPlayVideo)
                        .Select(item => item.GetFullScreenVideoDataModel())
                        .ToList();
        }
    }
}
