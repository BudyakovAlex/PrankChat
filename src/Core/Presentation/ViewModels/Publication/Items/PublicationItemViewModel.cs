using System;
using System.Collections.Generic;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using PrankChat.Mobile.Core.Presentation.Localization;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items
{
    public class PublicationItemViewModel : BaseItemViewModel
    {
        private int _numberOfViews;
        private DateTime _publicationDate;
        private int _numberOfLikes;

        public string ProfileName { get; }

        public string ProfilePhotoUrl { get; }

        public string VideoInformationText => $"{_numberOfViews} просмотров {_publicationDate.ToShortDateString()} месяцев назад";

        public string VideoName { get; }

        public string VideoUrl { get; }

        public string NumberOfLikesText => $"{Resources.Like} {_numberOfLikes}";

        public double DownsampleWidth { get; } = 100;

        public List<ITransformation> Transformations => new List<ITransformation> { new CircleTransformation() };

        public PublicationItemViewModel(string profileName,
                                        string profilePhotoUrl,
                                        string videoName,
                                        string videoUrl,
                                        int numberOfViews,
                                        DateTime publicationDate,
                                        int numberOfLikes)
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
