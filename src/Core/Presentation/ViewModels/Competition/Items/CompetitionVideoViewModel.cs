using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Managers.Publications;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Abstract;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.Core.Presentation.ViewModels.Video;
using PrankChat.Mobile.Core.Providers.UserSession;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items
{
    public class CompetitionVideoViewModel : BaseViewModel, IVideoItemViewModel
    {
        private readonly IPublicationsManager _publicationsManager;
        private readonly IUserSessionProvider _userSessionProvider;

        private readonly Models.Data.Video _video;
        private readonly long _numberOfDislikes;
        private readonly bool _isDisliked;
        private readonly Func<List<FullScreenVideo>> _getAllFullScreenVideoDataFunc;

        private CancellationTokenSource _cancellationSendingLikeTokenSource;
        private MvxSubscriptionToken _updateNumberOfViewsSubscriptionToken;

        public CompetitionVideoViewModel(
            IPublicationsManager publicationsManager,
            IVideoPlayerService videoPlayerService,
            IUserSessionProvider userSessionProvider,
            Models.Data.Video video,
            bool isMyPublication,
            bool isVotingAvailable,
            Func<List<FullScreenVideo>> getAllFullScreenVideoDataFunc)
        {
            _userSessionProvider = userSessionProvider;
            _video = video;

            _publicationsManager = publicationsManager;
            VideoPlayerService = videoPlayerService;
 
            NumberOfLikes = _video.LikesCount;
            _numberOfDislikes = _video.DislikesCount;

            NumberOfViews = _video.ViewsCount;
            IsLiked = video.IsLiked;
            _isDisliked = video.IsDisliked;

            IsMyPublication = isMyPublication;
            IsVotingAvailable = isVotingAvailable;
            _getAllFullScreenVideoDataFunc = getAllFullScreenVideoDataFunc;

            LikeCommand = new MvxCommand(Like);
            OpenUserProfileCommand = new MvxRestrictedAsyncCommand(
                OpenUserProfileAsync,
                restrictedCanExecute: () => _userSessionProvider.User != null,
                handleFunc: NavigationManager.NavigateAsync<LoginViewModel>);

            Subscribe();
        }

        public ICommand LikeCommand { get; }

        public IMvxAsyncCommand ShowFullScreenVideoCommand => new MvxAsyncCommand(ShowFullScreenVideoAsync);

        public IMvxAsyncCommand OpenUserProfileCommand { get; }

        public IVideoPlayerService VideoPlayerService { get; }

        public int VideoId => _video?.Id ?? -1;

        public string VideoUrl => _video?.StreamUri;

        public string PreviewUrl => _video?.PreviewUri;

        public string ShareLink => _video?.ShareUri;

        public string UserName => _video?.User?.Login;

        public string ProfileShortName => UserName?.ToShortenName();

        public string VideoName => _video?.Title;

        public string Description => _video?.Description;

        public string AvatarUrl => _video?.User?.Avatar;

        public string StubImageUrl => _video?.Poster;

        public DateTime PublicationDate => _video?.CreatedAt.UtcDateTime ?? DateTime.MinValue;

        public bool IsMyPublication { get; }

        public bool IsVotingAvailable { get; }

        public bool IsVideoProcessing => string.IsNullOrEmpty(VideoUrl);

        public long NumberOfComments => _video?.CommentsCount ?? 0;

        public bool CanPlayVideo => true;

        public string LikesCount => CountExtensions.ToCountString(NumberOfLikes);

        public string ViewsCount => CountExtensions.ToCountViewsString(NumberOfViews);

        public string PublicationDateString => PublicationDate.ToTimeAgoPublicationString();

        public bool CanVoteVideo => IsVotingAvailable && !IsMyPublication;

        private long _numberOfLikes;
        public long NumberOfLikes
        {
            get => _numberOfLikes;
            set
            {
                if (SetProperty(ref _numberOfLikes, value))
                {
                    RaisePropertyChanged(nameof(LikesCount));
                }
            }
        }

        private long _numberOfViews;
        public long NumberOfViews
        {
            get => _numberOfViews;
            set
            {
                if (SetProperty(ref _numberOfViews, value))
                {
                    RaisePropertyChanged(nameof(ViewsCount));
                }
            }
        }

        private bool _isLiked;
        public bool IsLiked
        {
            get => _isLiked;
            set => SetProperty(ref _isLiked, value);
        }

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
                _video.User?.Login?.ToShortenName(),
                NumberOfLikes,
                _numberOfDislikes,
                NumberOfComments,
                IsLiked,
                _isDisliked,
                StubImageUrl,
                CanVoteVideo);
        }

        private Task OpenUserProfileAsync()
        {
            if (_video.User?.Id is null ||
                _video.User.Id == _userSessionProvider.User.Id)
            {
                return Task.CompletedTask;
            }

            if (!Connectivity.NetworkAccess.HasConnection())
            {
                return Task.FromResult(false);
            }

            return NavigationManager.NavigateAsync<UserProfileViewModel, int, bool>(_video.User.Id);
        }

        private void Like()
        {
            if (IsLiked)
            {
                return;
            }

            IsLiked = !IsLiked;

            var totalLikes = IsLiked ? NumberOfLikes + 1 : NumberOfLikes - 1;
            NumberOfLikes = totalLikes > 0 ? totalLikes : 0;

            SendLikeAsync().FireAndForget();
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
                await _publicationsManager.SendLikeAsync(VideoId, IsLiked, _cancellationSendingLikeTokenSource.Token);
            }
            finally
            {
                _cancellationSendingLikeTokenSource?.Dispose();
                _cancellationSendingLikeTokenSource = null;
            }
        }

        private async Task ShowFullScreenVideoAsync()
        {
            VideoPlayerService.Player.TryRegisterViewedFact(VideoId, Constants.Delays.ViewedFactRegistrationDelayInMilliseconds);

            var items = _getAllFullScreenVideoDataFunc?.Invoke() ?? new List<FullScreenVideo> { GetFullScreenVideo() };
            var currentItem = items.FirstOrDefault(item => item.VideoId == VideoId);
            var index = currentItem is null ? 0 : items.IndexOf(currentItem);
            var navigationParams = new FullScreenVideoParameter(items, index);
            if (navigationParams.Videos.Count == 0)
            {
                return;
            }

            var shouldRefresh = await NavigationManager.NavigateAsync<FullScreenVideoViewModel, FullScreenVideoParameter, bool>(navigationParams);
            if (!shouldRefresh)
            {
                return;
            }

            Messenger.Publish(new ReloadCompetitionMessage(this));
        }

        private void Subscribe()
        {
            _updateNumberOfViewsSubscriptionToken = Messenger.Subscribe<ViewCountMessage>(viewCountMessage =>
            {
                if (viewCountMessage.VideoId == VideoId)
                {
                    NumberOfViews = viewCountMessage.ViewsCount;
                }
            });
        }

        private void Unsubscribe()
        {
            if (_updateNumberOfViewsSubscriptionToken is null)
            {
                return;
            }

            _updateNumberOfViewsSubscriptionToken.Dispose();
            _updateNumberOfViewsSubscriptionToken = null;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                Unsubscribe();
            }
        }
    }
}