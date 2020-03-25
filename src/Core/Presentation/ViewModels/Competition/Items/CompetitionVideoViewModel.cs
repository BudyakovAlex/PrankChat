using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items
{
    public class CompetitionVideoViewModel : BaseItemViewModel
    {
        public string VideoUrl { get; }
        public string UserName { get; }
        public string AvatarUrl { get; }
        public string LikesCount { get; }
        public string ViewsCount { get; }
        public string StubImageUrl { get; }
        public DateTime PublicationDate { get; }
        public bool IsLiked { get; }
        public bool IsMyPublication { get; }

        public ICommand LikeCommand { get; }

        public CompetitionVideoViewModel(string videoUrl,
                                         string userName,
                                         string avatarUrl,
                                         string likesCount,
                                         string viewsCount,
                                         DateTime publicationDate,
                                         bool isLiked)
        {
            VideoUrl = videoUrl;
            UserName = userName;
            AvatarUrl = avatarUrl;
            LikesCount = likesCount;
            ViewsCount = viewsCount;
            PublicationDate = publicationDate;
            IsLiked = isLiked;

            LikeCommand = new MvxAsyncCommand(LikeAsync);
        }

        private async Task LikeAsync()
        {
            //TODO: add logic here
        }
    }
}