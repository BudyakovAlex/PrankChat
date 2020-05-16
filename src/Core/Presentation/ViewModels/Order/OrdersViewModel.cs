using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.FilterTypes;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Arbitration.Items;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items;
using PrankChat.Mobile.Core.Presentation.ViewModels.Shared;
using PrankChat.Mobile.Core.Providers;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order
{
    public class OrdersViewModel : PaginationViewModel
    {
        private readonly IMvxMessenger _mvxMessenger;
        private readonly ISettingsService _settingsService;
        private readonly IWalkthroughsProvider _walkthroughsProvider;

        private readonly Dictionary<ArbitrationOrderFilterType, string> _arbitrationOrderFilterTypeTitleMap =
            new Dictionary<ArbitrationOrderFilterType, string>
            {
                { ArbitrationOrderFilterType.All, Resources.RateView_Filter_AllTasks },
                { ArbitrationOrderFilterType.New, Resources.RateView_Filter_NewTasks },
                { ArbitrationOrderFilterType.My, Resources.RateView_Filter_MyTasks },
            };

        private readonly Dictionary<OrderFilterType, string> _orderFilterTypeTitleMap =
            new Dictionary<OrderFilterType, string>
            {
                { OrderFilterType.All, Resources.OrdersView_Filter_AllTasks },
                { OrderFilterType.New, Resources.OrdersView_Filter_NewTasks },
                { OrderFilterType.InProgress, Resources.OrdersView_Filter_CurrentTasks },
                { OrderFilterType.MyOwn, Resources.OrdersView_Filter_MyTasks }
            };

        private MvxSubscriptionToken _newOrderMessageToken;
        private MvxSubscriptionToken _removeOrderMessageToken;
        private string _activeOrderFilterName = string.Empty;
        private string _activeArbitrationFilterName = string.Empty;

        public MvxObservableCollection<BaseItemViewModel> Items { get; } = new MvxObservableCollection<BaseItemViewModel>();

        public string ActiveFilterName => TabType == OrdersTabType.Order ? _activeOrderFilterName : _activeArbitrationFilterName;

        private OrderFilterType _orderFilterType;
        public OrderFilterType OrderFilterType
        {
            get => _orderFilterType;
            set
            {
                _orderFilterType = value;
                if (_orderFilterTypeTitleMap.TryGetValue(_orderFilterType, out var activeFilterName))
                {
                    _activeOrderFilterName = activeFilterName;
                    RaisePropertyChanged(nameof(ActiveFilterName));
                }
            }
        }

        private ArbitrationOrderFilterType _arbitrationFilterType;
        public ArbitrationOrderFilterType ArbitrationFilterType
        {
            get => _arbitrationFilterType;
            set
            {
                _arbitrationFilterType = value;
                if (_arbitrationOrderFilterTypeTitleMap.TryGetValue(_arbitrationFilterType, out var activeFilterName))
                {
                    _activeArbitrationFilterName = activeFilterName;
                    RaisePropertyChanged(nameof(ActiveFilterName));
                }
            }
        }

        private OrdersTabType _tabType;
        public OrdersTabType TabType
        {
            get => _tabType;
            set
            {
                if (SetProperty(ref _tabType, value))
                {
                    RaisePropertyChanged(nameof(ActiveFilterName));

                    LoadDataCommand.Cancel();
                    LoadDataCommand.ExecuteAsync();
                }
            }
        }

        public MvxAsyncCommand OpenFilterCommand { get; }

        public MvxAsyncCommand LoadDataCommand { get; }

        public IMvxAsyncCommand ShowWalkthrouthCommand { get; }

        public OrdersViewModel(INavigationService navigationService,
                               IDialogService dialogService,
                               IApiService apiService,
                               IMvxMessenger mvxMessenger,
                               ISettingsService settingsService,
                               IErrorHandleService errorHandleService,
                               IWalkthroughsProvider walkthroughsProvider)
            : base(Constants.Pagination.DefaultPaginationSize, navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
            _mvxMessenger = mvxMessenger;
            _settingsService = settingsService;
            _walkthroughsProvider = walkthroughsProvider;

            OpenFilterCommand = new MvxAsyncCommand(OpenFilterAsync);
            LoadDataCommand = new MvxAsyncCommand(LoadDataAsync);
            ShowWalkthrouthCommand = new MvxAsyncCommand(ShowWalkthrouthAsync);
        }

        public override Task Initialize()
        {
            OrderFilterType = OrderFilterType.All;
            ArbitrationFilterType = ArbitrationOrderFilterType.All;
            TabType = OrdersTabType.Order;

            return LoadDataCommand.ExecuteAsync();
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
            return _walkthroughsProvider.ShowWalthroughAsync<OrdersViewModel>();
        }

        private async Task OpenOrderFilterAsync()
        {
            var parameters = _orderFilterTypeTitleMap.Values.ToArray();
            var selectedFilterName = await DialogService.ShowMenuDialogAsync(parameters, Resources.Cancel);
            if (string.IsNullOrWhiteSpace(selectedFilterName) || selectedFilterName == Resources.Cancel)
            {
                return;
            }

            OrderFilterType = _orderFilterTypeTitleMap.FirstOrDefault(kv => kv.Value == selectedFilterName).Key;
            await LoadDataCommand.ExecuteAsync();
        }

        protected override async Task<int> LoadMoreItemsAsync(int page = 1, int pageSize = 20)
        {
            try
            {
                IsBusy = true;

                if (TabType == OrdersTabType.Arbitration)
                {
                    var arbitrationOrders = await ApiService.GetArbitrationOrdersAsync(ArbitrationFilterType, page, pageSize);
                    return SetList(arbitrationOrders, page, ProduceArbitrationOrderViewModel, Items);
                }

                var orders = await ApiService.GetOrdersAsync(OrderFilterType, page, pageSize);
                return SetList(orders, page, ProduceOrderViewModel, Items);
            }
            catch (Exception ex)
            {
                ErrorHandleService.HandleException(ex);
                ErrorHandleService.LogError(this, "Order list loading error occured.");
                return 0;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private Task LoadDataAsync()
        {
            Reset();
            Items.Clear();

            return LoadMoreItemsCommand.ExecuteAsync();
        }

        private Task OpenFilterAsync()
        {
            switch (TabType)
            {
                case OrdersTabType.Order:
                    return OpenOrderFilterAsync();

                case OrdersTabType.Arbitration:
                    return OpenArbitrationFilterAsync();

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private OrderItemViewModel ProduceOrderViewModel(OrderDataModel order)
        {
            return new OrderItemViewModel(NavigationService,
                                          _settingsService,
                                          _mvxMessenger,
                                          order);
        }

        private ArbitrationItemViewModel ProduceArbitrationOrderViewModel(ArbitrationOrderDataModel order)
        {
            return new ArbitrationItemViewModel(NavigationService,
                                                _settingsService,
                                                IsUserSessionInitialized,
                                                order.Id,
                                                order.Title,
                                                order.Customer?.Avatar,
                                                order.Customer?.Name,
                                                order.Price,
                                                order.Likes,
                                                order.Dislikes,
                                                order.ArbitrationFinishAt ?? DateTime.UtcNow,
                                                order.Customer?.Id);
        }

        private void Subscription()
        {
            _newOrderMessageToken = _mvxMessenger.SubscribeOnMainThread<NewOrderMessage>(OnNewOrderAdded);
            _removeOrderMessageToken = _mvxMessenger.SubscribeOnMainThread<RemoveOrderMessage>(OnRemoveOrderMessage);
        }

        private void Unsubscription()
        {
            if (_newOrderMessageToken != null)
            {
                _mvxMessenger.Unsubscribe<NewOrderMessage>(_newOrderMessageToken);
                _newOrderMessageToken.Dispose();
            }

            if (_removeOrderMessageToken != null)
            {
                _mvxMessenger.Unsubscribe<RemoveOrderMessage>(_removeOrderMessageToken);
                _removeOrderMessageToken.Dispose();
            }

            Items.OfType<IDisposable>().ForEach(x => x.Dispose());
        }

        private void OnNewOrderAdded(NewOrderMessage newOrderMessage)
        {
            ReloadItemsCommand?.Execute();
        }

        private void OnRemoveOrderMessage(RemoveOrderMessage message)
        {
            if (message.Status == OrderStatusType.InArbitration && TabType == OrdersTabType.Arbitration)
                return;

            var deletedItem = Items.OfType<OrderItemViewModel>().FirstOrDefault(order => order.OrderId == message.OrderId);
            if (deletedItem == null)
            {
                return;
            }

            Items.Remove(deletedItem);
            deletedItem.Dispose();
        }

        private async Task OpenArbitrationFilterAsync()
        {
            var parameters = _arbitrationOrderFilterTypeTitleMap.Values.ToArray();
            var selectedFilterName = await DialogService.ShowMenuDialogAsync(parameters, Resources.Cancel);
            if (string.IsNullOrWhiteSpace(selectedFilterName) || selectedFilterName == Resources.Cancel)
            {
                return;
            }

            ArbitrationFilterType = _arbitrationOrderFilterTypeTitleMap.FirstOrDefault(x => x.Value == selectedFilterName).Key;
            await LoadDataCommand.ExecuteAsync();
        }
    }
}