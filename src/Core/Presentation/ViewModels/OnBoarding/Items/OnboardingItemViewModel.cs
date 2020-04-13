using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Onboarding.Items
{
    public class OnboardingItemViewModel : BaseItemViewModel
    {
        public OnboardingItemViewModel(string title, string description, OnBoardingPageType type)
        {
            Title = title;
            Description = description;
            Type = type;
        }

        public string Title { get; }

        public string Description { get; }

        public OnBoardingPageType Type { get; }
    }
}
