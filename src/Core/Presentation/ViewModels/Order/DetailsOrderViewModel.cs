using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order
{
    public class OrderDetailsViewModel : BaseViewModel, IMvxViewModel<OrderDetailsNavigationParameter>
    {
        private readonly IApiService _apiService;
        private readonly IMvxLog _mvxLog;
        private readonly IDialogService _dialogService;
        private readonly ISettingsService _settingsService;

        private int _orderId;
        private OrderDataModel _order;

        #region Profile

        public string ProfilePhotoUrl => _order?.Customer?.Avatar;

        public string ProfileName => _order?.Customer?.Name;

        #endregion

        #region Video

        public string VideoUrl { get; set; } = "https://ksassets.timeincuk.net/wp/uploads/sites/55/2019/04/GettyImages-1136749971-920x584.jpg";

        public string VideoName => _order?.Title;

        public string VideoDetails => _order?.Description;

        #endregion

        #region Executor

        public string ExecutorPhotoUrl => _order?.Executor?.FirstOrDefault()?.Avatar;

        public string ExecutorName => _order?.Executor?.FirstOrDefault()?.Name;

        public string StartOrderDate => _order?.CreatedAt?.ToShortDateString();

        #endregion

        public string PriceValue => _order?.Price.ToString();

        public string TimeValue => _order?.FinishIn?.ToString("dd' : 'hh' : 'mm");

        public bool IsUserCustomer => _order?.Customer?.Id == _settingsService.User?.Id;

        public bool IsUserExecutor => _order?.Executor?.FirstOrDefault()?.Id == _settingsService.User?.Id;

        public bool IsUserListener => !IsUserCustomer && !IsUserExecutor;

        public bool IsTakeOrderAvailable => !IsUserCustomer && _order?.Status == OrderStatusType.New;

        public bool IsSubscribeAvailable => IsUserListener;

        public bool IsUnsubscribeAvailable => IsUserListener;

        public MvxAsyncCommand TakeOrderCommand => new MvxAsyncCommand(OnTakeOrderAsync);

        public MvxAsyncCommand SubscribeTheOrderCommand => new MvxAsyncCommand(OnSubscribeOrderAsync);

        public MvxAsyncCommand UnsubscribeOrderCommand => new MvxAsyncCommand(OnUnsubscribeOrderAsync);

        public MvxAsyncCommand YesCommand => new MvxAsyncCommand(OnYesAsync);

        public MvxAsyncCommand NoCommand => new MvxAsyncCommand(OnNoAsync);

        public MvxAsyncCommand DownloadOrderCommand => new MvxAsyncCommand(OnDownloadOrderAsync);

        public MvxAsyncCommand ExecuteOrderCommand => new MvxAsyncCommand(OnExecuteOrderAsync);

        public MvxAsyncCommand CancelOrderCommand => new MvxAsyncCommand(OnCancelOrderAsync);

        public MvxAsyncCommand ArqueOrderCommand => new MvxAsyncCommand(OnArqueOrderAsync);

        public MvxAsyncCommand AcceptOrderCommand => new MvxAsyncCommand(OnAcceptOrderAsync);

        public OrderDetailsViewModel(INavigationService navigationService,
                                    IApiService apiService,
                                    IDialogService dialogService,
                                    IMvxLog mvxLog,
                                    ISettingsService settingsService) : base(navigationService)
        {
            _dialogService = dialogService;
            _apiService = apiService;
            _mvxLog = mvxLog;
            _settingsService = settingsService;
        }

        public void Prepare(OrderDetailsNavigationParameter parameter)
        {
            _orderId = parameter.OrderId;
        }

        public override Task Initialize()
        {
            return LoadOrderDetails();
        }

        private async Task LoadOrderDetails()
        {
            try
            {
                IsBusy = true;

                _order = await _apiService.GetOrderDetailsAsync(_orderId);
                await RaiseAllPropertiesChanged();
            }
            catch (Exception ex)
            {
                _mvxLog.DebugException($"{nameof(OrderDetailsViewModel)}", ex);
                _dialogService.ShowToast("Can not load order details!");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnTakeOrderAsync()
        {
            var result = await _dialogService.ShowConfirmAsync(Resources.OrderDetailsView_TakeOrderQuestion,
                                                               Resources.Attention,
                                                               Resources.OrderDetailsView_TakeOrderTitle,
                                                               Resources.Cancel);
            if (!result)
                return;

            var lol = await _apiService.TakeOrderAsync(_orderId, _settingsService.User.Id);
        }

        private Task OnSubscribeOrderAsync()
        {
            return Task.CompletedTask;
        }

        private Task OnUnsubscribeOrderAsync()
        {
            return Task.CompletedTask;
        }

        private Task OnDownloadOrderAsync()
        {
            return Task.CompletedTask;
        }

        private Task OnArqueOrderAsync()
        {
            return Task.CompletedTask;
        }

        private Task OnAcceptOrderAsync()
        {
            return Task.CompletedTask;
        }

        private async Task OnCancelOrderAsync()
        {
            var result = await _dialogService.ShowConfirmAsync("Вы уверены что хотите отменить заказ?",
                                                   Resources.Attention,
                                                   "Отменить",
                                                   Resources.Cancel);

            if (!result)
                return;

            await _apiService.CancelOrderAsync(_orderId);
        }

        private Task OnExecuteOrderAsync()
        {
            return Task.CompletedTask;
        }

        private Task OnYesAsync()
        {
            return Task.CompletedTask;
        }

        private Task OnNoAsync()
        {
            return Task.CompletedTask;
        }
    }
}
