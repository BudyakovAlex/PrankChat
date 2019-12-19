using System;
using System.Collections.Generic;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Publication
{
    public class PublicationDetailsViewModel : BaseViewModel
    {
        private int _numberOfViews = 134;
        private DateTime _publicationDate = new DateTime(2018, 4, 24);
        private DateTime _commentDate = new DateTime(2018, 4, 24);
        private int _numberOfLikes = 245;
        private int _numberOfComments = 125;

        #region Profile

        public string ProfileName { get; } = "Name one";

        public string ProfilePhotoUrl { get; } = "https://images.pexels.com/photos/2092709/pexels-photo-2092709.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500";

        #endregion

        #region Video

        public string VideoInformationText => $"{_numberOfViews} просмотров {_publicationDate.ToShortDateString()} месяцев назад";

        public string VideoName { get; } = "Name video one";

        public string VideoDescription { get; } = "Description video one";

        public string VideoUrl { get; } = "https://ksassets.timeincuk.net/wp/uploads/sites/55/2019/04/GettyImages-1136749971-920x584.jpg";

        #endregion

        #region Last Comment

        public string CommentatorName { get; } = "Name one";

        public string CommentatorPhotoUrl { get; } = "https://images.pexels.com/photos/2092709/pexels-photo-2092709.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500";

        public string CommentDateText => $"{_commentDate}";

        #endregion

        public string NumberOfLikesText => $"{_numberOfLikes}";

        public string NumberOfCommentText => $"{_numberOfComments}";

        public MvxAsyncCommand OpenCommentsCommand => new MvxAsyncCommand(() => NavigationService.ShowCommentsView());

        public PublicationDetailsViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }
    }
}
