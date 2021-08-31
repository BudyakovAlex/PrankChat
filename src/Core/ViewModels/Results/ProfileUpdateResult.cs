namespace PrankChat.Mobile.Core.ViewModels.Results
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