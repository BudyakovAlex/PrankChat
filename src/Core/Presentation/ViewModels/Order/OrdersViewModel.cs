using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.FilterTypes;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.ViewModels.Arbitration.Items;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items;
using PrankChat.Mobile.Core.Presentation.ViewModels.Shared;
using PrankChat.Mobile.Core.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order
{
    public class OrdersViewModel : PaginationViewModel
    {
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
        private MvxSubscriptionToken _tabChangedMessage;
        private MvxSubscriptionToken _enterForegroundMessage;

        private string _activeOrderFilterName = string.Empty;
        private string _activeArbitrationFilterName = string.Empty;

        public OrdersViewModel(IWalkthroughsProvider walkthroughsProvider) : base(Constants.Pagination.DefaultPaginationSize)
        {
            _walkthroughsProvider = walkthroughsProvider;

            OpenFilterCommand = new MvxAsyncCommand(OpenFilterAsync);
            LoadDataCommand = new MvxAsyncCommand(LoadDataAsync);
            ShowWalkthrouthCommand = new MvxAsyncCommand(ShowWalkthrouthAsync);
        }

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

        public override async Task Initialize()
        {
            await base.Initialize();
            OrderFilterType = OrderFilterType.All;
            ArbitrationFilterType = ArbitrationOrderFilterType.All;
            TabType = OrdersTabType.Order;

            await LoadDataCommand.ExecuteAsync();
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
                                          SettingsService,
                                          Messenger,
                                          order,
                                          GetFullScreenVideoDataModels);
        }

        private ArbitrationItemViewModel ProduceArbitrationOrderViewModel(ArbitrationOrderDataModel order)
        {
            return new ArbitrationItemViewModel(NavigationService,
                                                SettingsService,
                                                IsUserSessionInitialized,
                                                order.Id,
                                                order.Title,
                                                order.Customer?.Avatar,
                                                order.Customer?.Login,
                                                order.Price,
                                                order.Likes,
                                                order.Dislikes,
                                                order.ArbitrationFinishAt ?? DateTime.UtcNow,
                                                order.Customer?.Id,
                                                order,
                                                GetFullScreenVideoDataModels);
        }

        private List<FullScreenVideoDataModel> GetFullScreenVideoDataModels()
        {
            return Items.OfType<IFullScreenVideoOwnerViewModel>()
                        .Where(item => item.CanPlayVideo)
                        .Select(item => item.GetFullScreenVideoDataModel())
                        .ToList();
        }

        private void Subscription()
        {
            _newOrderMessageToken = Messenger.SubscribeOnMainThread<OrderChangedMessage>(OrdersChanged);
            _removeOrderMessageToken = Messenger.SubscribeOnMainThread<RemoveOrderMessage>(OrderRemoved);
            _tabChangedMessage = Messenger.SubscribeOnMainThread<TabChangedMessage>(TabChanged);
            _enterForegroundMessage = Messenger.SubscribeOnMainThread<EnterForegroundMessage>((msg) => ReloadItemsCommand?.Execute());

            SubscribeToNotificationsUpdates();
        }

        private void TabChanged(TabChangedMessage msg)
        {
            if (msg.TabType != MainTabType.Orders)
            {
                return;
            }

            LoadDataCommand.Execute();
        }

        private void Unsubscription()
        {
            _newOrderMessageToken?.Dispose();
            _removeOrderMessageToken?.Dispose();
            _tabChangedMessage?.Dispose();
            _enterForegroundMessage?.Dispose();

            UnsubscribeFromNotificationsUpdates();

            Items.OfType<IDisposable>().ForEach(disposable => disposable.Dispose());
        }

        private void OrdersChanged(OrderChangedMessage newOrderMessage)
        {
            ReloadItemsCommand?.Execute();
        }

        private void OrderRemoved(RemoveOrderMessage message)
        {
            if (message.Status == OrderStatusType.InArbitration &&
                TabType == OrdersTabType.Arbitration)
            {
                return;
            }

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