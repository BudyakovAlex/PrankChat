using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items
{
    public class CompetitionVideoViewModel : BaseItemViewModel
    {
        private readonly IApiService _apiService;

        private CancellationTokenSource _cancellationSendingLikeTokenSource;

        public ICommand LikeCommand { get; }

        public int Id { get; }
        public string VideoUrl { get; }
        public string UserName { get; }
        public string AvatarUrl { get; }
        public string StubImageUrl { get; }
        public DateTime PublicationDate { get; }
        public bool IsMyPublication { get; }
        public bool IsVotingCompleted { get; }

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
                    RaisePropertyChanged(nameof(LikesCount));
                }
            }
        }

        public string LikesCount => CountExtensions.ToCountString(NumberOfLikes);
        public string ViewsCount => CountExtensions.ToCountViewsString(NumberOfViews);
        public bool CanVoteVideo => !IsVotingCompleted && !IsMyPublication;

        private bool _isLiked;
        public bool IsLiked
        {
            get => _isLiked;
            set => SetProperty(ref _isLiked, value);
        }

        public CompetitionVideoViewModel(IApiService apiService,
                                         int id,
                                         string videoUrl,
                                         string userName,
                                         string avatarUrl,
                                         long numberOfLikes,
                                         long numberOfViews,
                                         DateTime publicationDate,
                                         bool isLiked,
                                         bool isMyPublication,
                                         bool isVotingCompleted)
        {
            _apiService = apiService;
            Id = id;
            VideoUrl = videoUrl;
            UserName = userName;
            AvatarUrl = avatarUrl;
            NumberOfLikes = numberOfLikes;
            NumberOfViews = numberOfViews;
            PublicationDate = publicationDate;
            IsLiked = isLiked;
            IsMyPublication = isMyPublication;
            IsVotingCompleted = isVotingCompleted;
            LikeCommand = new MvxCommand(OnLike);
        }

        private void OnLike()
        {
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
                await _apiService.SendLikeAsync(Id, IsLiked, _cancellationSendingLikeTokenSource.Token);
            }
            finally
            {
                _cancellationSendingLikeTokenSource?.Dispose();
                _cancellationSendingLikeTokenSource = null;
            }
        }
    }
}