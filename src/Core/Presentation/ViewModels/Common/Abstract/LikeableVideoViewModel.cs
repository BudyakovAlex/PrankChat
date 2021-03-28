using Microsoft.AppCenter.Crashes;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Data.Enums;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Ioc;
using PrankChat.Mobile.Core.Managers.Publications;
using PrankChat.Mobile.Core.Managers.Video;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.ViewModels.Abstract;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Common.Abstract
{
    public abstract class VideoItemViewModel : BasePageViewModel, IVideoItemViewModel
    {
        private readonly IPublicationsManager _publicationsManager;
        private readonly IVideoManager _videoManager;

        private readonly Models.Data.Video _video;

        private CancellationTokenSource _cancellationSendingLikeTokenSource;
        private CancellationTokenSource _cancellationSendingDislikeTokenSource;

        public VideoItemViewModel(
            IPublicationsManager publicationsManager,
            IVideoManager videoManager,
            Models.Data.Video video)
        {
            _publicationsManager = publicationsManager;
            _videoManager = videoManager;
            _video = video;
            VideoPlayer = CompositionRoot.Container.Resolve<IVideoPlayer>();
            VideoPlayer.SubscribeToEvent<IVideoPlayer, VideoPlayingStatus>(
                OnVideoPlayingStatusChanged,
                (wrapper, handler) => wrapper.VideoPlayingStatusChanged += handler,
                (wrapper, handler) => wrapper.VideoPlayingStatusChanged -= handler).DisposeWith(Disposables);

            LikeCommand = this.CreateRestrictedCommand(
                OnLike,
                restrictedCanExecute: () => IsUserSessionInitialized,
                handleFunc: NavigationManager.NavigateAsync<LoginViewModel>);

            DislikeCommand = this.CreateRestrictedCommand(
                OnDislike,
                restrictedCanExecute: () => IsUserSessionInitialized,
                handleFunc: NavigationManager.NavigateAsync<LoginViewModel>);
        }

        protected long? NumberOfLikes { get; set; }

        protected long? NumberOfDislikes { get; set; }

        public IMvxCommand LikeCommand { get; }

        public IMvxCommand DislikeCommand { get; }

        public abstract string AvatarUrl { get; }

        public abstract string LoginShortName { get; }

        public abstract bool CanPlayVideo { get; }

        public abstract bool CanVoteVideo { get; }

        private bool _isLiked;
        public bool IsLiked
        {
            get => _isLiked;
            set => SetProperty(ref _isLiked, value);
        }

        private bool _isDisliked;
        public bool IsDisliked
        {
            get => _isDisliked;
            set => SetProperty(ref _isDisliked, value);
        }

        public int VideoId => _video.Id;

        public IVideoPlayer VideoPlayer { get; }

        public string VideoUrl => _video.StreamUri;

        public string PreviewUrl => _video.PreviewUri;

        public string StubImageUrl => _video.Poster;

        public string VideoName => _video.Title;

        public string Description => _video.Description;

        public string ShareLink => _video.ShareUri;

        public long NumberOfComments => _video.CommentsCount;

        public FullScreenVideo GetFullScreenVideo()
        {
            return new FullScreenVideo(
                _video.User?.Id ?? 0,
                _video.User?.IsSubscribed ?? false,
                VideoId,
                VideoUrl,
                VideoName,
                Description,
                ShareLink,
                AvatarUrl,
                LoginShortName,
                NumberOfLikes,
                NumberOfDislikes,
                NumberOfComments,
                IsLiked,
                IsDisliked,
                StubImageUrl,
                CanVoteVideo);
        }

        protected virtual void OnLikeChanged()
        {
        }

        protected virtual void OnDislikeChanged()
        {
        }

        protected virtual void OnDislike()
        {
            IsDisliked = !IsDisliked;

            var totalDislikes = IsDisliked ? NumberOfDislikes + 1 : NumberOfDislikes - 1;
            NumberOfDislikes = totalDislikes > 0 ? totalDislikes : 0;

            OnDislikeChanged();

            _ = SafeExecutionWrapper.WrapAsync(SendDislikeAsync);

            if (IsDisliked && IsLiked)
            {
                IsLiked = false;
                var totalLikes = NumberOfLikes - 1;
                NumberOfLikes = totalLikes > 0 ? totalLikes : 0;

                OnLikeChanged();
            }
        }

        protected virtual void OnLike()
        {
            IsLiked = !IsLiked;

            var totalLikes = IsLiked ? NumberOfLikes + 1 : NumberOfLikes - 1;
            NumberOfLikes = totalLikes > 0 ? totalLikes : 0;

            OnLikeChanged();

            _ = SafeExecutionWrapper.WrapAsync(SendLikeAsync);

            if (IsLiked && IsDisliked)
            {
                IsDisliked = false;
                var totalDislikes = NumberOfDislikes - 1;
                NumberOfDislikes = totalDislikes > 0 ? totalDislikes : 0;

                OnDislikeChanged();
            }
        }

        protected virtual void OnVideoPlayingStatusChanged(object sender, VideoPlayingStatus status)
        {
            if (status != VideoPlayingStatus.PartiallyPlayed)
            {
                return;
            }

            _ = SafeExecutionWrapper.WrapAsync(() => _videoManager.IncrementVideoViewsAsync(VideoId), (ex) => 
            {
                Crashes.TrackError(ex);
                return Task.CompletedTask;
            });
        }

        private async Task SendLikeAsync()
        {
            _cancellationSendingLikeTokenSource?.Cancel();
            if (_cancellationSendingLikeTokenSource == null)
            {
                _cancellationSendingLikeTokenSource = new CancellationTokenSource();
            }

            try
            {
                var video = await _publicationsManager.SendLikeAsync(VideoId, IsLiked, _cancellationSendingLikeTokenSource.Token);
                if (video is null)
                {
                    return;
                }

                NumberOfLikes = video.LikesCount;
                NumberOfDislikes = video.DislikesCount;
                IsLiked = video.IsLiked;
                IsDisliked = video.IsDisliked;
                OnDislikeChanged();
                OnLikeChanged();
            }
            finally
            {
                _cancellationSendingLikeTokenSource?.Dispose();
                _cancellationSendingLikeTokenSource = null;
            }
        }

        private async Task SendDislikeAsync()
        {
            _cancellationSendingDislikeTokenSource?.Cancel();
            if (_cancellationSendingDislikeTokenSource == null)
            {
                _cancellationSendingDislikeTokenSource = new CancellationTokenSource();
            }

            try
            {
                var video = await _publicationsManager.SendDislikeAsync(VideoId, IsDisliked, _cancellationSendingDislikeTokenSource.Token);
                if (video is null)
                {
                    return;
                }

                NumberOfLikes = video.LikesCount;
                NumberOfDislikes = video.DislikesCount;
                IsLiked = video.IsLiked;
                IsDisliked = video.IsDisliked;
                OnDislikeChanged();
                OnLikeChanged();
            }
            finally
            {
                _cancellationSendingDislikeTokenSource?.Dispose();
                _cancellationSendingDislikeTokenSource = null;
            }
        }
    }
}