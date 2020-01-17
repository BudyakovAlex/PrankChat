using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Messengers;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order
{
    public class OrdersViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IApiService _apiService;
        private readonly IMvxMessenger _mvxMessenger;
        private readonly ISettingsService _settingsService;

        private MvxSubscriptionToken _newOrderMessengertoken;

        public MvxObservableCollection<OrderItemViewModel> Items { get; } = new MvxObservableCollection<OrderItemViewModel>();

        private string _activeFilterName;
        public string ActiveFilterName
        {
            get => _activeFilterName;
            set => SetProperty(ref _activeFilterName, value);
        }

        public MvxAsyncCommand OpenFilterCommand => new MvxAsyncCommand(OnOpenFilterAsync);

        public MvxAsyncCommand LoadOrdersCommand => new MvxAsyncCommand(OnLoadOrdersAsync);

        public OrdersViewModel(INavigationService navigationService,
                               IDialogService dialogService,
                               IApiService apiService,
                               IMvxMessenger mvxMessenger)
            : base(navigationService)
        {
            _dialogService = dialogService;
            _apiService = apiService;
            _mvxMessenger = mvxMessenger;
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
                Resources.OrdersView_Filter_MyTasks,
            });

            if (string.IsNullOrWhiteSpace(selectedFilter) || selectedFilter == Resources.Cancel)
                return;

            ActiveFilterName = selectedFilter;
        }

        private async Task OnLoadOrdersAsync()
        {
            var orders = await _apiService.GetOrdersAsync();

            var orderItemViewModel = orders.Select(x =>
                new OrderItemViewModel(
                    NavigationService,
                    _settingsService,
                    x.Title,
                    "https://images.pexels.com/photos/2092709/pexels-photo-2092709.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500",
                    x.PriceTo,
                    DateTime.Now - x.ActiveTo.ToLocalTime(),
                    x.Status));

            Items.AddRange(orderItemViewModel);
        }

        private void Subscription()
        {
            _newOrderMessengertoken = _mvxMessenger.SubscribeOnMainThread<NewOrderMessenger>(OnNewOrderMessenger);
        }

        private void Unsubscription()
        {
            if (_newOrderMessengertoken != null)
            {
                _mvxMessenger.Unsubscribe<NewOrderMessenger>(_newOrderMessengertoken);
                _newOrderMessengertoken.Dispose();
            }
        }

        private void OnNewOrderMessenger(NewOrderMessenger newOrder)
        {
            var newOrderItemViewModel = new OrderItemViewModel(
                    NavigationService,
                    _settingsService,
                    newOrder.NewOrder.Title,
                    "https://images.pexels.com/photos/2092709/pexels-photo-2092709.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500",
                    newOrder.NewOrder.PriceTo,
                    DateTime.Now - newOrder.NewOrder.ActiveTo.ToLocalTime(),
                    newOrder.NewOrder.Status);
            Items.Add(newOrderItemViewModel);
        }
    }
}
