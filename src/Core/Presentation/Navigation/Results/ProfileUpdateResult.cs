using System;
namespace PrankChat.Mobile.Core.Presentation.Navigation.Results
{
    public class ProfileUpdateResult
    {
        public bool IsProfileUpdated { get; }

        public bool IsAvatarUpdated { get; }

        public ProfileUpdateResult(bool _isProfileUpdated, bool isAvatarUpdated)
        {
            IsProfileUpdated = _isProfileUpdated;
            IsAvatarUpdated = isAvatarUpdated;
        }
    }
}
