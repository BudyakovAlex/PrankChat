using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages;
using PrankChat.Mobile.Core.ApplicationServices.Mediaes;
using PrankChat.Mobile.Core.ApplicationServices.Platforms;
using PrankChat.Mobile.Core.ApplicationServices.Timer;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Exceptions;
using PrankChat.Mobile.Core.Exceptions.Network;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.Navigation.Results;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order
{
    //TODO: split logic to more specific view models
    public class OrderDetailsViewModel : BaseViewModel, IMvxViewModel<OrderDetailsNavigationParameter, OrderDetailsResult>
    {
        private readonly IMediaService _mediaService;
        private readonly IPlatformService _platformService;

        private int _orderId;
        private int _timerThicksCount;

        private List<FullScreenVideoDataModel> _fullScreenVideos;
        private int _currentIndex;
        private OrderDataModel _order;
        private MvxSubscriptionToken _timerTickMessageToken;

        private CancellationTokenSource _cancellationTokenSource;

        public OrderDetailsViewModel(IPlatformService platformService, IMediaService mediaService)
        {
            _mediaService = mediaService;
            _platformService = platformService;

            OpenCustomerProfileCommand = new MvxRestrictedAsyncCommand(OpenCustomerProfileAsync, restrictedCanExecute: () => SettingsService.User != null, handleFunc: NavigationService.ShowLoginView);
            OpenExecutorProfileCommand = new MvxRestrictedAsyncCommand(OpenExecutorProfileAsync, restrictedCanExecute: () => SettingsService.User != null, handleFunc: NavigationService.ShowLoginView);
            TakeOrderCommand = new MvxAsyncCommand(TakeOrderAsync);
            SubscribeOrderCommand = new MvxAsyncCommand(SubscribeOrderAsync);
            UnsubscribeOrderCommand = new MvxAsyncCommand(UnsubscribeOrderAsync);
            YesCommand = new MvxAsyncCommand(YesAsync);
            NoCommand = new MvxAsyncCommand(NoAsync);
            LoadVideoCommand = new MvxAsyncCommand(LoadVideoAsync);
            ExecuteOrderCommand = new MvxAsyncCommand(ExecuteOrderAsync);
            CancelOrderCommand = new MvxAsyncCommand(CancelOrderAsync);
            ArqueOrderCommand = new MvxAsyncCommand(ArgueOrderAsync);
            AcceptOrderCommand = new MvxAsyncCommand(AcceptOrderAsync);
            ShowFullVideoCommand = new MvxAsyncCommand(ShowFullVideoAsync);
            LoadOrderDetailsCommand = new MvxAsyncCommand(LoadOrderDetailsAsync);
            OpenSettingsCommand = new MvxAsyncCommand(OpenSettingsAsync);
            CancelUploadingCommand = new MvxCommand(() => _cancellationTokenSource?.Cancel());
        }

        #region Profile

        public string ProfilePhotoUrl => _order?.Customer?.Avatar;

        public string ProfileName => _order?.Customer?.Login;

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

        public string ExecutorName => _order?.Executor?.Login;

        public string ExecutorShortName => _order?.Executor?.Login?.ToShortenName();

        public string StartOrderDate => _order?.TakenToWorkAt?.ToShortDateString();

        #endregion Executor

        #region Decide

        public int LikesCount => _order?.PositiveArbitrationValuesCount ?? 0;

        public int DisikesCount => _order?.NegativeArbitrationValuesCount ?? 0;

        public string YesText => IsDecideEnabled ? Resources.OrderDetailsView_Yes_Button : LikesCount.ToString();

        public string NoText => IsDecideEnabled ? Resources.OrderDetailsView_No_Button : DisikesCount.ToString();

        public ArbitrationValueType? SelectedArbitration => _order?.MyArbitrationValue;

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

        #endregion

        private bool _isUploading;
        public bool IsUploading
        {
            get => _isUploading;
            private set => SetProperty(ref _isUploading, value);
        }

        private float _uploadingProgress;
        public float UploadingProgress
        {
            get => _uploadingProgress;
            private set => SetProperty(ref _uploadingProgress, value);
        }

        private string _uploadingProgressStringPresentation;
        public string UploadingProgressStringPresentation
        {
            get => _uploadingProgressStringPresentation;
            private set => SetProperty(ref _uploadingProgressStringPresentation, value);
        }

        private TimeSpan? TimeValue => _order?.GetActiveOrderTime();

        public string PriceValue => _order?.Price.ToPriceString();

        public string TimeDaysValue => TimeValue?.Days.ToString("00");

        public string TimeHourValue => TimeValue?.Hours.ToString("00");

        public string TimeMinutesValue => TimeValue?.Minutes.ToString("00");

        public bool IsUserCustomer => _order?.Customer?.Id == SettingsService.User?.Id;

        public bool IsUserExecutor => _order?.Executor?.Id == SettingsService.User?.Id;

        public bool IsUserGuest => !IsUserCustomer && !IsUserExecutor;

        public bool IsSubscribeAvailable => false; // IsUserListener;

        public bool IsUnsubscribeAvailable => false; // IsUserListener;

        public bool IsTakeOrderAvailable => !IsUserCustomer && _order?.Status == OrderStatusType.Active;

        public bool IsAnyOrderActionAvailable => IsTakeOrderAvailable || IsSubscribeAvailable || IsUnsubscribeAvailable;

        public bool IsCancelOrderAvailable => IsUserCustomer && _order?.Status == OrderStatusType.Active;

        public bool IsExecuteOrderAvailable => false;

        public bool IsVideoLoadAvailable => _order?.Status == OrderStatusType.InWork && IsUserExecutor;

        public bool IsVideoAvailable => _order?.Video != null && !IsVideoProcessing;

        public bool IsExecutorAvailable => _order?.Executor != null && _order?.Executor?.Id != SettingsService.User?.Id;

        public bool IsDecideVideoAvailable => _order?.Status == OrderStatusType.InArbitration;

        public bool IsDecisionVideoAvailable => (_order?.Status == OrderStatusType.WaitFinish || _order?.Status == OrderStatusType.VideoWaitModeration) && IsUserCustomer;

        public bool IsTimeAvailable => _order?.Status != null && TimeValue != null && TimeValue >= TimeSpan.Zero &&
                                       (_order.VideoUploadedAt != null &&
                                        (_order.Status.Value == OrderStatusType.WaitFinish ||
                                         _order.Status.Value == OrderStatusType.VideoInProcess ||
                                         _order.Status.Value == OrderStatusType.VideoWaitModeration) ||
                                        _order.FinishIn != null);

        public TaskCompletionSource<object> CloseCompletionSource { get; set; } = new TaskCompletionSource<object>();

        #region Commands

        public IMvxAsyncCommand TakeOrderCommand { get; }
        public IMvxAsyncCommand SubscribeOrderCommand { get; }
        public IMvxAsyncCommand UnsubscribeOrderCommand { get; }
        public IMvxAsyncCommand YesCommand { get; }
        public IMvxAsyncCommand NoCommand { get; }
        public IMvxAsyncCommand LoadVideoCommand { get; }
        public IMvxAsyncCommand ExecuteOrderCommand { get; }
        public IMvxAsyncCommand CancelOrderCommand { get; }
        public IMvxAsyncCommand ArqueOrderCommand { get; }
        public IMvxAsyncCommand AcceptOrderCommand { get; }
        public IMvxAsyncCommand ShowFullVideoCommand { get; }
        public IMvxAsyncCommand LoadOrderDetailsCommand { get; }
        public IMvxAsyncCommand OpenSettingsCommand { get; }
        public IMvxCommand CancelUploadingCommand { get; }
        public IMvxAsyncCommand OpenCustomerProfileCommand { get; }
        public IMvxAsyncCommand OpenExecutorProfileCommand { get; }

        #endregion Commands

        public void Prepare(OrderDetailsNavigationParameter parameter)
        {
            _orderId = parameter.OrderId;
            _fullScreenVideos = parameter.FullScreenVideos;
            _currentIndex = parameter.CurrentIndex;
        }

        public override Task Initialize()
        {
            return LoadOrderDetailsCommand.ExecuteAsync();
        }

        public override void ViewCreated()
        {
            _timerTickMessageToken = Messenger.Subscribe<TimerTickMessage>(OnTimerTick, MvxReference.Strong);

            base.ViewCreated();
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            _timerTickMessageToken?.Dispose();

            if (viewFinishing &&
                CloseCompletionSource != null &&
                !CloseCompletionSource.Task.IsCompleted &&
                !CloseCompletionSource.Task.IsFaulted)
            {
                CloseCompletionSource?.SetResult(new OrderDetailsResult(_order));
            }

            base.ViewDestroy(viewFinishing);
        }

        private async void OnTimerTick(TimerTickMessage msg)
        {
            _timerThicksCount++;
            if (_timerThicksCount >= 20)
            {
                _timerThicksCount = 0;
                try
                {
                    _order = await ApiService.GetOrderDetailsAsync(_orderId);
                    RefreshFullScreenVideo();
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

        private void RefreshFullScreenVideo()
        {
            var fullScreenVideo = _fullScreenVideos.FirstOrDefault(item => item.VideoId == _order?.Video?.Id);
            if (fullScreenVideo is null)
            {
                return;
            }

            fullScreenVideo.VideoUrl = _order?.Video?.StreamUri;
        }

        private Task OpenExecutorProfileAsync()
        {
            if (_order?.Executor?.Id is null ||
                _order.Executor.Id == SettingsService.User.Id)
            {
                return Task.CompletedTask;
            }

            return NavigationService.ShowUserProfile(_order.Executor.Id);
        }

        private Task OpenCustomerProfileAsync()
        {
            if (_order?.Customer?.Id is null ||
                _order.Customer.Id == SettingsService.User.Id)
            {
                return Task.CompletedTask;
            }

            return NavigationService.ShowUserProfile(_order.Customer.Id);
        }

        private async Task LoadOrderDetailsAsync()
        {
            try
            {
                IsBusy = true;

                _order = await ApiService.GetOrderDetailsAsync(_orderId);
                RefreshFullScreenVideo();
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
                var order = await ApiService.TakeOrderAsync(_orderId);
                if (order != null)
                {
                    _order.Status = order.Status;
                    _order.Executor = SettingsService.User;
                    _order.ActiveTo = order.ActiveTo;
                    await RaiseAllPropertiesChanged();
                }

                Messenger.Publish(new OrderChangedMessage(this, _order));
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
                IsBusy = true;

                var order = await ApiService.SubscribeOrderAsync(_orderId);
                await RaiseAllPropertiesChanged();
                Messenger.Publish(new OrderChangedMessage(this, _order));
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

        private async Task UnsubscribeOrderAsync()
        {
            try
            {
                IsBusy = true;

                var order = await ApiService.UnsubscribeOrderAsync(_orderId);
                await RaiseAllPropertiesChanged();
                Messenger.Publish(new OrderChangedMessage(this, _order));
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

        private async Task LoadVideoAsync()
        {
            try
            {
                IsBusy = true;

                var file = await _mediaService.PickVideoAsync();
                if (file == null)
                {
                    return;
                }

                IsBusy = false;

                UploadingProgress = 0;
                UploadingProgressStringPresentation = "- / -";

                IsUploading = true;
                _cancellationTokenSource = new CancellationTokenSource();

                var video = await ApiService.SendVideoAsync(_orderId,
                                                            file.Path,
                                                            _order?.Title,
                                                            _order?.Description,
                                                            OnUploadingProgressChanged,
                                                            _cancellationTokenSource.Token);
                if (video == null && (!_cancellationTokenSource?.IsCancellationRequested ?? true))
                {
                    DialogService.ShowToast(Resources.Video_Failed_To_Upload, ToastType.Negative);
                    _cancellationTokenSource = null;
                    IsUploading = false;
                    return;
                }

                _cancellationTokenSource = null;
                IsUploading = false;
                IsBusy = true;

                await LoadOrderDetailsAsync();
                DialogService.ShowToast(Resources.OrderDetailsView_Video_Uploaded, ToastType.Positive);
                await RaiseAllPropertiesChanged();
                _order.Video = video;
                _order.VideoUploadedAt = _order.VideoUploadedAt ?? DateTime.Now;

                if (!_fullScreenVideos.Any(item => item.VideoId == video.Id))
                {
                    _fullScreenVideos.Add(new FullScreenVideoDataModel(_order.Customer.Id,
                                                                        _order.Customer.IsSubscribed,
                                                                        _order.Video.Id,
                                                                        _order.Video.StreamUri,
                                                                        _order.Title,
                                                                        _order.Description,
                                                                        _order.Video.ShareUri,
                                                                        _order.Customer.Avatar,
                                                                        _order.Customer.Login.ToShortenName(),
                                                                        _order.Video.LikesCount,
                                                                        _order.Video.DislikesCount,
                                                                        _order.Video.CommentsCount,
                                                                        _order.Video.IsLiked,
                                                                        _order.Video.IsDisliked));
                    _currentIndex = _fullScreenVideos.Count - 1;
                }

                Messenger.Publish(new OrderChangedMessage(this, _order));
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void OnUploadingProgressChanged(double progress, double size)
        {
            UploadingProgress = (float)(progress / size * 100);
            UploadingProgressStringPresentation = $"{((long)progress).ToFileSizePresentation()} / {((long)size).ToFileSizePresentation()}";

            if (UploadingProgress < 100)
            {
                return;
            }

            IsUploading = false;
            IsBusy = true;
        }

        private async Task ArgueOrderAsync()
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

                Messenger.Publish(new OrderChangedMessage(this, order));
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

        private async Task AcceptOrderAsync()
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

                Messenger.Publish(new OrderChangedMessage(this, order));
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

        private async Task CancelOrderAsync()
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

            Messenger.Publish(new OrderChangedMessage(this, _order));
            IsBusy = false;
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
                var order = await ApiService.VoteVideoAsync(_orderId, ArbitrationValueType.Positive);
                if (order != null)
                {
                    _order.MyArbitrationValue = order.MyArbitrationValue;
                    _order.PositiveArbitrationValuesCount = order.PositiveArbitrationValuesCount;
                    _order.NegativeArbitrationValuesCount = order.NegativeArbitrationValuesCount;
                    await RaiseAllPropertiesChanged();
                }

                Messenger.Publish(new OrderChangedMessage(this, order));
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

        private async Task NoAsync()
        {
            if (!IsDecideEnabled || !IsUserGuest)
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

                Messenger.Publish(new OrderChangedMessage(this, order));
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

        private async Task ShowFullVideoAsync()
        {
            if (_order?.Video is null)
            {
                return;
            }

            var navigationParams = _fullScreenVideos.Count > 0
                ? new FullScreenVideoParameter(_fullScreenVideos, _currentIndex)
                : new FullScreenVideoParameter(new FullScreenVideoDataModel(_order?.Customer?.Id ?? 0,
                                                                            _order?.Customer?.IsSubscribed ?? false,
                                                                            _order.Video.Id,
                                                                            VideoUrl,
                                                                            VideoName,
                                                                            VideoDetails,
                                                                            _order.Video.ShareUri,
                                                                            ProfilePhotoUrl,
                                                                            ProfileName?.ToShortenName(),
                                                                            _order.Video.LikesCount,
                                                                            _order.Video.DislikesCount,
                                                                            _order.Video.CommentsCount,
                                                                            _order.Video.IsLiked,
                                                                            _order.Video.IsDisliked)); ;

            var shouldReload = await NavigationService.ShowFullScreenVideoView(navigationParams);
            if (!shouldReload)
            {
                return;
            }

            Messenger.Publish(new OrderChangedMessage(this, _order));
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
