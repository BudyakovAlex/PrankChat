using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order.Sections
{
    public class OrderDetailsVideoSectionViewModel : BaseItemViewModel
    {
        private readonly OrderDataModel _order;

        public OrderDetailsVideoSectionViewModel(OrderDataModel orderDataModel)
        {
            _order = orderDataModel;
        }

        public bool IsVideoProcessing => _order?.Status == OrderStatusType.VideoInProcess;

        public string VideoUrl => _order?.Video?.StreamUri;

        public string VideoPlaceholderUrl => _order?.Video?.Poster;

        public string VideoName => _order?.Title;

        public string VideoDetails => _order?.Description;

        public bool IsVideoLoadAvailable => _order?.Status == OrderStatusType.InWork && IsUserExecutor;

        public bool IsVideoAvailable => _order?.Video != null && !IsVideoProcessing;

        public bool IsDecideVideoAvailable => _order?.Status == OrderStatusType.InArbitration;

        public bool IsDecisionVideoAvailable => (_order?.Status == OrderStatusType.WaitFinish || _order?.Status == OrderStatusType.VideoWaitModeration) && IsUserCustomer;

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