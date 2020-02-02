using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.Mediaes;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
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
        private readonly IMediaService _mediaService;

        private int _orderId;
        private OrderDataModel _order;

        #region Profile

        public string ProfilePhotoUrl => _order?.Customer?.Avatar;

        public string ProfileName => _order?.Customer?.Name;

        public string ProfileShortName => _order?.Customer?.Name?.ToShortenName();

        #endregion Profile

        #region Video

        public string VideoUrl { get; set; } = "https://ksassets.timeincuk.net/wp/uploads/sites/55/2019/04/GettyImages-1136749971-920x584.jpg";

        public string VideoName => _order?.Title;

        public string VideoDetails => _order?.Description;

        #endregion Video

        #region Executor

        public string ExecutorPhotoUrl => _order?.Executor?.Avatar;

        public string ExecutorName => _order?.Executor?.Name;

        public string ExecutorShortName => _order?.Executor?.Name.ToShortenName();

        public string StartOrderDate => _order?.TakenToWorkAt?.ToShortDateString();

        #endregion Executor

        public string PriceValue => _order?.Price.ToString();

        public string TimeValue => _order?.FinishIn?.ToString("dd' : 'hh' : 'mm");

        public bool IsUserCustomer => _order?.Customer?.Id == _settingsService.User?.Id;

        public bool IsUserExecutor => _order?.Executor?.Id == _settingsService.User?.Id;

        public bool IsUserListener => !IsUserCustomer && !IsUserExecutor;

        public bool IsSubscribeAvailable => false; // IsUserListener;

        public bool IsUnsubscribeAvailable => false; // IsUserListener;

        public bool IsTakeOrderAvailable => !IsUserCustomer && _order?.Status == OrderStatusType.New;

        public bool IsCancelOrderAvailable => false;

        public bool IsExecuteOrderAvailable => false;

        public bool IsVideoLoadAvailable => _order?.Status != OrderStatusType.New && _order.Video == null && !IsUserCustomer;

        public bool IsVideoAvailable => _order.Video != null;

        public bool IsExecutorAvailable => _order?.Executor != null;

        public bool IsDecideVideoAvailable => false;

        public bool IsDecisionVideoAvailable => _order?.Status == OrderStatusType.WaitFinish;

        #region Commands

        public MvxAsyncCommand TakeOrderCommand => new MvxAsyncCommand(OnTakeOrderAsync);

        public MvxAsyncCommand SubscribeTheOrderCommand => new MvxAsyncCommand(OnSubscribeOrderAsync);

        public MvxAsyncCommand UnsubscribeOrderCommand => new MvxAsyncCommand(OnUnsubscribeOrderAsync);

        public MvxAsyncCommand YesCommand => new MvxAsyncCommand(OnYesAsync);

        public MvxAsyncCommand NoCommand => new MvxAsyncCommand(OnNoAsync);

        public MvxAsyncCommand LoadVideoCommand => new MvxAsyncCommand(OnLoadVideoAsync);

        public MvxAsyncCommand ExecuteOrderCommand => new MvxAsyncCommand(OnExecuteOrderAsync);

        public MvxAsyncCommand CancelOrderCommand => new MvxAsyncCommand(OnCancelOrderAsync);

        public MvxAsyncCommand ArqueOrderCommand => new MvxAsyncCommand(OnArgueOrderAsync);

        public MvxAsyncCommand AcceptOrderCommand => new MvxAsyncCommand(OnAcceptOrderAsync);

        #endregion Commands

        public OrderDetailsViewModel(INavigationService navigationService,
                                    IApiService apiService,
                                    IDialogService dialogService,
                                    IMvxLog mvxLog,
                                    ISettingsService settingsService,
                                    IMediaService mediaService) : base(navigationService)
        {
            _dialogService = dialogService;
            _apiService = apiService;
            _mvxLog = mvxLog;
            _settingsService = settingsService;
            _mediaService = mediaService;
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

            var order = await _apiService.TakeOrderAsync(_orderId);
            if (order != null)
            {
                _order.Status = OrderStatusType.InWork;
                _order.Executor = _settingsService.User;
                await RaiseAllPropertiesChanged();
                _dialogService.ShowToast("You have successfully taken the order!");
            }
        }

        private async Task OnSubscribeOrderAsync()
        {
            try
            {
                IsBusy = true;

                var order = await _apiService.SubscribeOrderAsync(_orderId);
                await RaiseAllPropertiesChanged();
            }
            catch (Exception ex)
            {
                _mvxLog.DebugException($"{nameof(OrderDetailsViewModel)}", ex);
                _dialogService.ShowToast("Can not subscribe order!");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnUnsubscribeOrderAsync()
        {
            try
            {
                IsBusy = true;

                var order = await _apiService.UnsubscribeOrderAsync(_orderId);
                await RaiseAllPropertiesChanged();
            }
            catch (Exception ex)
            {
                _mvxLog.DebugException($"{nameof(OrderDetailsViewModel)}", ex);
                _dialogService.ShowToast("Can not unsubscribe order!");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnLoadVideoAsync()
        {
            try
            {
                IsBusy = true;

                var file = await _mediaService.PickVideoAsync();
                if (file == null)
                    return;

                var video = await _apiService.SendVideoAsync(_orderId, file.Path, _order?.Title, _order?.Description);
                if (video != null)
                {
                    _order.Video = video;
                    await RaiseAllPropertiesChanged();
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnArgueOrderAsync()
        {
            try
            {
                IsBusy = true;

                _order = await _apiService.ArguebeOrderAsync(_orderId);
                await RaiseAllPropertiesChanged();
            }
            catch (Exception ex)
            {
                _mvxLog.DebugException($"{nameof(OrderDetailsViewModel)}", ex);
                _dialogService.ShowToast("Can not argue order!");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnAcceptOrderAsync()
        {
            try
            {
                IsBusy = true;

                _order = await _apiService.AcceptOrderAsync(_orderId);
                await RaiseAllPropertiesChanged();
            }
            catch (Exception ex)
            {
                _mvxLog.DebugException($"{nameof(OrderDetailsViewModel)}", ex);
                _dialogService.ShowToast("Can not accept order!");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnCancelOrderAsync()
        {
            var result = await _dialogService.ShowConfirmAsync(Resources.OrderDetails_View_Cancel_Title,
                                                   Resources.Attention,
                                                   Resources.Ok,
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
