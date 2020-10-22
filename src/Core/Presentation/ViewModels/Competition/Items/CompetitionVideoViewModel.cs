using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.BusinessServices.Logger;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items
{
    public class CompetitionVideoViewModel : BaseItemViewModel, IVideoItemViewModel, IDisposable
    {
        private readonly IApiService _apiService;
        private readonly INavigationService _navigationService;
        private readonly ISettingsService _settingsService;
        private readonly IMvxMessenger _mvxMessenger;

        private readonly VideoDataModel _videoDataModel;
        private readonly long _numberOfDislikes;
        private readonly bool _isDisliked;
        private readonly Func<List<FullScreenVideoDataModel>> _getAllFullScreenVideoDataFunc;

        private CancellationTokenSource _cancellationSendingLikeTokenSource;
        private MvxSubscriptionToken _updateNumberOfViewsSubscriptionToken;

        public CompetitionVideoViewModel(IApiService apiService,
                                         IVideoPlayerService videoPlayerService,
                                         INavigationService navigationService,
                                         ISettingsService settingsService,
                                         IMvxMessenger mvxMessenger,
                                         ILogger logger,
                                         VideoDataModel videoDataModel,
                                         bool isMyPublication,
                                         bool isVotingAvailable,
                                         Func<List<FullScreenVideoDataModel>> getAllFullScreenVideoDataFunc)
        {
            _apiService = apiService;
            _navigationService = navigationService;
            _settingsService = settingsService;
            _mvxMessenger = mvxMessenger;
            _videoDataModel = videoDataModel;

            Logger = logger;
            VideoPlayerService = videoPlayerService;
 
            NumberOfLikes = _videoDataModel.LikesCount;
            _numberOfDislikes = _videoDataModel.DislikesCount;

            NumberOfViews = _videoDataModel.ViewsCount;
            IsLiked = videoDataModel.IsLiked;
            _isDisliked = videoDataModel.IsDisliked;

            IsMyPublication = isMyPublication;
            IsVotingAvailable = isVotingAvailable;
            _getAllFullScreenVideoDataFunc = getAllFullScreenVideoDataFunc;

            LikeCommand = new MvxCommand(Like);
            OpenUserProfileCommand = new MvxRestrictedAsyncCommand(OpenUserProfileAsync, restrictedCanExecute: () => _settingsService.User != null, handleFunc: _navigationService.ShowLoginView);

            Subscribe();
        }

        public ICommand LikeCommand { get; }

        public IMvxAsyncCommand ShowFullScreenVideoCommand => new MvxAsyncCommand(ShowFullScreenVideoAsync);

        public IMvxAsyncCommand OpenUserProfileCommand { get; }

        public IVideoPlayerService VideoPlayerService { get; }

        public ILogger Logger { get; }

        public int VideoId => _videoDataModel?.Id ?? -1;

        public string VideoUrl => _videoDataModel?.StreamUri;

        public string PreviewUrl => _videoDataModel?.PreviewUri;

        public string ShareLink => _videoDataModel?.ShareUri;

        public string UserName => _videoDataModel?.User?.Login;

        public string ProfileShortName => UserName?.ToShortenName();

        public string VideoName => _videoDataModel?.Title;

        public string Description => _videoDataModel?.Description;

        public string AvatarUrl => _videoDataModel?.User?.Avatar;

        public string StubImageUrl => _videoDataModel?.Poster;

        public DateTime PublicationDate => _videoDataModel?.CreatedAt.UtcDateTime ?? DateTime.MinValue;

        public bool IsMyPublication { get; }

        public bool IsVotingAvailable { get; }

        public bool IsVideoProcessing => string.IsNullOrEmpty(VideoUrl);

        public long NumberOfComments => _videoDataModel?.CommentsCount ?? 0;

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

        public FullScreenVideoDataModel GetFullScreenVideoDataModel()
        {
            return new FullScreenVideoDataModel(_videoDataModel.User?.Id ?? 0,
                                                _videoDataModel.User?.IsSubscribed ?? false,
                                                VideoId,
                                                VideoUrl,
                                                VideoName,
                                                Description,
                                                ShareLink,
                                                AvatarUrl,
                                                _videoDataModel.User?.Login?.ToShortenName(),
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
            if (_videoDataModel.User?.Id is null ||
                _videoDataModel.User.Id == _settingsService.User.Id)
            {
                return Task.CompletedTask;
            }

            return _navigationService.ShowUserProfile(_videoDataModel.User.Id);
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
                await _apiService.SendLikeAsync(VideoId, IsLiked, _cancellationSendingLikeTokenSource.Token);
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

            var items = _getAllFullScreenVideoDataFunc?.Invoke() ?? new List<FullScreenVideoDataModel> { GetFullScreenVideoDataModel() };
            var currentItem = items.FirstOrDefault(item => item.VideoId == VideoId);
            var index = currentItem is null ? 0 : items.IndexOf(currentItem);
            var navigationParams = new FullScreenVideoParameter(items, index);

            var shouldRefresh = await _navigationService.ShowFullScreenVideoView(navigationParams);
            if (!shouldRefresh)
            {
                return;
            }

            _mvxMessenger.Publish(new ReloadCompetitionMessage(this));
        }

        private void Subscribe()
        {
            _updateNumberOfViewsSubscriptionToken = _mvxMessenger.Subscribe<ViewCountMessage>(viewCountMessage =>
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Unsubscribe();
            }
        }
    }
}
