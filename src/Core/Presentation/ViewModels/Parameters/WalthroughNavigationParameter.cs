﻿namespace PrankChat.Mobile.Core.Presentation.ViewModels.Parameters
{
    public class WalthroughNavigationParameter
    {
        public WalthroughNavigationParameter(string title, string description)
        {
            Title = title;
            Description = description;
        }

        public string Title { get; }

        public string Description { get; }
    }
}