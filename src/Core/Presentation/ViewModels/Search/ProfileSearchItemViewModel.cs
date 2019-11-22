namespace PrankChat.Mobile.Core.Presentation.ViewModels.Search
{
    public class ProfileSearchItemViewModel : BaseItemViewModel
    {
        public ProfileSearchItemViewModel(string profileName, string profileDescription)
        {
            ProfileName = profileName;
            ProfileDescription = profileDescription;
        }

        public string ProfileName { get; }

        public string ProfileDescription { get; }
    }
}
