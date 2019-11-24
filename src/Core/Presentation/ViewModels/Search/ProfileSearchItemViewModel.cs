using System.Collections.Generic;
using FFImageLoading.Transformations;
using FFImageLoading.Work;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Search
{
    public class ProfileSearchItemViewModel : BaseItemViewModel
    {
        public string ProfileName { get; }

        public string ProfileDescription { get; }

        public string ImageUrl { get; }

        public double DownsampleWidth { get; } = 100;

        public List<ITransformation> Transformations => new List<ITransformation> { new CircleTransformation() };

        public ProfileSearchItemViewModel(string profileName, string profileDescription)
        {
            ProfileName = profileName;
            ProfileDescription = profileDescription;
            ImageUrl = "https://images.pexels.com/photos/2092709/pexels-photo-2092709.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500";
        }
    }
}
