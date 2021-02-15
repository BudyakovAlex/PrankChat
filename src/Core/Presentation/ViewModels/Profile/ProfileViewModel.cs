using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Timer;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Managers.Orders;
using PrankChat.Mobile.Core.Managers.Users;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.FilterTypes;
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
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
        private readonly IOrdersManager _ordersManager;
        private readonly IVideoPlayerService _videoPlayerService;
        private readonly IWalkthroughsProvider _walkthroughsProvider;

        public ProfileViewModel(IOrdersManager ordersManager,
                                IUsersManager usersManager,
                                IVideoPlayerService videoPlayerService,
                                IWalkthroughsProvider walkthroughsProvider) : base(usersManager)
        {
            _ordersManager = ordersManager;
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
            var navigationParameters = new SubscriptionsNavigationParameter(SubscriptionTabType.Subscribers, UserSessionProvider.User.Id, UserSessionProvider.User.Name);
            var shouldRefresh = await NavigationService.ShowSubscriptionsView(navigationParameters);
            if (!shouldRefresh)
            {
                return;
            }

            await LoadProfileCommand.ExecuteAsync();
        }

        private async Task ShowSubscriptionsAsync()
        {
            var navigationParameters = new SubscriptionsNavigationParameter(SubscriptionTabType.Subscriptions, UserSessionProvider.User.Id, UserSessionProvider.User.Name);
            var shouldRefresh = await NavigationService.ShowSubscriptionsView(navigationParameters);
            if (!shouldRefresh)
            {
                return;
            }

            await LoadProfileCommand.ExecuteAsync();
        }

        private async Task ShowRefillAsync()
        {
            if (!IsEmailVerified)
            {
                var canGoProfile = await DialogService.ShowConfirmAsync(Resources.Profile_Your_Email_Not_Actual, Resources.Attention, Resources.Ok, Resources.Cancel);
                if (canGoProfile)
                {
                    await NavigationService.ShowUpdateProfileView();
                }

                return;
            }

            var isReloadNeeded = await NavigationService.ShowRefillView();
            if (!isReloadNeeded)
            {
                return;
            }

            await LoadProfileCommand.ExecuteAsync();
        }

        private async Task ShowWithdrawalAsync()
        {
            if (!IsEmailVerified)
            {
                var canGoProfile = await DialogService.ShowConfirmAsync(Resources.Profile_Your_Email_Not_Actual, Resources.Attention, Resources.Ok, Resources.Cancel);
                if (canGoProfile)
                {
                    await NavigationService.ShowUpdateProfileView();
                }

                return;
            }

            var isReloadNeeded = await NavigationService.ShowWithdrawalView();
            if (!isReloadNeeded)
            {
                return;
            }

            await LoadProfileCommand.ExecuteAsync();
        }

        private async Task ShowWalkthrouthAsync()
        {
            await _walkthroughsProvider.ShowWalthroughAsync<ProfileViewModel>();
        }

        private async Task LoadProfileAsync()
        {
            await UsersManager.GetCurrentUserAsync();
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

            var user = UserSessionProvider.User;
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

        protected virtual async Task<Pagination<Models.Data.Order>> GetOrdersAsync(int page, int pageSize)
        {
            return SelectedOrderType switch
            {
                ProfileOrderType.MyOrdered => await _ordersManager.GetOrdersAsync(OrderFilterType.MyOrdered, page, pageSize),
                ProfileOrderType.OrdersCompletedByMe => await _ordersManager.GetOrdersAsync(OrderFilterType.MyCompletion, page, pageSize),
                _ => new Pagination<Models.Data.Order>(),
            };
        }

        private OrderItemViewModel ProduceOrderItemViewModel(Models.Data.Order order)
        {
            return new OrderItemViewModel(
                NavigationService,
                UserSessionProvider,
                order,
                GetFullScreenVideos);
        }

        protected override int SetList<TDataModel, TApiModel>(Pagination<TApiModel> pagination, int page, Func<TApiModel, TDataModel> produceItemViewModel, MvxObservableCollection<TDataModel> items)
        {
            SetTotalItemsCount(pagination.TotalCount);
            var viewModels = pagination.Items.Select(produceItemViewModel).ToList();

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

        private List<FullScreenVideo> GetFullScreenVideos()
        {
            return Items.Where(item => item.CanPlayVideo)
                        .Select(item => item.GetFullScreenVideo())
                        .ToList();
        }
    }
}
