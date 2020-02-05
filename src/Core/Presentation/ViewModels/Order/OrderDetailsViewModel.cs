using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Mediaes;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Exceptions;
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
        private readonly IMvxLog _mvxLog;
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

        #region Decide

        public int LikesCount { get; set; } = 100;

        public int DisikesCount { get; set; } = 100;

        public string YesText => $"{Resources.OrderDetailsView_Yes_Button} {LikesCount}";

        public string NoText => $"{Resources.OrderDetailsView_No_Button} {DisikesCount}";

        private bool _isNoSelected = true;
        public bool IsNoSelected
        {
            get => _isNoSelected;
            set => SetProperty(ref _isNoSelected, value);
        }

        private bool _isYesSelected;
        public bool IsYesSelected
        {
            get => _isYesSelected;
            set => SetProperty(ref _isYesSelected, value);
        }

        #endregion

        public string PriceValue => _order?.Price.ToString();

        public string TimeValue => _order?.FinishIn?.ToString("dd' : 'hh' : 'mm");

        public bool IsUserCustomer => _order?.Customer?.Id == _settingsService.User?.Id;

        public bool IsUserExecutor => _order?.Executor?.Id == _settingsService.User?.Id;

        public bool IsUserGuest => !IsUserCustomer && !IsUserExecutor;

        public bool IsSubscribeAvailable => false; // IsUserListener;

        public bool IsUnsubscribeAvailable => false; // IsUserListener;

        public bool IsTakeOrderAvailable => !IsUserCustomer && _order?.Status == OrderStatusType.New;

        public bool IsCancelOrderAvailable => false;

        public bool IsExecuteOrderAvailable => false;

        public bool IsVideoLoadAvailable => _order?.Status == OrderStatusType.InWork && IsUserExecutor;

        public bool IsVideoAvailable => _order.Video != null;

        public bool IsExecutorAvailable => _order?.Executor != null;

        public bool IsDecideVideoAvailable => _order?.Status == OrderStatusType.InArbitration;// && IsUserGuest;

        public bool IsDecisionVideoAvailable => _order?.Status == OrderStatusType.WaitFinish && IsUserCustomer;

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
                                    IMvxLog mvxLog,
                                    ISettingsService settingsService,
                                    IMediaService mediaService,
                                    IErrorHandleService errorHandleService,
                                    IApiService apiService,
                                    IDialogService dialogService)
            : base(navigationService, errorHandleService, apiService, dialogService)
        {
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

                _order = await ApiService.GetOrderDetailsAsync(_orderId);
                await RaiseAllPropertiesChanged();
            }
            catch (Exception ex)
            {
                _mvxLog.DebugException($"{nameof(OrderDetailsViewModel)}", ex);
                ErrorHandleService.HandleException(new UserVisibleException("Проблема с загрузкой детальной страницы заказ."));
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnTakeOrderAsync()
        {
            var result = await DialogService.ShowConfirmAsync(Resources.OrderDetailsView_TakeOrderQuestion,
                                                               Resources.Attention,
                                                               Resources.OrderDetailsView_TakeOrderTitle,
                                                               Resources.Cancel);
            if (!result)
                return;

            var order = await ApiService.TakeOrderAsync(_orderId);
            if (order != null)
            {
                _order.Status = OrderStatusType.InWork;
                _order.Executor = _settingsService.User;
                await RaiseAllPropertiesChanged();
            }
        }

        private async Task OnSubscribeOrderAsync()
        {
            try
            {
                IsBusy = true;

                var order = await ApiService.SubscribeOrderAsync(_orderId);
                await RaiseAllPropertiesChanged();
            }
            catch (Exception ex)
            {
                _mvxLog.DebugException($"{nameof(OrderDetailsViewModel)}", ex);
                ErrorHandleService.HandleException(new UserVisibleException("Неудачная попытка подписаться на заказ."));
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

                var order = await ApiService.UnsubscribeOrderAsync(_orderId);
                await RaiseAllPropertiesChanged();
            }
            catch (Exception ex)
            {
                _mvxLog.DebugException($"{nameof(OrderDetailsViewModel)}", ex);
                ErrorHandleService.HandleException(new UserVisibleException("Неудачная попытка отписаться от заказ."));
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

                var video = await ApiService.SendVideoAsync(_orderId, file.Path, _order?.Title, _order?.Description);
                if (video != null)
                {
                    _order.Video = video;
                    _order.Status = OrderStatusType.WaitFinish;
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

                var order = await ApiService.ArgueOrderAsync(_orderId);
                if (order != null)
                {
                    await RaiseAllPropertiesChanged();
                }
            }
            catch (Exception ex)
            {
                _mvxLog.DebugException($"{nameof(OrderDetailsViewModel)}", ex);
                ErrorHandleService.HandleException(new UserVisibleException("Неудачная отправка заказа на спор."));
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

                _order = await ApiService.AcceptOrderAsync(_orderId);
                await RaiseAllPropertiesChanged();
            }
            catch (Exception ex)
            {
                _mvxLog.DebugException($"{nameof(OrderDetailsViewModel)}", ex);
                ErrorHandleService.HandleException(new UserVisibleException("Ошибка в подтверждении заказа."));
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnCancelOrderAsync()
        {
            var result = await DialogService.ShowConfirmAsync(Resources.OrderDetails_View_Cancel_Title,
                                                   Resources.Attention,
                                                   Resources.Ok,
                                                   Resources.Cancel);

            if (!result)
                return;

            await ApiService.CancelOrderAsync(_orderId);
        }

        private Task OnExecuteOrderAsync()
        {
            return Task.CompletedTask;
        }

        private async Task OnYesAsync()
        {
            IsYesSelected = !IsYesSelected;
            //var order = await ApiService.VoteVideoAsync(_orderId, true);
        }

        private async Task OnNoAsync()
        {
            IsNoSelected = !IsNoSelected;
            //var order = await ApiService.VoteVideoAsync(_orderId, false);
        }
    }
}
