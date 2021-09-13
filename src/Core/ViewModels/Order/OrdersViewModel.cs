using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Managers.Orders;
using PrankChat.Mobile.Core.Managers.Video;
using PrankChat.Mobile.Core.Messages;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.FilterTypes;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.ViewModels.Arbitration.Items;
using PrankChat.Mobile.Core.ViewModels.Common;
using PrankChat.Mobile.Core.ViewModels.Common.Abstract;
using PrankChat.Mobile.Core.ViewModels.Order.Items;
using PrankChat.Mobile.Core.ViewModels.Order.Items.Abstract;
using PrankChat.Mobile.Core.Providers;
using PrankChat.Mobile.Core.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ViewModels.Order
{
    public class OrdersViewModel : PaginationViewModel
    {
        private readonly IVideoManager _videoManager;
        private readonly IOrdersManager _ordersManager;
        private readonly IWalkthroughsProvider _walkthroughsProvider;

        private readonly ExecutionStateWrapper _loadDataStateWrapper;

        private readonly Dictionary<ArbitrationOrderFilterType, string> _arbitrationOrderFilterTypeTitleMap =
            new Dictionary<ArbitrationOrderFilterType, string>
            {
                { ArbitrationOrderFilterType.All, Resources.AllOrdersInDispute },
                { ArbitrationOrderFilterType.New, Resources.NewDisputeOrders },
                { ArbitrationOrderFilterType.My, Resources.MyOrdersInDispute },
            };

        private readonly Dictionary<OrderFilterType, string> _orderFilterTypeTitleMap =
            new Dictionary<OrderFilterType, string>
            {
                { OrderFilterType.All, Resources.AllOrders },
                { OrderFilterType.New, Resources.NewOrders },
                { OrderFilterType.InProgress, Resources.OrderExecuted },
                { OrderFilterType.MyOwn, Resources.MyOrders }
            };

        private Task _reloadTask;

        private string _activeOrderFilterName = string.Empty;
        private string _activeArbitrationFilterName = string.Empty;

        public OrdersViewModel(
            IVideoManager videoManager,
            IOrdersManager ordersManager,
            IWalkthroughsProvider walkthroughsProvider) : base(Constants.Pagination.DefaultPaginationSize)
        {
            _videoManager = videoManager;
            _ordersManager = ordersManager;
            _walkthroughsProvider = walkthroughsProvider;

            _loadDataStateWrapper = new ExecutionStateWrapper();
            _loadDataStateWrapper.SubscribeToEvent<ExecutionStateWrapper, bool>(
                OnIsBusyChanged,
                (wrapper, handler) => wrapper.IsBusyChanged += handler,
                (wrapper, handler) => wrapper.IsBusyChanged -= handler).DisposeWith(Disposables);

            Items = new MvxObservableCollection<BaseOrderItemViewModel>();

            OpenFilterCommand = this.CreateCommand(OpenFilterAsync);
            ShowWalkthrouthCommand = this.CreateCommand(ShowWalkthrouthAsync);
            LoadDataCommand = this.CreateCommand(() => _loadDataStateWrapper.WrapAsync(LoadDataAsync), useIsBusyWrapper: false);

            Messenger.SubscribeOnMainThread<OrderChangedMessage>(OrdersChanged).DisposeWith(Disposables);
            Messenger.SubscribeOnMainThread<RemoveOrderMessage>(OrderRemoved).DisposeWith(Disposables);
        }

        public override bool IsBusy => base.IsBusy || _loadDataStateWrapper.IsBusy;

        public MvxObservableCollection<BaseOrderItemViewModel> Items { get; }

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
            var selectedFilterName = await UserInteraction.ShowMenuDialogAsync(parameters, Resources.Cancel);
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

        private Task OpenFilterAsync() => TabType switch
        {
            OrdersTabType.Order => OpenOrderFilterAsync(),
            OrdersTabType.Arbitration => OpenArbitrationFilterAsync(),
            _ => throw new ArgumentOutOfRangeException(),
        };

        private OrderItemViewModel ProduceOrderViewModel(Models.Data.Order order)
        {
            return new OrderItemViewModel(
                _videoManager,
                UserSessionProvider,
                order,
                GetFullScreenVideos);
        }

        private ArbitrationOrderItemViewModel ProduceArbitrationOrderViewModel(ArbitrationOrder order)
        {
            return new ArbitrationOrderItemViewModel(
                _videoManager,
                UserSessionProvider,
                order,
                GetFullScreenVideos);
        }

        private BaseVideoItemViewModel[] GetFullScreenVideos()
        {
            return Items.Where(item => item.VideoItemViewModel != null)
                        .Select(item => item.VideoItemViewModel)
                        .ToArray();
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
            var selectedFilterName = await UserInteraction.ShowMenuDialogAsync(parameters, Resources.Cancel);
            if (string.IsNullOrWhiteSpace(selectedFilterName) || selectedFilterName == Resources.Cancel)
            {
                return;
            }

            ArbitrationFilterType = _arbitrationOrderFilterTypeTitleMap.FirstOrDefault(x => x.Value == selectedFilterName).Key;
            await LoadDataCommand.ExecuteAsync();
        }
    }
}