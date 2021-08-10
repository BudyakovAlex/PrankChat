using Microsoft.AppCenter.Crashes;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Data.Enums;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Ioc;
using PrankChat.Mobile.Core.Managers.Video;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.ViewModels.Abstract;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.Core.Providers.UserSession;
using PrankChat.Mobile.Core.Wrappers;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Common.Abstract
{
    public abstract class BaseVideoItemViewModel : BaseViewModel
    {
        private CancellationTokenSource _cancellationSendingLikeTokenSource;
        private CancellationTokenSource _cancellationSendingDislikeTokenSource;

        private readonly SafeExecutionWrapper _silentSafeExecutionWrapper;

        public BaseVideoItemViewModel(
            IVideoManager videoManager,
            IUserSessionProvider userSessionProvider,
            Models.Data.Video video)
        {
            VideoManager = videoManager;
            UserSessionProvider = userSessionProvider;
            Video = video;

            _silentSafeExecutionWrapper = new SafeExecutionWrapper((ex) =>
            {
                Crashes.TrackError(ex);
                return Task.CompletedTask;
            });

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
            PreviewVideoPlayer.IsMuted = true;
            PreviewVideoPlayer.SetVideoUrl(Video.PreviewUri);

            FullVideoPlayer.SubscribeToEvent<IVideoPlayer, VideoPlayingStatus>(
                OnVideoPlayingStatusChanged,
                (wrapper, handler) => wrapper.VideoPlayingStatusChanged += handler,
                (wrapper, handler) => wrapper.VideoPlayingStatusChanged -= handler).DisposeWith(Disposables);

            FullVideoPlayer.SetVideoUrl(Video.StreamUri);

            LikeCommand = this.CreateRestrictedCommand(
                Like,
                restrictedCanExecute: () => IsUserSessionInitialized,
                handleFunc: NavigateByRestrictionAsync,
                useIsBusyWrapper: false);

            DislikeCommand = this.CreateRestrictedCommand(
                Dislike,
                restrictedCanExecute: () => IsUserSessionInitialized,
                handleFunc: NavigateByRestrictionAsync,
                useIsBusyWrapper: false);

            OpenUserProfileCommand = this.CreateRestrictedCommand(
                OpenUserProfileAsync,
                restrictedCanExecute: () => IsUserSessionInitialized,
                handleFunc: NavigateByRestrictionAsync);

            IncrementVideoCountCommand = this.CreateCommand(IncrementVideoCountAsync);
        }

        public event EventHandler ViewsCountChanged;

        public event EventHandler LikesChanged;

        public abstract bool CanPlayVideo { get; }

        public abstract bool CanVoteVideo { get; }

        public IMvxCommand LikeCommand { get; }

        public IMvxCommand DislikeCommand { get; }

        public IMvxAsyncCommand OpenUserProfileCommand { get; }

        public IMvxCommand IncrementVideoCountCommand { get; }

        public string ProfileShortName => User?.Login?.ToShortenName();

        public string AvatarUrl => User?.Avatar;

        public int UserId => User?.Id ?? 0;

        public bool IsVideoProcessing => VideoUrl.IsNullOrEmpty();

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

        protected virtual void Dislike()
        {
            if (!CanVoteVideo)
            {
                return;
            }

            IsDisliked = !IsDisliked;

            var totalDislikes = IsDisliked ? NumberOfDislikes + 1 : NumberOfDislikes - 1;
            NumberOfDislikes = totalDislikes > 0 ? totalDislikes : 0;

            OnDislikeChanged();
            LikesChanged?.Invoke(this, EventArgs.Empty);

            _ = SafeExecutionWrapper.WrapAsync(SendDislikeAsync);

            if (IsDisliked && IsLiked)
            {
                IsLiked = false;
                var totalLikes = NumberOfLikes - 1;
                NumberOfLikes = totalLikes > 0 ? totalLikes : 0;

                OnLikeChanged();
                LikesChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        protected virtual void Like()
        {
            if (!CanVoteVideo)
            {
                return;
            }

            IsLiked = !IsLiked;

            var totalLikes = IsLiked ? NumberOfLikes + 1 : NumberOfLikes - 1;
            NumberOfLikes = totalLikes > 0 ? totalLikes : 0;

            OnLikeChanged();
            LikesChanged?.Invoke(this, EventArgs.Empty);

            _ = SafeExecutionWrapper.WrapAsync(SendLikeAsync);

            if (IsLiked && IsDisliked)
            {
                IsDisliked = false;
                var totalDislikes = NumberOfDislikes - 1;
                NumberOfDislikes = totalDislikes > 0 ? totalDislikes : 0;

                OnDislikeChanged();
                LikesChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        protected virtual void OnVideoPlayingStatusChanged(object sender, VideoPlayingStatus status)
        {
            if (status != VideoPlayingStatus.PartiallyPlayed)
            {
                return;
            }

            IncrementVideoCountCommand.Execute();
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

        protected virtual Task NavigateByRestrictionAsync()
        {
            FullVideoPlayer?.Stop();
            PreviewVideoPlayer?.Stop();
            return NavigationManager.NavigateAsync<LoginViewModel>();
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
            catch (TaskCanceledException)
            {
                Debug.WriteLine("Task for dislike cancelled");
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
            catch (TaskCanceledException)
            {
                Debug.WriteLine("Task for dislike cancelled");
            }
            finally
            {
                _cancellationSendingDislikeTokenSource?.Dispose();
                _cancellationSendingDislikeTokenSource = null;
            }
        }

        private Task IncrementVideoCountAsync()
        {
            return _silentSafeExecutionWrapper.WrapAsync(async () =>
            {
                var newCount = await VideoManager.IncrementVideoViewsAsync(VideoId);
                if (newCount > Video.ViewsCount)
                {
                    ViewsCountChanged?.Invoke(this, EventArgs.Empty);
                }
            });
        }
    }
}