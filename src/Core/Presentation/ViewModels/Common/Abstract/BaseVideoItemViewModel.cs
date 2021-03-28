using Microsoft.AppCenter.Crashes;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Data.Enums;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Ioc;
using PrankChat.Mobile.Core.Managers.Video;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.ViewModels.Abstract;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.Core.Providers.UserSession;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Common.Abstract
{
    public abstract class BaseVideoItemViewModel : BaseViewModel
    {
        private CancellationTokenSource _cancellationSendingLikeTokenSource;
        private CancellationTokenSource _cancellationSendingDislikeTokenSource;

        public BaseVideoItemViewModel(
            IVideoManager videoManager,
            IUserSessionProvider userSessionProvider,
            Models.Data.Video video)
        {
            VideoManager = videoManager;
            UserSessionProvider = userSessionProvider;
            Video = video;

            IsLiked = video.IsLiked;
            IsDisliked = video.IsDisliked;
            NumberOfLikes = video.LikesCount;
            NumberOfDislikes = video.DislikesCount;
            NumberOfComments = Video.CommentsCount;
            IsSubscribedToUser = User?.IsSubscribed ?? false;

            PreviewVideoPlayer = CompositionRoot.Container.Resolve<IVideoPlayer>();
            PreviewVideoPlayer.SubscribeToEvent<IVideoPlayer, VideoPlayingStatus>(
                OnVideoPlayingStatusChanged,
                (wrapper, handler) => wrapper.VideoPlayingStatusChanged += handler,
                (wrapper, handler) => wrapper.VideoPlayingStatusChanged -= handler).DisposeWith(Disposables);

            FullVideoPlayer = CompositionRoot.Container.Resolve<IVideoPlayer>();

            PreviewVideoPlayer.CanRepeat = true;
            PreviewVideoPlayer.SetVideoUrl(Video.PreviewUri);
            FullVideoPlayer.SetVideoUrl(Video.StreamUri);

            FullVideoPlayer.SubscribeToEvent<IVideoPlayer, VideoPlayingStatus>(
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

            OpenUserProfileCommand = this.CreateRestrictedCommand(
                OpenUserProfileAsync,
                restrictedCanExecute: () => IsUserSessionInitialized,
                handleFunc: NavigationManager.NavigateAsync<LoginViewModel>);
        }

        public abstract bool CanPlayVideo { get; }

        public abstract bool CanVoteVideo { get; }

        public IMvxCommand LikeCommand { get; }

        public IMvxCommand DislikeCommand { get; }

        public IMvxAsyncCommand OpenUserProfileCommand { get; }

        public string ProfileShortName => User?.Login?.ToShortenName();

        public string AvatarUrl => User?.Avatar;

        public int UserId => User?.Id ?? 0;

        private bool _isSubscribedToUser;
        public bool IsSubscribedToUser
        {
            get => _isSubscribedToUser;
            set => SetProperty(ref _isSubscribedToUser, value);
        }

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

        public int VideoId => Video.Id;

        public IVideoPlayer PreviewVideoPlayer { get; }

        public IVideoPlayer FullVideoPlayer { get; }

        public string VideoUrl => Video.StreamUri;

        public string PreviewUrl => Video.PreviewUri;

        public string StubImageUrl => Video.Poster;

        public string VideoName => Video.Title;

        public string Description => Video.Description;

        public string ShareLink => Video.ShareUri;

        private long _numberOfComments;
        public long NumberOfComments
        {
            get => _numberOfComments;
            set => SetProperty(ref _numberOfComments, value);
        }

        private long? _numberOfLikes;
        public long? NumberOfLikes
        {
            get => _numberOfLikes;
            set => SetProperty(ref _numberOfLikes, value);
        }

        private long? _numberOfDislikes;
        public long? NumberOfDislikes
        {
            get => _numberOfDislikes;
            set => SetProperty(ref _numberOfDislikes, value);
        }

        protected abstract User User { get; }

        protected IVideoManager VideoManager { get; }

        protected IUserSessionProvider UserSessionProvider { get; }

        protected bool IsUserSessionInitialized => UserSessionProvider.User != null;

        protected Models.Data.Video Video { get; }

        protected virtual void OnLikeChanged()
        {
        }

        protected virtual void OnDislikeChanged()
        {
        }

        protected virtual void OnDislike()
        {
            if (!CanVoteVideo)
            {
                return;
            }

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
            if (!CanVoteVideo)
            {
                return;
            }

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

            _ = SafeExecutionWrapper.WrapAsync(() => VideoManager.IncrementVideoViewsAsync(VideoId), (ex) =>
            {
                Crashes.TrackError(ex);
                return Task.CompletedTask;
            });
        }

        protected virtual Task OpenUserProfileAsync()
        {
            if (User?.Id is null ||
                User.Id == UserSessionProvider.User.Id)
            {
                return Task.CompletedTask;
            }

            if (!Connectivity.NetworkAccess.HasConnection())
            {
                return Task.FromResult(false);
            }

            return NavigationManager.NavigateAsync<UserProfileViewModel, int, bool>(User.Id);
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
                var video = await VideoManager.SendLikeAsync(VideoId, IsLiked, _cancellationSendingLikeTokenSource.Token);
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
                var video = await VideoManager.SendDislikeAsync(VideoId, IsDisliked, _cancellationSendingDislikeTokenSource.Token);
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