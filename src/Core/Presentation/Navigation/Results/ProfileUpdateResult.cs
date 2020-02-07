﻿using System;
namespace PrankChat.Mobile.Core.Presentation.Navigation.Results
{
    public class ProfileUpdateResult
    {
        public bool IsProfileUpdated { get; set; }

        public ProfileUpdateResult(bool _isProfileUpdated)
        {
            IsProfileUpdated = _isProfileUpdated;
        }
    }
}
