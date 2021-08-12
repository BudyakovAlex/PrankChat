using MvvmCross.Commands;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Managers.Media;
using PrankChat.Mobile.Core.Managers.Video;
using PrankChat.Mobile.Core.Messages;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels.Arbitration.Items;
using PrankChat.Mobile.Core.Presentation.ViewModels.Common.Abstract;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order.Sections.Abstract;
using PrankChat.Mobile.Core.Presentation.ViewModels.Parameters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Video;
using PrankChat.Mobile.Core.Providers.UserSession;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order.Sections
{
    public class OrderDetailsVideoSectionViewModel : BaseOrderDetailsSectionViewModel
    {
        private readonly IVideoManager _videoManager;
        private readonly IUserSessionProvider _userSessionProvider;
        private readonly IMediaManager _mediaService;

        private CancellationTokenSource _cancellationTokenSource;
        private BaseVideoItemViewModel[] _fullScreenVideos;

        private int _currentIndex;

        public OrderDetailsVideoSectionViewModel(
            IVideoManager videoManager,
            IUserSessionProvider userSessionProvider,
            IMediaManager mediaService)
        {
            _userSessionProvider = userSessionProvider;
            _videoManager = videoManager;
            _mediaService = mediaService;

            CancelUploadingCommand = this.CreateCommand(() => _cancellationTokenSource?.Cancel());
            ShowFullVideoCommand = this.CreateCommand(ShowFullVideoAsync);
            LoadVideoCommand = this.CreateCommand(LoadVideoAsync);
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

        public void SetFullScreenVideos(BaseVideoItemViewModel[] fullScreenVideos, int currentIndex)
        {
            _fullScreenVideos = fullScreenVideos ?? Array.Empty<BaseVideoItemViewModel>();
            _currentIndex = currentIndex;
        }

        public void RefreshFullScreenVideo()
        {
            if (Order.Video?.StreamUri.IsNullOrEmpty() ?? true)
            {
                return;
            }

            var fullScreenVideo = _fullScreenVideos.FirstOrDefault(item => item.VideoId == Order?.Video?.Id);
            if (fullScreenVideo is null)
            {
                return;
            }

            var videoIndex = _fullScreenVideos.IndexOfOrDefault(fullScreenVideo);
            _fullScreenVideos[videoIndex] = new OrderedVideoItemViewModel(_videoManager, _userSessionProvider, Order.Video);
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
                var videoVideModel = new OrderedVideoItemViewModel(_videoManager, _userSessionProvider, Order.Video);
                _fullScreenVideos.Append(videoVideModel);
                _currentIndex = _fullScreenVideos.Length - 1;
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

            var navigationParams = _fullScreenVideos.Length > 0
                ? new FullScreenVideoParameter(_fullScreenVideos, _currentIndex)
                : new FullScreenVideoParameter(new OrderedVideoItemViewModel(_videoManager, _userSessionProvider, Order.Video));

            if (navigationParams.Videos.Length == 0)
            {
                return;
            }

            var shouldReload = await NavigationManager.NavigateAsync<FullScreenVideoViewModel, FullScreenVideoParameter, bool>(navigationParams);
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