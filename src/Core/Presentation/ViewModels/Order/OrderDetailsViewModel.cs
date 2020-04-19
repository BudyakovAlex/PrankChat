﻿using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Mediaes;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Platforms;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.Navigation.Results;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order
{
    public class OrderDetailsViewModel : BaseViewModel, IMvxViewModel<OrderDetailsNavigationParameter, OrderDetailsResult>
    {
        private readonly ISettingsService _settingsService;
        private readonly IMediaService _mediaService;
        private readonly IPlatformService _platformService;

        private int _orderId;
        private OrderDataModel _order;

        #region Profile

        public string ProfilePhotoUrl => _order?.Customer?.Avatar;

        public string ProfileName => _order?.Customer?.Name;

        public string ProfileShortName => ProfileName?.ToShortenName();

        #endregion Profile

        #region Video

        public bool IsVideoProcessing => _order?.Status == OrderStatusType.VideoInProcess;

        public string VideoUrl => _order?.Video?.StreamUri;

        public string VideoPlaceholderUrl => _order?.Video?.Poster;

        public string VideoName => _order?.Title;

        public string VideoDetails => _order?.Description;

        #endregion Video

        #region Executor

        public string ExecutorPhotoUrl => _order?.Executor?.Avatar;

        public string ExecutorName => _order?.Executor?.Name;

        public string ExecutorShortName => _order?.Executor?.Name?.ToShortenName();

        public string StartOrderDate => _order?.TakenToWorkAt?.ToShortDateString();

        #endregion Executor

        #region Decide

        public int LikesCount => _order?.PositiveArbitrationValuesCount ?? 0;

        public int DisikesCount => _order?.NegativeArbitrationValuesCount ?? 0;

        public string YesText => SelectedArbitration == null ? Resources.OrderDetailsView_Yes_Button : LikesCount.ToString();

        public string NoText => SelectedArbitration == null ? Resources.OrderDetailsView_No_Button : DisikesCount.ToString();

        public ArbitrationValueType? SelectedArbitration => _order?.MyArbitrationValue;

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

        private TimeSpan? TimeValue
        {
            get
            {
                switch (_order?.Status)
                {
                    case null:
                        return null;
                    case OrderStatusType.VideoInProcess:
                    case OrderStatusType.VideoWaitModeration:
                    case OrderStatusType.VideoProcessError:
                        return _order?.VideoUploadedIn < TimeSpan.Zero ? TimeSpan.Zero : _order.VideoUploadedIn;
                    default:
                        return _order?.FinishIn < TimeSpan.Zero ? TimeSpan.Zero : _order.FinishIn;
                }
            }
        }

        public string PriceValue => _order?.Price.ToPriceString();

        public string TimeDaysValue => TimeValue?.Days.ToString("00");

        public string TimeHourValue => TimeValue?.Hours.ToString("00");

        public string TimeMinutesValue => TimeValue?.Minutes.ToString("00");

        public bool IsUserCustomer => _order?.Customer?.Id == _settingsService.User?.Id;

        public bool IsUserExecutor => _order?.Executor?.Id == _settingsService.User?.Id;

        public bool IsUserGuest => !IsUserCustomer && !IsUserExecutor;

        public bool IsSubscribeAvailable => false; // IsUserListener;

        public bool IsUnsubscribeAvailable => false; // IsUserListener;

        public bool IsTakeOrderAvailable => !IsUserCustomer && _order?.Status == OrderStatusType.Active;

        public bool IsAnyOrderActionAvailable => IsTakeOrderAvailable || IsSubscribeAvailable || IsUnsubscribeAvailable;

        public bool IsCancelOrderAvailable => IsUserCustomer && _order?.Status == OrderStatusType.Active;

        public bool IsExecuteOrderAvailable => false;

        public bool IsVideoLoadAvailable => _order?.Status == OrderStatusType.InWork && IsUserExecutor;

        public bool IsVideoAvailable => _order?.Video != null && !IsVideoProcessing;

        public bool IsExecutorAvailable => _order?.Executor != null && _order?.Executor?.Id != _settingsService.User?.Id;

        public bool IsDecideVideoAvailable => _order?.Status == OrderStatusType.InArbitration && IsUserGuest;

        public bool IsDecisionVideoAvailable => (_order?.Status == OrderStatusType.WaitFinish || _order?.Status == OrderStatusType.VideoWaitModeration) && IsUserCustomer;

        public bool IsTimeAvailable => _order?.Status != null && TimeValue != null && TimeValue >= TimeSpan.Zero &&
                                       (_order.VideoUploadedAt != null &&
                                        (_order.Status.Value == OrderStatusType.WaitFinish ||
                                         _order.Status.Value == OrderStatusType.VideoInProcess ||
                                         _order.Status.Value == OrderStatusType.VideoWaitModeration) ||
                                        _order.FinishIn != null);

        public TaskCompletionSource<object> CloseCompletionSource { get; set; } = new TaskCompletionSource<object>();

        #region Commands

        public MvxAsyncCommand TakeOrderCommand => new MvxAsyncCommand(OnTakeOrderAsync);

        public MvxAsyncCommand SubscribeOrderCommand => new MvxAsyncCommand(OnSubscribeOrderAsync);

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

        public MvxAsyncCommand OpenSettingsCommand => new MvxAsyncCommand(OpenSettingsAsync);

        #endregion Commands

        public OrderDetailsViewModel(INavigationService navigationService,
                                     ISettingsService settingsService,
                                     IMediaService mediaService,
                                     IErrorHandleService errorHandleService,
                                     IApiService apiService,
                                     IDialogService dialogService,
                                     IPlatformService platformService)
            : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
            _settingsService = settingsService;
            _mediaService = mediaService;
            _platformService = platformService;
        }

        public void Prepare(OrderDetailsNavigationParameter parameter)
        {
            _orderId = parameter.OrderId;
        }

        public override Task Initialize()
        {
            base.Initialize();
            return LoadOrderDetailsCommand.ExecuteAsync();
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            if (viewFinishing && CloseCompletionSource != null && !CloseCompletionSource.Task.IsCompleted && !CloseCompletionSource.Task.IsFaulted)
                CloseCompletionSource?.SetResult(new OrderDetailsResult(_order));

            base.ViewDestroy(viewFinishing);
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
                _order.ActiveTo = order.ActiveTo;
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
                    await LoadOrderDetailsAsync();
                    DialogService.ShowToast(Resources.OrderDetailsView_Video_Uploaded, ToastType.Positive);
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

                var order = await ApiService.AcceptOrderAsync(_orderId);
                if (order != null)
                {
                    _order.Status = order.Status;
                    await RaiseAllPropertiesChanged();
                }
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

            IsBusy = true;

            var order = await ApiService.CancelOrderAsync(_orderId);
            if (order != null)
            {
                _order.Status = order.Status;
                await RaiseAllPropertiesChanged();
            }

            IsBusy = false;
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
                    await RaiseAllPropertiesChanged();
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
                    await RaiseAllPropertiesChanged();
                }
            }
            catch (Exception ex)
            {
                ErrorHandleService.HandleException(ex);
                ErrorHandleService.LogError(this, "Error on order voting.");

                IsNoSelected = !IsNoSelected;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private Task OnShowFullVideoAsync()
        {
            if (_order?.Video is null)
            {
                return Task.CompletedTask;
            }

            var navigationParams = new FullScreenVideoParameter(_order.Video.Id,
                                                                VideoUrl,
                                                                VideoName,
                                                                VideoDetails,
                                                                _order.Video.ShareUri,
                                                                ProfilePhotoUrl,
                                                                _order.Video.LikesCount,
                                                                _order.Video.IsLiked);
            return NavigationService.ShowFullScreenVideoView(navigationParams);
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

                await ApiService.ComplainOrderAsync(_orderId, text, text);
                DialogService.ShowToast(Resources.Complaint_Complete_Message, ToastType.Positive);
                return;
            }

            if (result == Resources.Publication_Item_Copy_Link)
            {
                await _platformService.CopyTextAsync(_order.Video.ShareUri);
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
