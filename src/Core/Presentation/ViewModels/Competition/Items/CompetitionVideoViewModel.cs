using MvvmCross.Commands;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Data.Enums;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Ioc;
using PrankChat.Mobile.Core.Managers.Publications;
using PrankChat.Mobile.Core.Managers.Video;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Abstract;
using PrankChat.Mobile.Core.Presentation.ViewModels.Common.Abstract;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.Core.Presentation.ViewModels.Video;
using PrankChat.Mobile.Core.Providers.UserSession;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items
{
    public class CompetitionVideoViewModel : VideoItemViewModel
    {
        private readonly IUserSessionProvider _userSessionProvider;

        private readonly Models.Data.Video _video;

        private readonly Func<List<FullScreenVideo>> _getAllFullScreenVideoDataFunc;

        public CompetitionVideoViewModel(
            IPublicationsManager publicationsManager,
            IVideoManager videoManager,
            IUserSessionProvider userSessionProvider,
            Models.Data.Video video,
            bool isMyPublication,
            bool isVotingAvailable,
            Func<List<FullScreenVideo>> getAllFullScreenVideoDataFunc) : base(publicationsManager, videoManager, video)
        {
            _userSessionProvider = userSessionProvider;
            _video = video;

            NumberOfViews = _video.ViewsCount;

            IsMyPublication = isMyPublication;
            IsVotingAvailable = isVotingAvailable;
            _getAllFullScreenVideoDataFunc = getAllFullScreenVideoDataFunc;

            OpenUserProfileCommand = this.CreateRestrictedCommand(
                OpenUserProfileAsync,
                restrictedCanExecute: () => _userSessionProvider.User != null,
                handleFunc: NavigationManager.NavigateAsync<LoginViewModel>);

           Messenger.SubscribeWithCondition<ViewCountMessage>(msg => msg.VideoId == VideoId, msg => NumberOfViews = msg.ViewsCount)
                .DisposeWith(Disposables);
        }

        public IMvxAsyncCommand ShowFullScreenVideoCommand => this.CreateCommand(ShowFullScreenVideoAsync);

        public IMvxAsyncCommand OpenUserProfileCommand { get; }



        public string ShareLink => _video?.ShareUri;

        public string UserName => _video?.User?.Login;

        public string ProfileShortName => UserName?.ToShortenName();

        public string VideoName => _video?.Title;

        public string Description => _video?.Description;

        public string AvatarUrl => _video?.User?.Avatar;

        public DateTime PublicationDate => _video?.CreatedAt.UtcDateTime ?? DateTime.MinValue;

        public bool IsMyPublication { get; }

        public bool IsVotingAvailable { get; }

        public bool IsVideoProcessing => string.IsNullOrEmpty(VideoUrl);

        public long NumberOfComments => _video?.CommentsCount ?? 0;

        public override bool CanPlayVideo => true;

        public string LikesCount => CountExtensions.ToCountString(NumberOfLikes);

        public string ViewsCount => CountExtensions.ToCountViewsString(NumberOfViews);

        public string PublicationDateString => PublicationDate.ToTimeAgoPublicationString();

        public bool CanVoteVideo => IsVotingAvailable && !IsMyPublication;

        private long _numberOfViews;
        public long NumberOfViews
        {
            get => _numberOfViews;
            set => SetProperty(ref _numberOfViews, value, () => RaisePropertyChanged(nameof(ViewsCount)));
        }

        protected override void OnLikeChanged()
        {
            base.OnLikeChanged();
            RaisePropertyChanged(LikesCount);
        }

        protected override void OnDislikeChanged()
        {
            base.OnDislikeChanged();
            RaisePropertyChanged(LikesCount);
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

        private async Task ShowFullScreenVideoAsync()
        {
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
    }
}