using System;
using PrankChat.Mobile.Core.Presentation.Localization;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items
{
    public class PublicationItemViewModel : BaseItemViewModel
    {
        private long _numberOfViews;
        private DateTime _publicationDate;
        private long _numberOfLikes;

        public string ProfileName { get; }

        public string ProfilePhotoUrl { get; }

        public string VideoInformationText => $"{_numberOfViews} просмотров {_publicationDate.ToShortDateString()} месяцев назад";

        public string VideoName { get; }

        public string VideoUrl { get; }

        public string NumberOfLikesText => $"{Resources.Like} {_numberOfLikes}";

        public PublicationItemViewModel(string profileName,
                                        string profilePhotoUrl,
                                        string videoName,
                                        string videoUrl,
                                        long numberOfViews,
                                        DateTime publicationDate,
                                        long numberOfLikes)
        {
            ProfileName = profileName;
            ProfilePhotoUrl = profilePhotoUrl;
            VideoName = videoName;
            VideoUrl = videoUrl;

            _numberOfViews = numberOfViews;
            _publicationDate = publicationDate;
            _numberOfLikes = numberOfLikes;
        }
    }
}
