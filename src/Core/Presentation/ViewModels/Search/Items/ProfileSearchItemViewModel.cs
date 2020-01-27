using System.Collections.Generic;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using PrankChat.Mobile.Core.Infrastructure.Extensions;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Search.Items
{
    public class ProfileSearchItemViewModel : BaseItemViewModel
    {
        public string ProfileName { get; }

        public string ProfileShortName { get; }

        public string ProfileDescription { get; }

        public string ImageUrl { get; }

        public ProfileSearchItemViewModel(string profileName, string profileDescription, string photoUrl)
        {
            ProfileName = profileName;
            ProfileShortName = profileName.ToShortenName();
            ProfileDescription = profileDescription;
            ImageUrl = photoUrl;
        }
    }
}
