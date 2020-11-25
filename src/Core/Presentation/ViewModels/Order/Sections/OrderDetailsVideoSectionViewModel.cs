using PrankChat.Mobile.Core.ApplicationServices.Mediaes;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order.Sections
{
    public class OrderDetailsVideoSectionViewModel : BaseViewModel
    {
        private readonly ISettingsService _settingsService;
        private readonly IMediaService _mediaService;

        private readonly OrderDataModel _order;

        private CancellationTokenSource _cancellationTokenSource;
        private int _currentIndex;
        private List<FullScreenVideoDataModel> _fullScreenVideos;

        public OrderDetailsVideoSectionViewModel(ISettingsService settingsService, IMediaService mediaService, OrderDataModel orderDataModel)
        {
            _settingsService = settingsService;
            _mediaService = mediaService;
            _order = orderDataModel;
        }

        public bool IsVideoProcessing => _order?.Status == OrderStatusType.VideoInProcess;

        public string VideoUrl => _order?.Video?.StreamUri;

        public string VideoPlaceholderUrl => _order?.Video?.Poster;

        public string VideoName => _order?.Title;

        public string VideoDetails => _order?.Description;

        public bool IsVideoLoadAvailable => _order?.Status == OrderStatusType.InWork &&
                                            _order?.Executor?.Id == _settingsService.User?.Id;

        public bool IsVideoAvailable => _order?.Video != null && !IsVideoProcessing;

        public bool IsDecideVideoAvailable => _order?.Status == OrderStatusType.InArbitration;

        public bool IsDecisionVideoAvailable => (_order?.Status == OrderStatusType.WaitFinish ||
                                                 _order?.Status == OrderStatusType.VideoWaitModeration) &&
                                                 _order.Customer?.Id == _settingsService.User?.Id;

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

        private async Task LoadVideoAsync()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }

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

                if (_order is null)
                {
                    return;
                }

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
                                                                       _order.Video.IsDisliked,
                                                                       _order.Video.Poster));
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
                                                                            _order.Video.IsDisliked,
                                                                            _order.Video.Poster)); ;

            var shouldReload = await NavigationService.ShowFullScreenVideoView(navigationParams);
            if (!shouldReload)
            {
                return;
            }

            if (_order is null)
            {
                return;
            }

            Messenger.Publish(new OrderChangedMessage(this, _order));
        }

    }
}