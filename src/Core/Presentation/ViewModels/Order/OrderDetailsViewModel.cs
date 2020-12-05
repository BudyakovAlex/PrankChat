using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages;
using PrankChat.Mobile.Core.ApplicationServices.Platforms;
using PrankChat.Mobile.Core.ApplicationServices.Timer;
using PrankChat.Mobile.Core.Exceptions;
using PrankChat.Mobile.Core.Exceptions.Network;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Managers.Orders;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.Navigation.Results;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order.Sections;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order.Sections.Abstract;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order
{
    public class OrderDetailsViewModel : BasePageViewModel, IMvxViewModel<OrderDetailsNavigationParameter, OrderDetailsResult>
    {
        private readonly IOrdersManager _ordersManager;
        private readonly IPlatformService _platformService;

        private int _orderId;
        private int _timerThicksCount;

        private readonly BaseOrderDetailsSectionViewModel[] _sections;

        public OrderDetailsViewModel(IOrdersManager ordersManager, IPlatformService platformService)
        {
            _ordersManager = ordersManager;
            _platformService = platformService;

            TakeOrderCommand = new MvxAsyncCommand(TakeOrderAsync);
            SubscribeOrderCommand = new MvxAsyncCommand(() => ExecutionStateWrapper.WrapAsync(SubscribeOrderAsync));
            UnsubscribeOrderCommand = new MvxAsyncCommand(() => ExecutionStateWrapper.WrapAsync(UnsubscribeOrderAsync));
            YesCommand = new MvxAsyncCommand(() => ExecutionStateWrapper.WrapAsync(YesAsync));
            NoCommand = new MvxAsyncCommand(() => ExecutionStateWrapper.WrapAsync(NoAsync));
            ExecuteOrderCommand = new MvxAsyncCommand(() => ExecutionStateWrapper.WrapAsync(ExecuteOrderAsync));
            CancelOrderCommand = new MvxAsyncCommand(() => ExecutionStateWrapper.WrapAsync(CancelOrderAsync));
            ArqueOrderCommand = new MvxAsyncCommand(() => ExecutionStateWrapper.WrapAsync(ArgueOrderAsync));
            AcceptOrderCommand = new MvxAsyncCommand(() => ExecutionStateWrapper.WrapAsync(AcceptOrderAsync));

            LoadOrderDetailsCommand = new MvxAsyncCommand(LoadOrderDetailsAsync);
            OpenSettingsCommand = new MvxAsyncCommand(OpenSettingsAsync);

            Messenger.Subscribe<TimerTickMessage>(OnTimerTick, MvxReference.Strong).DisposeWith(Disposables);
            _sections = new BaseOrderDetailsSectionViewModel[]
            {
                CustomerSectionViewModel = Mvx.IoCProvider.IoCConstruct<OrderDetailsCustomerSectionViewModel>(),
                ExecutorSectionViewModel = Mvx.IoCProvider.IoCConstruct<OrderDetailsExecutorSectionViewModel>(),
                VideoSectionViewModel = Mvx.IoCProvider.IoCConstruct<OrderDetailsVideoSectionViewModel>()
            };

            VideoSectionViewModel.RefreshDataFunc = LoadOrderDetailsAsync;
        }

        public int LikesCount => Order?.PositiveArbitrationValuesCount ?? 0;

        public int DisikesCount => Order?.NegativeArbitrationValuesCount ?? 0;

        public string YesText => IsDecideEnabled ? Resources.OrderDetailsView_Yes_Button : LikesCount.ToString();

        public string NoText => IsDecideEnabled ? Resources.OrderDetailsView_No_Button : DisikesCount.ToString();

        public ArbitrationValueType? SelectedArbitration => Order?.MyArbitrationValue;

        public bool IsDecideEnabled => SelectedArbitration == null && IsUserGuest;

        private bool _isNoSelected;
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

        private OrderDataModel _order;
        public OrderDataModel Order
        {
            get => _order;
            set
            {
                _order = value;
                _sections.ForEach(section => section.SetOrder(value));
            }
        }

        public IMvxAsyncCommand TakeOrderCommand { get; }
        public IMvxAsyncCommand SubscribeOrderCommand { get; }
        public IMvxAsyncCommand UnsubscribeOrderCommand { get; }
        public IMvxAsyncCommand YesCommand { get; }
        public IMvxAsyncCommand NoCommand { get; }
        public IMvxAsyncCommand ExecuteOrderCommand { get; }
        public IMvxAsyncCommand CancelOrderCommand { get; }
        public IMvxAsyncCommand ArqueOrderCommand { get; }
        public IMvxAsyncCommand AcceptOrderCommand { get; }
        public IMvxAsyncCommand LoadOrderDetailsCommand { get; }
        public IMvxAsyncCommand OpenSettingsCommand { get; }

        public OrderDetailsCustomerSectionViewModel CustomerSectionViewModel { get; }
        public OrderDetailsExecutorSectionViewModel ExecutorSectionViewModel { get; }
        public OrderDetailsVideoSectionViewModel VideoSectionViewModel { get; }

        private TimeSpan? TimeValue => Order?.GetActiveOrderTime();

        public string PriceValue => Order?.Price.ToPriceString();

        public string TimeDaysValue => TimeValue?.Days.ToString("00");

        public string TimeHourValue => TimeValue?.Hours.ToString("00");

        public string TimeMinutesValue => TimeValue?.Minutes.ToString("00");

        public string StartOrderDate => _order?.TakenToWorkAt?.ToShortDateString();

        public bool IsUserGuest => !CustomerSectionViewModel.IsUserCustomer && !ExecutorSectionViewModel.IsUserExecutor;

        public bool IsSubscribeAvailable => false; // IsUserListener;

        public bool IsUnsubscribeAvailable => false; // IsUserListener;

        public bool IsTakeOrderAvailable => !CustomerSectionViewModel.IsUserCustomer && Order?.Status == OrderStatusType.Active;

        public bool IsAnyOrderActionAvailable => IsTakeOrderAvailable || IsSubscribeAvailable || IsUnsubscribeAvailable;

        public bool IsCancelOrderAvailable => CustomerSectionViewModel.IsUserCustomer && Order?.Status == OrderStatusType.Active;

        public bool IsExecuteOrderAvailable => false;

        public bool IsTimeAvailable => Order.CheckIsTimeAvailable();

        public TaskCompletionSource<object> CloseCompletionSource { get; set; } = new TaskCompletionSource<object>();

        public bool IsHiddenOrder => Order?.OrderCategory == OrderCategory.Private &&
                                     !CustomerSectionViewModel.IsUserCustomer &&
                                     !ExecutorSectionViewModel.IsUserExecutor;

        public string OrderTitle => Order?.Title;

        public string OrderDescription => Order?.Description;

        public void Prepare(OrderDetailsNavigationParameter parameter)
        {
            _orderId = parameter.OrderId;
            VideoSectionViewModel.SetFullScreenVideos(parameter.FullScreenVideos, parameter.CurrentIndex);
        }

        public override Task InitializeAsync()
        {
            return LoadOrderDetailsCommand.ExecuteAsync();
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            if (viewFinishing &&
                CloseCompletionSource != null &&
                !CloseCompletionSource.Task.IsCompleted &&
                !CloseCompletionSource.Task.IsFaulted)
            {
                CloseCompletionSource?.SetResult(new OrderDetailsResult(Order));
            }

            base.ViewDestroy(viewFinishing);
        }

        private new async void OnTimerTick(TimerTickMessage msg)
        {
            _timerThicksCount++;
            if (_timerThicksCount >= 20)
            {
                _timerThicksCount = 0;
                try
                {
                    var refreshedOrder = await _ordersManager.GetOrderDetailsAsync(_orderId);
                    if (refreshedOrder is null)
                    {
                        return;
                    }

                    Order = refreshedOrder;
                    VideoSectionViewModel.RefreshFullScreenVideo();
                    await RaiseAllPropertiesChanged();

                    IsNoSelected = SelectedArbitration == ArbitrationValueType.Negative;
                    IsYesSelected = SelectedArbitration == ArbitrationValueType.Positive;
                }
                catch (Exception ex)
                {
                    ErrorHandleService.HandleException(ex);
                    ErrorHandleService.LogError(this, "Error on loading order page.");
                }
            }
        }

        private async Task LoadOrderDetailsAsync()
        {
            try
            {
                var refreshedOrder = await _ordersManager.GetOrderDetailsAsync(_orderId);
                if (refreshedOrder is null)
                {
                    return;
                }

                Order = refreshedOrder;
                VideoSectionViewModel.RefreshFullScreenVideo();
                await RaiseAllPropertiesChanged();

                IsNoSelected = SelectedArbitration == ArbitrationValueType.Negative;
                IsYesSelected = SelectedArbitration == ArbitrationValueType.Positive;
            }
            catch (Exception ex)
            {
                ErrorHandleService.HandleException(ex);
                ErrorHandleService.LogError(this, "Error on loading order page.");
            }
        }

        private async Task TakeOrderAsync()
        {
            var result = await DialogService.ShowConfirmAsync(Resources.OrderDetailsView_TakeOrderQuestion,
                                                              Resources.Attention,
                                                              Resources.OrderDetailsView_TakeOrderTitle,
                                                              Resources.Cancel);
            if (!result)
            {
                return;
            }

            try
            {
                ErrorHandleService.SuspendServerErrorsHandling();
                var takenOrder = await _ordersManager.TakeOrderAsync(_orderId);
                if (takenOrder != null && Order != null)
                {
                    Order.Status = takenOrder.Status;
                    Order.Executor = SettingsService.User;
                    Order.ActiveTo = takenOrder.ActiveTo;

                    await Task.WhenAll(_sections.Select(item => item.RaiseAllPropertiesChanged()));
                    await RaiseAllPropertiesChanged();
                }

                if (Order is null)
                {
                    return;
                }

                Messenger.Publish(new OrderChangedMessage(this, Order));
            }
            catch (NetworkException ex) when (ex.InnerException is ProblemDetailsDataModel problemDetails && problemDetails?.CodeError == Constants.ErrorCodes.LowBalance)
            {
                await HandleLowBalanceExceptionAsync(ex);
            }
            catch (Exception ex)
            {
                ErrorHandleService.ResumeServerErrorsHandling();
                Messenger.Publish(new ServerErrorMessage(this, ex));
            }
            finally
            {
                ErrorHandleService.ResumeServerErrorsHandling();
            }
        }

        private async Task HandleLowBalanceExceptionAsync(Exception exception)
        {
            var canRefil = await DialogService.ShowConfirmAsync(exception.Message, Resources.Attention, Resources.ProfileView_Refill, Resources.Cancel);
            if (!canRefil)
            {
                return;
            }

            await NavigationService.ShowRefillView();
        }

        private async Task SubscribeOrderAsync()
        {
            try
            {
                Order = await _ordersManager.SubscribeOrderAsync(_orderId);
                await RaiseAllPropertiesChanged();
                Messenger.Publish(new OrderChangedMessage(this, Order));
            }
            catch (Exception ex)
            {
                ErrorHandleService.HandleException(ex);
                ErrorHandleService.LogError(this, "Order subscription failed.");
            }
        }

        private async Task UnsubscribeOrderAsync()
        {
            try
            {
                Order = await _ordersManager.UnsubscribeOrderAsync(_orderId);
                await RaiseAllPropertiesChanged();
                Messenger.Publish(new OrderChangedMessage(this, Order));
            }
            catch (Exception ex)
            {
                ErrorHandleService.HandleException(ex);
                ErrorHandleService.LogError(this, "Error on order unsubscription.");
            }
        }

        private async Task ArgueOrderAsync()
        {
            try
            {
                var order = await _ordersManager.ArgueOrderAsync(_orderId);
                if (order != null && Order != null)
                {
                    Order.Status = order.Status;
                    await Task.WhenAll(_sections.Select(item => item.RaiseAllPropertiesChanged()));
                    await RaiseAllPropertiesChanged();
                    Messenger.Publish(new OrderChangedMessage(this, Order));
                }
            }
            catch (Exception ex)
            {
                ErrorHandleService.HandleException(ex);
                ErrorHandleService.LogError(this, "Error on argue initialization.", ex);
            }
        }

        private async Task AcceptOrderAsync()
        {
            try
            {
                var order = await _ordersManager.AcceptOrderAsync(_orderId);
                if (order != null && Order != null)
                {
                    Order.Status = order.Status;

                    await Task.WhenAll(_sections.Select(item => item.RaiseAllPropertiesChanged()));
                    await RaiseAllPropertiesChanged();
                    Messenger.Publish(new OrderChangedMessage(this, order));
                }
            }
            catch (Exception ex)
            {
                ErrorHandleService.HandleException(ex);
                ErrorHandleService.LogError(this, "Error on accept order.", ex);
            }
        }

        private async Task CancelOrderAsync()
        {
            var result = await DialogService.ShowConfirmAsync(Resources.OrderDetails_View_Cancel_Title,
                                                              Resources.Attention,
                                                              Resources.Ok,
                                                              Resources.Cancel);
            if (!result)
            {
                return;
            }

            var canceledOrder = await _ordersManager.CancelOrderAsync(_orderId);
            if (canceledOrder != null && Order != null)
            {
                Order.Status = canceledOrder.Status;
                await Task.WhenAll(_sections.Select(item => item.RaiseAllPropertiesChanged()));
                await RaiseAllPropertiesChanged();
                Messenger.Publish(new OrderChangedMessage(this, Order));
            }
        }

        private Task ExecuteOrderAsync()
        {
            return Task.CompletedTask;
        }

        private async Task YesAsync()
        {
            if (!IsDecideEnabled || !IsUserGuest)
            {
                return;
            }

            try
            {
                IsYesSelected = !IsYesSelected;
                var votedOrder = await _ordersManager.VoteVideoAsync(_orderId, ArbitrationValueType.Positive);
                if (votedOrder != null && Order != null)
                {
                    Order.MyArbitrationValue = votedOrder.MyArbitrationValue;
                    Order.PositiveArbitrationValuesCount = votedOrder.PositiveArbitrationValuesCount;
                    Order.NegativeArbitrationValuesCount = votedOrder.NegativeArbitrationValuesCount;
                    await RaiseAllPropertiesChanged();
                    Messenger.Publish(new OrderChangedMessage(this, votedOrder));
                }
            }
            catch (Exception ex)
            {
                ErrorHandleService.HandleException(ex);
                ErrorHandleService.LogError(this, "Order confirmation error.", ex);

                IsYesSelected = !IsYesSelected;
            }
        }

        private async Task NoAsync()
        {
            if (!IsDecideEnabled || !IsUserGuest)
            {
                return;
            }

            try
            {
                IsNoSelected = !IsNoSelected;
                var votedOrder = await _ordersManager.VoteVideoAsync(_orderId, ArbitrationValueType.Negative);
                if (votedOrder != null && Order != null)
                {
                    Order.MyArbitrationValue = votedOrder.MyArbitrationValue;
                    Order.PositiveArbitrationValuesCount = votedOrder.PositiveArbitrationValuesCount;
                    Order.NegativeArbitrationValuesCount = votedOrder.NegativeArbitrationValuesCount;
                    await RaiseAllPropertiesChanged();
                    Messenger.Publish(new OrderChangedMessage(this, votedOrder));
                }
            }
            catch (Exception ex)
            {
                ErrorHandleService.HandleException(ex);
                ErrorHandleService.LogError(this, "Error on order voting.");

                IsNoSelected = !IsNoSelected;
            }
        }

        private async Task OpenSettingsAsync()
        {
            var result = await DialogService.ShowMenuDialogAsync(new[]
            {
                Resources.Publication_Item_Complain
                // TODO: uncomment this when functionality will be available
                //Resources.Publication_Item_Copy_Link
            });

            if (string.IsNullOrWhiteSpace(result))
            {
                return;
            }

            if (result == Resources.Publication_Item_Complain)
            {
                if (!IsUserSessionInitialized)
                {
                    await NavigationService.ShowLoginView();
                    return;
                }

                var text = await GetComplaintTextAsync();
                if (string.IsNullOrWhiteSpace(text))
                {
                    return;
                }

                await _ordersManager.ComplainOrderAsync(_orderId, text, text);
                DialogService.ShowToast(Resources.Complaint_Complete_Message, ToastType.Positive);
                return;
            }

            if (result == Resources.Publication_Item_Copy_Link)
            {
                await _platformService.CopyTextAsync(Order?.Video?.ShareUri);
                DialogService.ShowToast(Resources.LinkCopied, ToastType.Positive);
                return;
            }
        }

        private async Task<string> GetComplaintTextAsync()
        {
            var result = await DialogService.ShowMenuDialogAsync(Constants.ComplaintConstants.CommonCompetitionAims);
            return result;
        }
    }
}
