using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Managers.Orders;
using PrankChat.Mobile.Core.Managers.Users;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items;
using PrankChat.Mobile.Core.Presentation.ViewModels.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Profile
{
    public class UserProfileViewModel : PaginationViewModel, IMvxViewModel<int, bool>
    {
        private readonly IUsersManager _usersManager;
        private readonly IOrdersManager _ordersManager;

        private CancellationTokenSource _cancellationSunsciptionTokenSource;

        private int _userId;
        private bool _isReloadNeeded;
        private int _subscribersCount;

        public UserProfileViewModel(IUsersManager usersManager, IOrdersManager ordersManager) : base(Constants.Pagination.DefaultPaginationSize)
        {
            _usersManager = usersManager;
            _ordersManager = ordersManager;

            Items = new MvxObservableCollection<OrderItemViewModel>();
            CloseCompletionSource = new TaskCompletionSource<object>();

            RefreshUserDataCommand = new MvxAsyncCommand(() => ExecutionStateWrapper.WrapAsync(RefreshUserDataAsync));
            SubscribeCommand = new MvxCommand(Subscribe);
            ShowSubscriptionsCommand = new MvxAsyncCommand(ShowSubscriptionsAsync);
            ShowSubscribersCommand = new MvxAsyncCommand(ShowSubscribersAsync);
        }

        private ProfileOrderType _selectedOrderType;
        public ProfileOrderType SelectedOrderType
        {
            get => _selectedOrderType;
            set
            {
                SetProperty(ref _selectedOrderType, value);

                Items.Clear();
                ReloadItemsCommand.Cancel();
                ReloadItemsCommand.Execute();
            }
        }

        public IMvxAsyncCommand RefreshUserDataCommand { get; }

        public IMvxAsyncCommand ShowSubscriptionsCommand { get; }

        public IMvxAsyncCommand ShowSubscribersCommand { get; }

        public IMvxCommand SubscribeCommand { get; }

        public MvxObservableCollection<OrderItemViewModel> Items { get; }

        public TaskCompletionSource<object> CloseCompletionSource { get; set; }

        public string ProfileShortLogin => Login.ToShortenName();

        public bool HasDescription => !string.IsNullOrEmpty(Description);

        private string _profilePhotoUrl;
        public string ProfilePhotoUrl
        {
            get => _profilePhotoUrl;
            private set => SetProperty(ref _profilePhotoUrl, value);
        }

        private string _login;
        public string Login
        {
            get => _login;
            private set => SetProperty(ref _login, value, () => RaisePropertyChanged(nameof(ProfileShortLogin)));
        }

        private string _subscribersValue;
        public string SubscribersValue
        {
            get => _subscribersValue;
            private set => SetProperty(ref _subscribersValue, value);
        }

        private string _subscriptionsValue;
        public string SubscriptionsValue
        {
            get => _subscriptionsValue;
            private set => SetProperty(ref _subscriptionsValue, value);
        }

        private bool _isSubscribed;
        public bool IsSubscribed
        {
            get => _isSubscribed;
            private set => SetProperty(ref _isSubscribed, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            private set => SetProperty(ref _description, value, () => RaisePropertyChanged(nameof(HasDescription)));
        }

        public void Prepare(int parameter)
        {
            _userId = parameter;
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            if (viewFinishing &&
                CloseCompletionSource != null &&
                !CloseCompletionSource.Task.IsCompleted &&
                !CloseCompletionSource.Task.IsFaulted)
            {
                CloseCompletionSource?.SetResult(_isReloadNeeded);
            }

            base.ViewDestroy(viewFinishing);
        }

        public override Task InitializeAsync()
        {
            return Task.WhenAll(base.InitializeAsync(), RefreshUserDataAsync());
        }

        protected virtual void Subscribe()
        {
            IsSubscribed = !IsSubscribed;
            var subscribersCount = _subscribersCount = IsSubscribed
                ? _subscribersCount + 1
                : _subscribersCount - 1;

            SubscribersValue = (subscribersCount > 0 ? subscribersCount : 0).ToCountString();

            SubscribeAsync().FireAndForget();
            _isReloadNeeded = true;
        }

        protected override async Task<int> LoadMoreItemsAsync(int page = 1, int pageSize = Constants.Pagination.DefaultPaginationSize)
        {
            var items = await GetOrdersAsync(page, pageSize);
            return SetList(items, page, ProduceOrderItemViewModel, Items);
        }

        protected virtual async Task<Pagination<Models.Data.Order>> GetOrdersAsync(int page, int pageSize)
        {
            switch (SelectedOrderType)
            {
                case ProfileOrderType.MyOrdered:
                    return await _ordersManager.GetUserOwnOrdersAsync(_userId, page, pageSize);

                case ProfileOrderType.OrdersCompletedByMe:
                    return await _ordersManager.GetUserExecuteOrdersAsync(_userId, page, pageSize);
            }

            return new Pagination<Models.Data.Order>();
        }

        private OrderItemViewModel ProduceOrderItemViewModel(Models.Data.Order order)
        {
            return new OrderItemViewModel(NavigationService,
                                          SettingsService,
                                          order,
                                          GetFullScreenVideoDataModels);
        }

        private List<FullScreenVideo> GetFullScreenVideoDataModels()
        {
            return Items.Where(item => item.CanPlayVideo)
                        .Select(item => item.GetFullScreenVideoDataModel())
                        .ToList();
        }

        private async Task ShowSubscribersAsync()
        {
            var navigationParameters = new SubscriptionsNavigationParameter(SubscriptionTabType.Subscribers, _userId, Login);
            var shouldRefresh = await NavigationService.ShowSubscriptionsView(navigationParameters);
            if (!shouldRefresh)
            {
                return;
            }

            await LoadUserProfileAsync();
        }

        private async Task ShowSubscriptionsAsync()
        {
            var navigationParameters = new SubscriptionsNavigationParameter(SubscriptionTabType.Subscriptions, _userId, Login);
            var shouldRefresh = await NavigationService.ShowSubscriptionsView(navigationParameters);
            if (!shouldRefresh)
            {
                return;
            }

            await LoadUserProfileAsync();
        }

        private async Task SubscribeAsync()
        {
            _cancellationSunsciptionTokenSource?.Cancel();
            if (_cancellationSunsciptionTokenSource == null)
            {
                _cancellationSunsciptionTokenSource = new CancellationTokenSource();
            }

            try
            {
                if (!IsSubscribed)
                {
                    await _usersManager.UnsubscribeFromUserAsync(_userId, _cancellationSunsciptionTokenSource.Token);
                    return;
                }

                await _usersManager.SubscribeToUserAsync(_userId, _cancellationSunsciptionTokenSource.Token);
            }
            finally
            {
                _cancellationSunsciptionTokenSource?.Dispose();
                _cancellationSunsciptionTokenSource = null;

                Messenger.Publish(new SubscriptionChangedMessage(this));
            }
        }

        private Task RefreshUserDataAsync()
        {
            return Task.WhenAll(LoadUserProfileAsync(), ReloadItemsCommand.ExecuteAsync());
        }

        private async Task LoadUserProfileAsync()
        {
            var user = await _usersManager.GetUserAsync(_userId);
            if (user is null)
            {
                return;
            }

            ProfilePhotoUrl = user.Avatar;
            Login = user.Login;
            Description = user.Description;
            _subscribersCount = user.SubscribersCount ?? 0;
            SubscribersValue = _subscribersCount.ToCountString();
            SubscriptionsValue = user.SubscriptionsCount.ToCountString();
            IsSubscribed = user.IsSubscribed;
        }
    }
}