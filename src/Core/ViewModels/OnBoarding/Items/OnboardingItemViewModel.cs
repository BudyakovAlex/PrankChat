using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.ViewModels.Abstract;

namespace PrankChat.Mobile.Core.ViewModels.Onboarding.Items
{
    public class OnboardingItemViewModel : BaseViewModel
    {
        public OnboardingItemViewModel(
            string title,
            string description,
            OnBoardingPageType type)
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
