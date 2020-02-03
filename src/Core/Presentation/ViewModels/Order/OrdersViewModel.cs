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
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data.FilterTypes;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order
{
    public class OrdersViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IApiService _apiService;
        private readonly IMvxMessenger _mvxMessenger;
        private readonly ISettingsService _settingsService;
        private readonly IMvxLog _mvxLog;
        private readonly Dictionary<string, OrderFilterType> _orderFilterTypeTitleMap;

        private MvxSubscriptionToken _newOrderMessageToken;

        public MvxObservableCollection<OrderItemViewModel> Items { get; } = new MvxObservableCollection<OrderItemViewModel>();

        private string _activeFilterName;
        public string ActiveFilterName
        {
            get => _activeFilterName;
            set
            {
                SetProperty(ref _activeFilterName, value);
                LoadOrdersCommand.ExecuteAsync().FireAndForget();
            }
        }

        public MvxAsyncCommand OpenFilterCommand => new MvxAsyncCommand(OnOpenFilterAsync);

        public MvxAsyncCommand LoadOrdersCommand => new MvxAsyncCommand(OnLoadOrdersAsync);

        public OrdersViewModel(INavigationService navigationService,
                               IDialogService dialogService,
                               IApiService apiService,
                               IMvxMessenger mvxMessenger,
                               IMvxLog mvxLog,
                               ISettingsService settingsService)
            : base(navigationService)
        {
            _dialogService = dialogService;
            _apiService = apiService;
            _mvxMessenger = mvxMessenger;
            _mvxLog = mvxLog;
            _settingsService = settingsService;

            _orderFilterTypeTitleMap = new Dictionary<string, OrderFilterType>
            {
                { Resources.OrdersView_Filter_AllTasks, OrderFilterType.All },
                { Resources.OrdersView_Filter_NewTasks, OrderFilterType.New },
                { Resources.OrdersView_Filter_CurrentTasks, OrderFilterType.InProgress },
                { Resources.OrdersView_Filter_MyTasks, OrderFilterType.MyOwn }
            };
        }

        public override Task Initialize()
        {
            return LoadOrdersCommand.ExecuteAsync();
        }

        public override void Prepare()
        {
            ActiveFilterName = Resources.OrdersView_Filter_AllTasks;
            base.Prepare();
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
            var selectedFilter = await _dialogService.ShowMenuDialogAsync(new[]
            {
                Resources.OrdersView_Filter_AllTasks,
                Resources.OrdersView_Filter_NewTasks,
                Resources.OrdersView_Filter_CurrentTasks,
                Resources.OrdersView_Filter_MyTasks
            });

            if (string.IsNullOrWhiteSpace(selectedFilter) || selectedFilter == Resources.Cancel)
                return;

            ActiveFilterName = selectedFilter;
        }

        private async Task OnLoadOrdersAsync()
        {
            try
            {
                IsBusy = true;

                _orderFilterTypeTitleMap.TryGetValue(ActiveFilterName, out var orderFilterType);

                var orders = await _apiService.GetOrdersAsync(orderFilterType);
                if (Items.Count != 0)
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
                        x.ActiveTo,
                        x.Status ?? OrderStatusType.None,
                        x.Customer?.Id));

                Items.SwitchTo(orderItemViewModel);
            }
            catch (Exception ex)
            {
                _mvxLog.DebugException($"{nameof(OrdersViewModel)}", ex);
                _dialogService.ShowToast("Can not load order details!");
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
