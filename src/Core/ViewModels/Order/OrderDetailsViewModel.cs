using MvvmCross;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Data.Enums;
using PrankChat.Mobile.Core.Exceptions;
using PrankChat.Mobile.Core.Exceptions.Network;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Managers.Orders;
using PrankChat.Mobile.Core.Managers.Users;
using PrankChat.Mobile.Core.Messages;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.ViewModels.Abstract;
using PrankChat.Mobile.Core.ViewModels.Order.Sections;
using PrankChat.Mobile.Core.ViewModels.Order.Sections.Abstract;
using PrankChat.Mobile.Core.ViewModels.Parameters;
using PrankChat.Mobile.Core.ViewModels.Profile;
using PrankChat.Mobile.Core.ViewModels.Profile.Cashbox;
using PrankChat.Mobile.Core.ViewModels.Registration;
using PrankChat.Mobile.Core.ViewModels.Results;
using PrankChat.Mobile.Core.Services.ErrorHandling.Messages;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.ViewModels.Order
{
    public class OrderDetailsViewModel : BasePageViewModel<OrderDetailsNavigationParameter, bool>
    {
        private readonly IOrdersManager _ordersManager;
        private readonly IUsersManager _usersManager;

        private readonly BaseOrderDetailsSectionViewModel[] _sections;

        private int _orderId;
        private int _timerThicksCount;

        private bool _hasChanges;

        public OrderDetailsViewModel(IOrdersManager ordersManager, IUsersManager usersManager)
        {
            _ordersManager = ordersManager;
            _usersManager = usersManager;

            TakeOrderCommand = this.CreateCommand(TakeOrderAsync);
            SubscribeOrderCommand = this.CreateCommand(SubscribeOrderAsync);
            UnsubscribeOrderCommand = this.CreateCommand(UnsubscribeOrderAsync);
            YesCommand = this.CreateCommand(YesAsync);
            NoCommand = this.CreateCommand(NoAsync);
            ExecuteOrderCommand = this.CreateCommand(ExecuteOrderAsync);
            CancelOrderCommand = this.CreateCommand(CancelOrderAsync);
            ArqueOrderCommand = this.CreateCommand(ArgueOrderAsync);
            AcceptOrderCommand = this.CreateCommand(AcceptOrderAsync);

            LoadOrderDetailsCommand = this.CreateCommand(LoadOrderDetailsAsync);
            OpenSettingsCommand = this.CreateCommand(OpenSettingsAsync);

            SystemTimer.SubscribeToEvent(
                (o, e) => _ = SafeExecutionWrapper.WrapAsync(HandleTimerTickAsync),
                (timer, handler) => timer.TimerElapsed += handler,
                (timer, handler) => timer.TimerElapsed -= handler).DisposeWith(Disposables);

            _sections = new BaseOrderDetailsSectionViewModel[]
            {
                CustomerSectionViewModel = Mvx.IoCProvider.IoCConstruct<OrderDetailsCustomerSectionViewModel>(),
                ExecutorSectionViewModel = Mvx.IoCProvider.IoCConstruct<OrderDetailsExecutorSectionViewModel>(),
                VideoSectionViewModel = Mvx.IoCProvider.IoCConstruct<OrderDetailsVideoSectionViewModel>()
            };

            VideoSectionViewModel.RefreshDataFunc = LoadOrderDetailsAsync;
        }

        protected override bool DefaultResult => _hasChanges;

        public int LikesCount => Order?.PositiveArbitrationValuesCount ?? 0;

        public int DisikesCount => Order?.NegativeArbitrationValuesCount ?? 0;

        public string YesText => IsDecideEnabled ? Resources.Yes : LikesCount.ToString();

        public string NoText => IsDecideEnabled ? Resources.No : DisikesCount.ToString();

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

        private Models.Data.Order _order;
        public Models.Data.Order Order
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

        public bool IsHiddenOrder => Order?.OrderCategory == OrderCategory.Private &&
                                     !CustomerSectionViewModel.IsUserCustomer &&
                                     !ExecutorSectionViewModel.IsUserExecutor;

        public string OrderTitle => Order?.Title;

        public string OrderDescription => Order?.Description;

        public override void Prepare(OrderDetailsNavigationParameter parameter)
        {
            _orderId = parameter.OrderId;
            VideoSectionViewModel.SetFullScreenVideos(parameter.FullScreenVideos, parameter.CurrentIndex);
        }

        public override Task InitializeAsync()
        {
            return Task.WhenAll(base.InitializeAsync(), LoadOrderDetailsAsync());
        }

        private Task HandleTimerTickAsync()
        {
            _timerThicksCount++;
            if (_timerThicksCount < 5)
            {
                return Task.CompletedTask;
            }

            _timerThicksCount = 0;
            return UpdateOrderAsync();
        }

        private Task LoadOrderDetailsAsync()
        {
            return UpdateOrderAsync();
        }

        private async Task UpdateOrderAsync()
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
            var user = UserSessionProvider.User;

            if (user?.EmailVerifiedAt == null)
            {
                var canGoProfile = await UserInteraction.ShowConfirmAsync(Resources.ProfileYourEmailNotActual, Resources.Attention, Resources.Ok, Resources.Cancel);
                if (canGoProfile)
                {
                    await NavigationManager.NavigateAsync<ProfileUpdateViewModel, ProfileUpdateResult>();
                }

                return;
            }

            var result = await UserInteraction.ShowConfirmAsync(Resources.TakeOrderQuestion,
                                                              Resources.Attention,
                                                              Resources.TakeAnOrder,
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
                    Order.Executor = UserSessionProvider.User;
                    Order.ActiveTo = takenOrder.ActiveTo;
                    Order.Title = takenOrder.Title;
                    Order.Description = takenOrder.Description;

                    await Task.WhenAll(_sections.Select(item => item.RaiseAllPropertiesChanged()));
                    await RaiseAllPropertiesChanged();
                }

                if (Order is null)
                {
                    return;
                }

                _hasChanges = true;
                Messenger.Publish(new OrderChangedMessage(this, Order));
            }
            catch (NetworkException ex) when (ex.InnerException is ProblemDetailsException problemDetails && problemDetails?.CodeError == Constants.ErrorCodes.LowBalance)
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
            var canRefil = await UserInteraction.ShowConfirmAsync(exception.Message, Resources.Attention, Resources.Replenish, Resources.Cancel);
            if (!canRefil)
            {
                return;
            }

            var navigationParameter = new CashboxTypeNavigationParameter(CashboxType.Refill);
            await NavigationManager.NavigateAsync<CashboxViewModel, CashboxTypeNavigationParameter, bool>(navigationParameter);
        }

        private async Task SubscribeOrderAsync()
        {
            try
            {
                Order = await _ordersManager.SubscribeOrderAsync(_orderId);
                await RaiseAllPropertiesChanged();

                _hasChanges = true;
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

                    _hasChanges = true;
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
                    Order.Title = order.Title;
                    Order.Description = order.Description;

                    await Task.WhenAll(_sections.Select(item => item.RaiseAllPropertiesChanged()));
                    await RaiseAllPropertiesChanged();

                    _hasChanges = true;
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
            var result = await UserInteraction.ShowConfirmAsync(
                Resources.WantToCancelOrder,
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

                _hasChanges = true;
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

                    _hasChanges = true;
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

                    _hasChanges = true;
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
            var result = await UserInteraction.ShowMenuDialogAsync(new[]
            {
                Resources.Complain,
                Resources.BlockUser,
                // TODO: uncomment this when functionality will be available
            });

            if (string.IsNullOrWhiteSpace(result))
            {
                return;
            }

            if (result == Resources.Complain)
            {
                if (!IsUserSessionInitialized)
                {
                    await NavigationManager.NavigateAsync<LoginViewModel>();
                    return;
                }

                var text = await GetComplaintTextAsync();
                if (string.IsNullOrWhiteSpace(text))
                {
                    return;
                }

                await _ordersManager.ComplainOrderAsync(_orderId, text, text);
                UserInteraction.ShowToast(Resources.ThankYouForLettingUsKnow, ToastType.Positive);

                _hasChanges = true;
                Messenger.Publish(new OrderChangedMessage(this, Order));
                return;
            }

            if (result == Resources.BlockUser)
            {
                await BlockUserAsync();
                return;
            }

            if (result == Resources.CopyLink)
            {
                await Clipboard.SetTextAsync(Order?.Video?.ShareUri);
                UserInteraction.ShowToast(Resources.LinkCopied, ToastType.Positive);
                return;
            }
        }

        private async Task BlockUserAsync()
        {
            if (CustomerSectionViewModel?.Order?.Customer?.Id is null)
            {
                return;
            }

            var customerId = CustomerSectionViewModel.Order.Customer.Id;
            var complaintMessage = $"Complaint to user {customerId}";
            var isComplaintSent = await _usersManager.ComplainUserAsync(customerId, complaintMessage, complaintMessage);
            if (!isComplaintSent)
            {
                UserInteraction.ShowToast(Resources.ErrorSomethingWentWrongMessage, ToastType.Negative);
                return;
            }

            var customerUsername = CustomerSectionViewModel.Order.Customer.Login;
            UserInteraction.ShowToast(string.Format(Resources.BlockedUser, customerUsername), ToastType.Positive);

            _hasChanges = true;
            Messenger.Publish(new OrderChangedMessage(this, Order));
            return;
        }

        private async Task<string> GetComplaintTextAsync()
        {
            var result = await UserInteraction.ShowMenuDialogAsync(Constants.ComplaintConstants.CommonCompetitionAims);
            return result;
        }
    }
}
