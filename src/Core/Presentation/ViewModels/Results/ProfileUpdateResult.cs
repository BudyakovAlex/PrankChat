namespace PrankChat.Mobile.Core.Presentation.ViewModels.Results
{
    public class ProfileUpdateResult
    {
        public ProfileUpdateResult(bool isProfileUpdated, bool isAvatarUpdated)
        {
            IsProfileUpdated = isProfileUpdated;
            IsAvatarUpdated = isAvatarUpdated;
        }

        public bool IsProfileUpdated { get; }

        public bool IsAvatarUpdated { get; }
    }
}