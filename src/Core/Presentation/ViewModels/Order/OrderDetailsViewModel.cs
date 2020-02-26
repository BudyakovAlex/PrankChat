using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Mediaes;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Exceptions;
using PrankChat.Mobile.Core.Exceptions.UserVisible;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

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

        public string VideoUrl => _order?.Video?.StreamUri;

        public string VideoPlaceholderUrl => "https://ksassets.timeincuk.net/wp/uploads/sites/55/2019/04/GettyImages-1136749971-920x584.jpg";// _order?.Video?.Poster;

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

        public int LikesCount => _order.PositiveArbitrationValuesCount ?? 0;

        public int DisikesCount => _order.NegativeArbitrationValuesCount ?? 0;

        public string YesText => $"{Resources.OrderDetailsView_Yes_Button} {LikesCount}";

        public string NoText => $"{Resources.OrderDetailsView_No_Button} {DisikesCount}";

        public ArbitrationValueType? SelectedArbitration => _order.MyArbitrationValue;

        public bool IsDecideEnabled => SelectedArbitration == null;

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

        #endregion

        public string PriceValue => _order?.Price.ToPriceString();

        public string TimeDaysValue => _order?.FinishIn?.Days.ToString("00");

        public string TimeHourValue => _order?.FinishIn?.Hours.ToString("00");

        public string TimeMinutesValue => _order?.FinishIn?.Minutes.ToString("00");

        public bool IsUserCustomer => _order?.Customer?.Id == _settingsService.User?.Id;

        public bool IsUserExecutor => _order?.Executor?.Id == _settingsService.User?.Id;

        public bool IsUserGuest => !IsUserCustomer && !IsUserExecutor;

        public bool IsSubscribeAvailable => false; // IsUserListener;

        public bool IsUnsubscribeAvailable => false; // IsUserListener;

        public bool IsTakeOrderAvailable => !IsUserCustomer && _order?.Status == OrderStatusType.New;

        public bool IsCancelOrderAvailable => false;

        public bool IsExecuteOrderAvailable => false;

        public bool IsVideoLoadAvailable => _order?.Status == OrderStatusType.InWork && IsUserExecutor;

        public bool IsVideoAvailable => _order?.Video != null;

        public bool IsExecutorAvailable => _order?.Executor != null;

        public bool IsDecideVideoAvailable => _order?.Status == OrderStatusType.InArbitration && IsUserGuest;

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

        public MvxAsyncCommand ShowFullVideoCommand => new MvxAsyncCommand(OnShowFullVideoAsync);

        public MvxAsyncCommand LoadOrderDetailsCommand => new MvxAsyncCommand(LoadOrderDetailsAsync);

        #endregion Commands

        public OrderDetailsViewModel(INavigationService navigationService,
                                     IMvxLog mvxLog,
                                     ISettingsService settingsService,
                                     IMediaService mediaService,
                                     IErrorHandleService errorHandleService,
                                     IApiService apiService,
                                     IDialogService dialogService)
            : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
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
            base.Initialize();
            return LoadOrderDetailsAsync();
        }

        private async Task LoadOrderDetailsAsync()
        {
            try
            {
                IsBusy = true;

                _order = await ApiService.GetOrderDetailsAsync(_orderId);
                await RaiseAllPropertiesChanged();

                IsNoSelected = SelectedArbitration == ArbitrationValueType.Negative;
                IsYesSelected = SelectedArbitration == ArbitrationValueType.Positive;
            }
            catch (Exception ex)
            {
                ErrorHandleService.HandleException(ex);
                ErrorHandleService.LogError(this, "Error on loading order page.");
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
                _order.Status = order.Status;
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
                ErrorHandleService.HandleException(ex);
                ErrorHandleService.LogError(this, "Order subscription failed.");
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
                ErrorHandleService.HandleException(ex);
                ErrorHandleService.LogError(this, "Error on order unsubscription.");
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
                    _order.Status = order.Status;
                    await RaiseAllPropertiesChanged();
                }
            }
            catch (Exception ex)
            {
                ErrorHandleService.HandleException(ex);
                ErrorHandleService.LogError(this, "Error on argue initialization.", ex);
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
                ErrorHandleService.HandleException(ex);
                ErrorHandleService.LogError(this, "Error on accept order.", ex);
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
            if (!IsDecideEnabled)
                return;

            try
            {
                IsYesSelected = !IsYesSelected;
                var order = await ApiService.VoteVideoAsync(_orderId, ArbitrationValueType.Positive);
                if (order != null)
                {
                    _order.MyArbitrationValue = order.MyArbitrationValue;
                    _order.PositiveArbitrationValuesCount = order.PositiveArbitrationValuesCount;
                    _order.NegativeArbitrationValuesCount = order.NegativeArbitrationValuesCount;
                }
            }
            catch (Exception ex)
            {
                ErrorHandleService.HandleException(ex);
                ErrorHandleService.LogError(this, "Order confirmation error.", ex);

                IsYesSelected = !IsYesSelected;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnNoAsync()
        {
            if (!IsDecideEnabled)
                return;

            try
            {
                IsNoSelected = !IsNoSelected;
                var order = await ApiService.VoteVideoAsync(_orderId, ArbitrationValueType.Negative);
                if (order != null)
                {
                    _order.MyArbitrationValue = order.MyArbitrationValue;
                    _order.PositiveArbitrationValuesCount = order.PositiveArbitrationValuesCount;
                    _order.NegativeArbitrationValuesCount = order.NegativeArbitrationValuesCount;
                }
            }
            catch (Exception ex)
            {
                ErrorHandleService.HandleException(ex);
                ErrorHandleService.LogError(this, "Error on order voting.");

                IsYesSelected = !IsYesSelected;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private Task OnShowFullVideoAsync()
        {
            return NavigationService.ShowFullScreenVideoView(VideoUrl, VideoName, VideoDetails);
        }
    }
}
