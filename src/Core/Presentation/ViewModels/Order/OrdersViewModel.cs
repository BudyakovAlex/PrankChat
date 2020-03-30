using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items;
using PrankChat.Mobile.Core.Presentation.ViewModels.Shared;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order
{
    public class OrdersViewModel : PaginationViewModel
    {
        private readonly IMvxMessenger _mvxMessenger;
        private readonly ISettingsService _settingsService;
        private readonly Dictionary<OrderFilterType, string> _orderFilterTypeTitleMap;

        private MvxSubscriptionToken _newOrderMessageToken;
        private MvxSubscriptionToken _removeOrderMessageToken;

        public MvxObservableCollection<OrderItemViewModel> Items { get; } = new MvxObservableCollection<OrderItemViewModel>();

        private string _activeFilterName;
        public string ActiveFilterName
        {
            get => _activeFilterName;
            set => SetProperty(ref _activeFilterName, value);
        }

        private OrderFilterType _activeFilter;
        public OrderFilterType ActiveFilter
        {
            get => _activeFilter;
            set
            {
                _activeFilter = value;
                if (_orderFilterTypeTitleMap.TryGetValue(_activeFilter, out var activeFilterName))
                {
                    ActiveFilterName = activeFilterName;
                }
            }
        }

        public MvxAsyncCommand OpenFilterCommand => new MvxAsyncCommand(OnOpenFilterAsync);

        public MvxAsyncCommand LoadOrdersCommand => new MvxAsyncCommand(RefreshOrdersAsync);

        public OrdersViewModel(INavigationService navigationService,
                               IDialogService dialogService,
                               IApiService apiService,
                               IMvxMessenger mvxMessenger,
                               ISettingsService settingsService,
                               IErrorHandleService errorHandleService)
            : base(Constants.Pagination.DefaultPaginationSize, navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
            _mvxMessenger = mvxMessenger;
            _settingsService = settingsService;

            _orderFilterTypeTitleMap = new Dictionary<OrderFilterType, string>
            {
                { OrderFilterType.All, Resources.OrdersView_Filter_AllTasks },
                { OrderFilterType.New, Resources.OrdersView_Filter_NewTasks },
                { OrderFilterType.InProgress, Resources.OrdersView_Filter_CurrentTasks },
                { OrderFilterType.MyOwn, Resources.OrdersView_Filter_MyTasks }
            };
        }

        public override Task Initialize()
        {
            ActiveFilter = OrderFilterType.All;
            return LoadOrdersCommand.ExecuteAsync();
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

        private Task RefreshOrdersAsync()
        {
            Reset();
            return LoadMoreItemsAsync();
        }

        private async Task OnOpenFilterAsync(CancellationToken arg)
        {
            var parameters = _orderFilterTypeTitleMap.Values.ToArray();
            var selectedFilterName = await DialogService.ShowMenuDialogAsync(parameters, Resources.Cancel);

            if (string.IsNullOrWhiteSpace(selectedFilterName) || selectedFilterName == Resources.Cancel)
                return;

            ActiveFilter = _orderFilterTypeTitleMap.FirstOrDefault(kv => kv.Value == selectedFilterName).Key;
            await LoadOrdersCommand.ExecuteAsync();
        }

        protected override async Task<int> LoadMoreItemsAsync(int page = 1, int pageSize = 20)
        {
            try
            {
                IsBusy = true;
                var orders = await ApiService.GetOrdersAsync(ActiveFilter, page, pageSize);
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

        private OrderItemViewModel ProduceOrderViewModel(OrderDataModel order)
        {
            return new OrderItemViewModel(NavigationService,
                                          _settingsService,
                                          _mvxMessenger,
                                          order.Id,
                                          order.Title,
                                          order.Customer?.Avatar,
                                          order.Customer?.Name,
                                          order.Price,
                                          order.ActiveTo,
                                          order.DurationInHours,
                                          order.Status ?? OrderStatusType.None,
                                          order.Customer?.Id);
        }

        private void Subscription()
        {
            _newOrderMessageToken = _mvxMessenger.SubscribeOnMainThread<NewOrderMessage>(OnNewOrderMessenger);
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

            foreach (var item in Items)
            {
                item.Dispose();
            }
        }

        private void OnNewOrderMessenger(NewOrderMessage newOrderMessage)
        {
            var newOrderItemViewModel = new OrderItemViewModel(
                    NavigationService,
                    _settingsService,
                    _mvxMessenger,
                    newOrderMessage.NewOrder.Id,
                    newOrderMessage.NewOrder.Title,
                    newOrderMessage.NewOrder.Customer?.Avatar,
                    newOrderMessage.NewOrder?.Customer?.Name,
                    newOrderMessage.NewOrder.Price,
                    newOrderMessage.NewOrder.ActiveTo,
                    newOrderMessage.NewOrder.DurationInHours,
                    newOrderMessage.NewOrder.Status ?? OrderStatusType.None,
                    newOrderMessage.NewOrder.Customer?.Id);
            Items.Add(newOrderItemViewModel);
        }

        private void OnRemoveOrderMessage(RemoveOrderMessage message)
        {
            var deletedItem = Items.FirstOrDefault(order => order.OrderId == message.OrderId);
            if (deletedItem == null)
                return;

            Items.Remove(deletedItem);
            deletedItem.Dispose();
        }
    }
}
