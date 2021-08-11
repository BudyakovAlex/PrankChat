using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Common.Constants;
using PrankChat.Mobile.Core.Data.Enums;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Managers.Orders;
using PrankChat.Mobile.Core.Managers.Users;
using PrankChat.Mobile.Core.Managers.Video;
using PrankChat.Mobile.Core.Messages;
using PrankChat.Mobile.Core.Models.Data.FilterTypes;
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels.Common.Abstract;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items;
using PrankChat.Mobile.Core.Presentation.ViewModels.Parameters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile.Abstract;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile.Cashbox;
using PrankChat.Mobile.Core.Presentation.ViewModels.Results;
using PrankChat.Mobile.Core.Presentation.ViewModels.Subscriptions.Items;
using PrankChat.Mobile.Core.Providers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Profile
{
    public class ProfileViewModel : BaseProfileViewModel
    {
        private readonly IVideoManager _videoManager;
        private readonly IOrdersManager _ordersManager;
        private readonly IWalkthroughsProvider _walkthroughsProvider;

        public ProfileViewModel(
            IVideoManager videoManager,
            IOrdersManager ordersManager,
            IUsersManager usersManager,
            IWalkthroughsProvider walkthroughsProvider) : base(usersManager)
        {
            _videoManager = videoManager;
            _ordersManager = ordersManager;
            _walkthroughsProvider = walkthroughsProvider;

            Items = new MvxObservableCollection<OrderItemViewModel>();

            ShowWithdrawalCommand = this.CreateCommand(ShowWithdrawalAsync);
            ShowRefillCommand = this.CreateCommand(ShowRefillAsync);
            ShowSubscriptionsCommand = this.CreateCommand(ShowSubscriptionsAsync);
            ShowSubscribersCommand = this.CreateCommand(ShowSubscribersAsync);
            ShowWalkthrouthCommand = this.CreateCommand(ShowWalkthrouthAsync);
            LoadProfileCommand = this.CreateCommand(LoadProfileAsync);
            ShowUpdateProfileCommand = this.CreateCommand(ShowUpdateProfileAsync);

            Messenger.SubscribeOnMainThread<OrderChangedMessage>((msg) => ReloadItemsCommand?.Execute()).DisposeWith(Disposables);
            Messenger.SubscribeOnMainThread<SubscriptionChangedMessage>((msg) => LoadProfileCommand.Execute()).DisposeWith(Disposables);
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

        public override Task InitializeAsync()
        {
            SelectedOrderType = ProfileOrderType.MyOrdered;
            
            _ = SafeExecutionWrapper.WrapAsync(async () =>
            {
                await base.InitializeAsync();
                await LoadProfileAsync();
            });

            return Task.CompletedTask;
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
            var shouldRefresh = await NavigationManager.NavigateAsync<SubscriptionsViewModel, SubscriptionsNavigationParameter, bool>(navigationParameters);
            if (!shouldRefresh)
            {
                return;
            }

            await LoadProfileAsync();
        }

        private async Task ShowSubscriptionsAsync()
        {
            var navigationParameters = new SubscriptionsNavigationParameter(SubscriptionTabType.Subscriptions, UserSessionProvider.User.Id, UserSessionProvider.User.Name);
            var shouldRefresh = await NavigationManager.NavigateAsync<SubscriptionsViewModel, SubscriptionsNavigationParameter, bool>(navigationParameters);
            if (!shouldRefresh)
            {
                return;
            }

            await LoadProfileAsync();
        }

        private async Task ShowRefillAsync()
        {
            if (!IsEmailVerified)
            {
                var canGoProfile = await UserInteraction.ShowConfirmAsync(Resources.Profile_Your_Email_Not_Actual, Resources.Attention, Resources.Ok, Resources.Cancel);
                if (canGoProfile)
                {
                    await NavigationManager.NavigateAsync<ProfileUpdateViewModel, ProfileUpdateResult>();
                }

                return;
            }

            var navigationParameter = new CashboxTypeNavigationParameter(CashboxType.Refill);
            var isReloadNeeded = await NavigationManager.NavigateAsync<CashboxViewModel, CashboxTypeNavigationParameter, bool>(navigationParameter);
            if (!isReloadNeeded)
            {
                return;
            }

            await LoadProfileAsync();
        }

        private async Task ShowWithdrawalAsync()
        {
            if (!IsEmailVerified)
            {
                var canGoProfile = await UserInteraction.ShowConfirmAsync(Resources.Profile_Your_Email_Not_Actual, Resources.Attention, Resources.Ok, Resources.Cancel);
                if (canGoProfile)
                {
                    await NavigationManager.NavigateAsync<ProfileUpdateViewModel, ProfileUpdateResult>();
                }

                return;
            }

            var navigationParameter = new CashboxTypeNavigationParameter(CashboxType.Withdrawal);
            var isReloadNeeded = await NavigationManager.NavigateAsync<CashboxViewModel, CashboxTypeNavigationParameter, bool>(navigationParameter);
            if (!isReloadNeeded)
            {
                return;
            }

            await LoadProfileAsync();
        }

        private async Task ShowWalkthrouthAsync()
        {
            await _walkthroughsProvider.ShowWalthroughAsync<ProfileViewModel>();
        }

        private async Task LoadProfileAsync()
        {
            await UsersManager.GetAndRefreshUserInSessionAsync();
            Reset();

            await InitializeProfileData();
        }

        private async Task ShowUpdateProfileAsync()
        {
            var result = await NavigationManager.NavigateAsync<ProfileUpdateViewModel, ProfileUpdateResult>();
            var isUpdated =(result?.IsProfileUpdated ?? false) || (result?.IsAvatarUpdated ?? false);
            if (!isUpdated)
            {
                return;
            }

            await LoadProfileAsync();
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
                _videoManager,
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

        private BaseVideoItemViewModel[] GetFullScreenVideos()
        {
            return Items.Where(item => item.VideoItemViewModel != null)
                        .Select(item => item.VideoItemViewModel)
                        .ToArray();
        }
    }
}
