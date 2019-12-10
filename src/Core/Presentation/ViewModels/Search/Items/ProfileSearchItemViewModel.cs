using System.Collections.Generic;
using FFImageLoading.Transformations;
using FFImageLoading.Work;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Search.Items
{
    public class ProfileSearchItemViewModel : BaseItemViewModel
    {
        public string ProfileName { get; }

        public string ProfileDescription { get; }

        public string ImageUrl { get; }

        public ProfileSearchItemViewModel(string profileName, string profileDescription)
        {
            ProfileName = profileName;
            ProfileDescription = profileDescription;
            ImageUrl = "https://images.pexels.com/photos/2092709/pexels-photo-2092709.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500";
        }
    }
}
