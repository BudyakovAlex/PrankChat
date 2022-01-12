using MvvmCross.Commands;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Managers.Video;
using PrankChat.Mobile.Core.Messages;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.ViewModels.Common.Abstract;
using PrankChat.Mobile.Core.ViewModels.Parameters;
using PrankChat.Mobile.Core.ViewModels.Video;
using PrankChat.Mobile.Core.Providers.UserSession;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using PrankChat.Mobile.Core.ViewModels.Results;

namespace PrankChat.Mobile.Core.ViewModels.Competitions.Items
{
    public class CompetitionVideoViewModel : BaseVideoItemViewModel
    {
        private readonly Func<BaseVideoItemViewModel[]> _getAllFullScreenVideosFunc;

        public CompetitionVideoViewModel(
            IVideoManager videoManager,
            IUserSessionProvider userSessionProvider,
            Models.Data.Video video,
            bool isMyPublication,
            bool isVotingAvailable,
            Func<BaseVideoItemViewModel[]> getAllFullScreenVideosFunc) : base(videoManager, userSessionProvider, video)
        {
            NumberOfViews = Video.ViewsCount;

            IsMyPublication = isMyPublication;
            IsVotingAvailable = isVotingAvailable;
            _getAllFullScreenVideosFunc = getAllFullScreenVideosFunc;

            Messenger.SubscribeWithCondition<ViewCountMessage>(msg => msg.VideoId == VideoId, msg => NumberOfViews = msg.ViewsCount)
                 .DisposeWith(Disposables);

            ShowFullScreenVideoCommand = this.CreateCommand(ShowFullScreenVideoAsync);

            LikesCount = CountExtensions.ToCountString(NumberOfLikes);
        }

        public override bool CanVoteVideo => IsVotingAvailable && !IsMyPublication;

        public override bool CanPlayVideo => true;

        public string UserName => User?.Login;

        public DateTime PublicationDate => Video.CreatedAt.UtcDateTime;

        public bool IsMyPublication { get; }

        public bool IsVotingAvailable { get; }

        private string _likesCount;
        public string LikesCount
        {
            get => _likesCount;
            set => SetProperty(ref _likesCount, value);
        }

        public string ViewsCount => CountExtensions.ToCountViewsString(NumberOfViews);

        public string PublicationDateString => PublicationDate.ToTimeAgoPublicationString();

        private long _numberOfViews;
        public long NumberOfViews
        {
            get => _numberOfViews;
            set => SetProperty(ref _numberOfViews, value, () => RaisePropertyChanged(nameof(ViewsCount)));
        }

        public IMvxAsyncCommand ShowFullScreenVideoCommand { get; }

        protected override User User => Video.User;

        protected override void OnLikeChanged()
        {
            LikesCount = CountExtensions.ToCountString(NumberOfLikes);
        }

        protected override void OnDislikeChanged()
        {
            LikesCount = CountExtensions.ToCountString(NumberOfLikes);
        }
        
        private async Task ShowFullScreenVideoAsync()
        {
            var items = _getAllFullScreenVideosFunc?.Invoke() ?? new[] { this };
            var currentItem = items.FirstOrDefault(item => item.VideoId == VideoId);
            var index = currentItem is null ? 0 : items.IndexOfOrDefault(currentItem);
            var navigationParams = new FullScreenVideoParameter(items, index);
            if (navigationParams.Videos.Length == 0)
            {
                return;
            }

            var refreshedItems = await NavigationManager.NavigateAsync<FullScreenVideoViewModel, FullScreenVideoParameter, Dictionary<int, FullScreenVideoResult>>(navigationParams);
            if (refreshedItems == null || refreshedItems.Count == 0)
            {
                return;
            }

            Messenger.Publish(new ReloadCompetitionMessage(this));
        }
    }
}