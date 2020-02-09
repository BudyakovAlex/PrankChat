using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Exceptions;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data.FilterTypes;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order
{
    public class OrdersViewModel : BaseViewModel
    {
        private readonly IMvxMessenger _mvxMessenger;
        private readonly ISettingsService _settingsService;
        private readonly IMvxLog _mvxLog;
        private readonly Dictionary<OrderFilterType, string> _orderFilterTypeTitleMap;

        private MvxSubscriptionToken _newOrderMessageToken;

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

        public MvxAsyncCommand LoadOrdersCommand => new MvxAsyncCommand(OnLoadOrdersAsync);

        public OrdersViewModel(INavigationService navigationService,
                               IDialogService dialogService,
                               IApiService apiService,
                               IMvxMessenger mvxMessenger,
                               IMvxLog mvxLog,
                               ISettingsService settingsService,
                               IErrorHandleService errorHandleService)
            : base(navigationService, errorHandleService, apiService, dialogService)
        {
            _mvxMessenger = mvxMessenger;
            _mvxLog = mvxLog;
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

        private async Task OnOpenFilterAsync(CancellationToken arg)
        {
            var parametres = _orderFilterTypeTitleMap.Values.ToArray();
            var selectedFilterName = await DialogService.ShowMenuDialogAsync(parametres, Resources.Cancel);

            if (string.IsNullOrWhiteSpace(selectedFilterName) || selectedFilterName == Resources.Cancel)
                return;

            ActiveFilter = _orderFilterTypeTitleMap.FirstOrDefault(x => x.Value == selectedFilterName).Key;
            await LoadOrdersCommand.ExecuteAsync();
        }

        private async Task OnLoadOrdersAsync()
        {
            try
            {
                IsBusy = true;

                var orders = await ApiService.GetOrdersAsync(ActiveFilter);
                Items.Clear();

                var orderItemViewModel = orders.Select(x =>
                    new OrderItemViewModel(
                        NavigationService,
                        _settingsService,
                        _mvxMessenger,
                        x.Id,
                        x.Title,
                        x.Customer?.Avatar,
                        x.Customer?.Name,
                        x.Price,
                        x.ActiveTo?.ToLocalTime(),
                        x.Status ?? OrderStatusType.None,
                        x.Customer?.Id));

                Items.SwitchTo(orderItemViewModel);
            }
            catch (Exception ex)
            {
                _mvxLog.DebugException($"{nameof(OrdersViewModel)}", ex);
                ErrorHandleService.HandleException(new UserVisibleException("Ошибка в загрузке заказов."));
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void Subscription()
        {
            _newOrderMessageToken = _mvxMessenger.SubscribeOnMainThread<NewOrderMessage>(OnNewOrderMessenger);
        }

        private void Unsubscription()
        {
            if (_newOrderMessageToken != null)
            {
                _mvxMessenger.Unsubscribe<NewOrderMessage>(_newOrderMessageToken);
                _newOrderMessageToken.Dispose();
            }

            foreach(var item in Items)
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
                    newOrderMessage.NewOrder.Status ?? OrderStatusType.None,
                    newOrderMessage.NewOrder.Customer?.Id);
            Items.Add(newOrderItemViewModel);
        }
    }
}
