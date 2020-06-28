using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.BusinessServices;
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
    public class CompetitionVideoViewModel : BaseItemViewModel, IVideoItemViewModel
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

        public ICommand LikeCommand { get; }

        public IMvxAsyncCommand ShowFullScreenVideoCommand => new MvxAsyncCommand(ShowFullScreenVideoAsync);

        public IMvxAsyncCommand OpenUserProfileCommand { get; }

        public IVideoPlayerService VideoPlayerService { get; }

        public int VideoId { get; }
        public string VideoUrl { get; }
        public string PreviewUrl { get; }
        public string ShareLink { get; }
        public string UserName { get; }
        public string ProfileShortName { get; }
        public string VideoName { get; }
        public string Description { get; }
        public string AvatarUrl { get; }
        public string StubImageUrl { get; }
        public DateTime PublicationDate { get; }
        public bool IsMyPublication { get; }
        public bool IsVotingAvailable { get; }
        public bool IsVideoProcessing => string.IsNullOrEmpty(VideoUrl);

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

        public long NumberOfComments { get; }

        private long _numberOfViews;
        public long NumberOfViews
        {
            get => _numberOfViews;
            set
            {
                if (SetProperty(ref _numberOfViews, value))
                {
                    RaisePropertyChanged(nameof(LikesCount));
                }
            }
        }

        public bool CanPlayVideo => true;

        public string LikesCount => CountExtensions.ToCountString(NumberOfLikes);
        public string ViewsCount => CountExtensions.ToCountViewsString(NumberOfViews);

        public string PublicationDateString => PublicationDate.ToTimeAgoPublicationString();

        public bool CanVoteVideo => IsVotingAvailable && !IsMyPublication;

        private bool _isLiked;
        public bool IsLiked
        {
            get => _isLiked;
            set => SetProperty(ref _isLiked, value);
        }

        public CompetitionVideoViewModel(IApiService apiService,
                                         IVideoPlayerService videoPlayerService,
                                         INavigationService navigationService,
                                         ISettingsService settingsService,
                                         IMvxMessenger mvxMessenger,
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

            VideoPlayerService = videoPlayerService;
            StubImageUrl = _videoDataModel.Poster;
            VideoId = _videoDataModel.Id;
            VideoUrl = _videoDataModel.StreamUri;
            PreviewUrl = _videoDataModel.PreviewUri;
            ShareLink = _videoDataModel.ShareUri;
            VideoName = _videoDataModel.Title;
            Description = _videoDataModel.Description;
            UserName = _videoDataModel.User?.Login;
            ProfileShortName = UserName.ToShortenName();
            AvatarUrl = _videoDataModel.User?.Avatar;
            NumberOfLikes = _videoDataModel.LikesCount;
            _numberOfDislikes = _videoDataModel.DislikesCount;
            NumberOfComments = _videoDataModel.CommentsCount;
            NumberOfViews = _videoDataModel.ViewsCount;
            PublicationDate = videoDataModel.CreatedAt.UtcDateTime;
            IsLiked = videoDataModel.IsLiked;
            _isDisliked = videoDataModel.IsDisliked;

            IsMyPublication = isMyPublication;
            IsVotingAvailable = isVotingAvailable;
            _getAllFullScreenVideoDataFunc = getAllFullScreenVideoDataFunc;

            LikeCommand = new MvxCommand(OnLike);
            OpenUserProfileCommand = new MvxRestrictedAsyncCommand(OpenUserProfileAsync, restrictedCanExecute: () => _settingsService.User != null, handleFunc: _navigationService.ShowLoginView);
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

        private void OnLike()
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
    }
}
