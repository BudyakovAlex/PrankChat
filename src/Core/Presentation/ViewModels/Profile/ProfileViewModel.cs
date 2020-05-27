using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.FilterTypes;
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items;
using PrankChat.Mobile.Core.Providers;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Profile
{
    public class ProfileViewModel : BaseProfileViewModel
    {
        private readonly IVideoPlayerService _videoPlayerService;
        private readonly IMvxMessenger _mvxMessenger;
        private readonly IWalkthroughsProvider _walkthroughsProvider;

        private MvxSubscriptionToken _newOrderMessageToken;

        private ProfileOrderType _selectedOrderType;
        public ProfileOrderType SelectedOrderType
        {
            get => _selectedOrderType;
            set
            {
                if (SetProperty(ref _selectedOrderType, value))
                {
                    LoadProfileCommand.Execute();
                }
            }
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

        public MvxObservableCollection<OrderItemViewModel> Items { get; set; } = new MvxObservableCollection<OrderItemViewModel>();

        public MvxAsyncCommand ShowWalkthrouthCommand => new MvxAsyncCommand(ShowWalkthrouthAsync);

        public MvxAsyncCommand ShowRefillCommand => new MvxAsyncCommand(NavigationService.ShowRefillView);

        public MvxAsyncCommand ShowWithdrawalCommand => new MvxAsyncCommand(NavigationService.ShowWithdrawalView);

        public MvxAsyncCommand LoadProfileCommand => new MvxAsyncCommand(OnLoadProfileAsync);

        public MvxAsyncCommand ShowUpdateProfileCommand => new MvxAsyncCommand(OnShowUpdateProfileAsync);

        public ProfileViewModel(INavigationService navigationService,
                                IDialogService dialogService,
                                IApiService apiService,
                                IVideoPlayerService videoPlayerService,
                                IErrorHandleService errorHandleService,
                                ISettingsService settingsService,
                                IMvxMessenger mvxMessenger,
                                IWalkthroughsProvider walkthroughsProvider)
            : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
            _videoPlayerService = videoPlayerService;
            _mvxMessenger = mvxMessenger;
            _walkthroughsProvider = walkthroughsProvider;
        }

        private void Subscription()
        {
            _newOrderMessageToken = _mvxMessenger.SubscribeOnMainThread<OrderChangedMessage>(OnOrdersChanged);
        }

        private void Unsubscription()
        {
            if (_newOrderMessageToken is null)
            {
                return;
            }

            _mvxMessenger.Unsubscribe<OrderChangedMessage>(_newOrderMessageToken);
            _newOrderMessageToken.Dispose();
        }

        private void OnOrdersChanged(OrderChangedMessage message)
        {
            ReloadItemsCommand.Execute();
        }

        public override Task Initialize()
        {
            SelectedOrderType = ProfileOrderType.MyOrdered;
            base.Initialize();
            return LoadProfileCommand.ExecuteAsync();
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

        public override void ViewCreated()
        {
            base.ViewCreated();
            Subscription();
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            Unsubscription();
            base.ViewDestroy(viewFinishing);
        }

        private Task ShowWalkthrouthAsync()
        {
            return _walkthroughsProvider.ShowWalthroughAsync<ProfileViewModel>();
        }

        private async Task OnLoadProfileAsync()
        {
            try
            {
                IsBusy = true;
                await ApiService.GetCurrentUserAsync();
                await InitializeProfileData();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnShowUpdateProfileAsync()
        {
            var isUpdated = await NavigationService.ShowUpdateProfileView();
            if (isUpdated)
            {
                await InitializeProfileData();
            }
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
                                          _mvxMessenger,
                                          order);
        }
    }
}
