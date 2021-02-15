using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Mediaes;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Managers.Video;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order.Sections.Abstract;
using PrankChat.Mobile.Core.Providers.UserSession;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order.Sections
{
    public class OrderDetailsVideoSectionViewModel : BaseOrderDetailsSectionViewModel
    {
        private readonly IVideoManager _videoManager;
        private readonly IUserSessionProvider _userSessionProvider;
        private readonly IMediaService _mediaService;

        private CancellationTokenSource _cancellationTokenSource;
        private List<FullScreenVideo> _fullScreenVideos;

        private int _currentIndex;

        public OrderDetailsVideoSectionViewModel(IVideoManager videoManager,
                                                 IUserSessionProvider userSessionProvider,
                                                 IMediaService mediaService)
        {
            _userSessionProvider = userSessionProvider;
            _videoManager = videoManager;
            _mediaService = mediaService;

            CancelUploadingCommand = new MvxCommand(() => _cancellationTokenSource?.Cancel());
            ShowFullVideoCommand = new MvxAsyncCommand(ShowFullVideoAsync);
            LoadVideoCommand = new MvxAsyncCommand(() => ExecutionStateWrapper.WrapAsync(LoadVideoAsync));
        }

        public IMvxCommand CancelUploadingCommand { get; }
        public IMvxAsyncCommand ShowFullVideoCommand { get; }
        public IMvxAsyncCommand LoadVideoCommand { get; }

        public Func<Task> RefreshDataFunc { get; set; }

        public bool IsVideoProcessing => Order?.Status == OrderStatusType.VideoInProcess;

        public string VideoUrl => Order?.Video?.StreamUri;

        public string VideoPlaceholderUrl => Order?.Video?.Poster;

        public string VideoName => Order?.Title;

        public string VideoDetails => Order?.Description;

        public bool IsVideoLoadAvailable => Order?.Status == OrderStatusType.InWork &&
                                            Order?.Executor?.Id == _userSessionProvider.User?.Id;

        public bool IsVideoAvailable => Order?.Video != null && !IsVideoProcessing;

        public bool IsDecideVideoAvailable => Order?.Status == OrderStatusType.InArbitration;

        public bool IsDecisionVideoAvailable => (Order?.Status == OrderStatusType.WaitFinish ||
                                                 Order?.Status == OrderStatusType.VideoWaitModeration) &&
                                                 Order.Customer?.Id == _userSessionProvider.User?.Id;

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

        public void SetFullScreenVideos(List<FullScreenVideo> fullScreenVideos, int currentIndex)
        {
            _fullScreenVideos = fullScreenVideos ?? new List<FullScreenVideo>();
            _currentIndex = currentIndex;
        }

        public void RefreshFullScreenVideo()
        {
            var fullScreenVideo = _fullScreenVideos.FirstOrDefault(item => item.VideoId == Order?.Video?.Id);
            if (fullScreenVideo is null)
            {
                return;
            }

            fullScreenVideo.VideoUrl = Order?.Video?.StreamUri;
        }

        private async Task LoadVideoAsync()
        {
            var file = await _mediaService.PickVideoAsync();
            if (file == null)
            {
                return;
            }

            UploadingProgress = 0;
            UploadingProgressStringPresentation = "- / -";

            IsUploading = true;
            _cancellationTokenSource = new CancellationTokenSource();

            var video = await _videoManager.SendVideoAsync(Order.Id,
                                                           file.Path,
                                                           Order?.Title,
                                                           Order?.Description,
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

            var refreshTask = RefreshDataFunc?.Invoke() ?? Task.CompletedTask;
            await refreshTask;

            DialogService.ShowToast(Resources.OrderDetailsView_Video_Uploaded, ToastType.Positive);
            await RaiseAllPropertiesChanged();

            if (Order is null)
            {
                return;
            }

            Order.Video = video;
            Order.VideoUploadedAt = Order.VideoUploadedAt ?? DateTime.Now;

            if (!_fullScreenVideos.Any(item => item.VideoId == video.Id))
            {
                _fullScreenVideos.Add(Order.ToFullScreenVideo());
                _currentIndex = _fullScreenVideos.Count - 1;
            }

            Messenger.Publish(new OrderChangedMessage(this, Order));
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
        }

        private async Task ShowFullVideoAsync()
        {
            if (Order?.Video is null)
            {
                return;
            }

            var navigationParams = _fullScreenVideos.Count > 0
                ? new FullScreenVideoParameter(_fullScreenVideos, _currentIndex)
                : new FullScreenVideoParameter(Order.ToFullScreenVideo());

            var shouldReload = await NavigationService.ShowFullScreenVideoView(navigationParams);
            if (!shouldReload)
            {
                return;
            }

            if (Order is null)
            {
                return;
            }

            Messenger.Publish(new OrderChangedMessage(this, Order));
        }
    }
}