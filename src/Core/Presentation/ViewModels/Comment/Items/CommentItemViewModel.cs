using System;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Comment.Items
{
    public class CommentItemViewModel : BaseItemViewModel
    {
        private DateTime _date;

        public string ProfileName { get; }

        public string ProfileShortName { get; }

        public string ProfilePhotoUrl { get; }

        public string Comment { get; }

        public string DateText => _date.ToTimeAgoCommentString();

        public CommentItemViewModel(string profileName, string profilePhotoUrl, string comment, DateTime date)
        {
            ProfileName = profileName;
            ProfilePhotoUrl = profilePhotoUrl;
            ProfileShortName = profileName.ToShortenName();
            Comment = comment;
            _date = date;
        }
    }
}
