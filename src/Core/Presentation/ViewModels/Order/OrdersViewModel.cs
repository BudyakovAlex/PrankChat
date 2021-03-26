using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Managers.Orders;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.FilterTypes;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.ViewModels.Abstract;
using PrankChat.Mobile.Core.Presentation.ViewModels.Arbitration.Items;
using PrankChat.Mobile.Core.Presentation.ViewModels.Common;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items;
using PrankChat.Mobile.Core.Providers;
using PrankChat.Mobile.Core.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order
{
    public class OrdersViewModel : PaginationViewModel
    {
        private readonly IOrdersManager _ordersManager;
        private readonly IWalkthroughsProvider _walkthroughsProvider;
        private readonly ExecutionStateWrapper _loadDataStateWrapper;

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

        private Task _reloadTask;

        private string _activeOrderFilterName = string.Empty;
        private string _activeArbitrationFilterName = string.Empty;

        public OrdersViewModel(IOrdersManager ordersManager, IWalkthroughsProvider walkthroughsProvider) : base(Constants.Pagination.DefaultPaginationSize)
        {
            _ordersManager = ordersManager;
            _walkthroughsProvider = walkthroughsProvider;

            _loadDataStateWrapper = new ExecutionStateWrapper();
            _loadDataStateWrapper.SubscribeToEvent<ExecutionStateWrapper, bool>(
                OnIsBusyChanged,
                (wrapper, handler) => wrapper.IsBusyChanged += handler,
                (wrapper, handler) => wrapper.IsBusyChanged -= handler).DisposeWith(Disposables);

            OpenFilterCommand = this.CreateCommand(OpenFilterAsync);
            ShowWalkthrouthCommand = this.CreateCommand(ShowWalkthrouthAsync);
            LoadDataCommand = this.CreateCommand(() => _loadDataStateWrapper.WrapAsync(LoadDataAsync), useIsBusyWrapper: false);

            Messenger.SubscribeOnMainThread<OrderChangedMessage>(OrdersChanged).DisposeWith(Disposables);
            Messenger.SubscribeOnMainThread<RemoveOrderMessage>(OrderRemoved).DisposeWith(Disposables);
        }

        public override bool IsBusy => base.IsBusy || _loadDataStateWrapper.IsBusy;

        public MvxObservableCollection<BaseViewModel> Items { get; } = new MvxObservableCollection<BaseViewModel>();

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

                    _ = DebounceRefreshDataAsync();
                }
            }
        }

        public MvxAsyncCommand OpenFilterCommand { get; }

        public MvxAsyncCommand LoadDataCommand { get; }

        public IMvxAsyncCommand ShowWalkthrouthCommand { get; }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            OrderFilterType = OrderFilterType.All;
            ArbitrationFilterType = ArbitrationOrderFilterType.All;
            TabType = OrdersTabType.Order;

            _ = LoadDataCommand.ExecuteAsync();
        }

        protected async override Task<int> LoadMoreItemsAsync(int page = 1, int pageSize = 20)
        {
            try
            {
                if (TabType == OrdersTabType.Arbitration)
                {
                    var arbitrationOrders = await _ordersManager.GetArbitrationOrdersAsync(ArbitrationFilterType, page, pageSize);
                    return SetList(arbitrationOrders, page, ProduceArbitrationOrderViewModel, Items);
                }

                var orders = await _ordersManager.GetOrdersAsync(OrderFilterType, page, pageSize);
                return SetList(orders, page, ProduceOrderViewModel, Items);
            }
            catch (Exception ex)
            {
                ErrorHandleService.HandleException(ex);
                ErrorHandleService.LogError(this, "Order list loading error occured.");
                return 0;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (!disposing)
            {
                return;
            }

            Items.OfType<IDisposable>().ForEach(disposable => disposable.Dispose());
        }

        private async Task DebounceRefreshDataAsync()
        {
            if (_reloadTask != null &&
                !_reloadTask.IsCompleted &&
                !_reloadTask.IsCanceled &&
                !_reloadTask.IsFaulted)
            {
                await _reloadTask;
            }

            Items.Clear();
            _reloadTask = ReloadItemsCommand.ExecuteAsync();
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

        private OrderItemViewModel ProduceOrderViewModel(Models.Data.Order order)
        {
            return new OrderItemViewModel(
                UserSessionProvider,
                order,
                GetFullScreenVideos);
        }

        private ArbitrationItemViewModel ProduceArbitrationOrderViewModel(ArbitrationOrder order)
        {
            return new ArbitrationItemViewModel(
                UserSessionProvider,
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
                GetFullScreenVideos);
        }

        private List<FullScreenVideo> GetFullScreenVideos()
        {
            return Items.OfType<IFullScreenVideoOwnerViewModel>()
                        .Where(item => item.CanPlayVideo)
                        .Select(item => item.GetFullScreenVideo())
                        .ToList();
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