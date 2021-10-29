using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.ViewModels.Abstract;
using PrankChat.Mobile.Core.ViewModels.Onboarding.Items;
using PrankChat.Mobile.Core.ViewModels.Registration;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.ViewModels.Onboarding
{
    public class OnboardingViewModel : BasePageViewModel
    {
        public OnboardingViewModel()
        {
            Items = new MvxObservableCollection<OnboardingItemViewModel>();
            ActionCommand = this.CreateCommand(ExecuteActionAsync);

            ProduceSlides();
        }

        public MvxObservableCollection<OnboardingItemViewModel> Items { get; }

        public int ItemsCount => Items.Count;

        public IMvxAsyncCommand ActionCommand { get; }

        public bool IsLastSlide => ItemsCount == SelectedIndex + 1;

        public string ActionTitle => IsLastSlide ? Resources.GoToFeed : Resources.Continue;

        private int _selectedIndex;
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (value > ItemsCount - 1)
                {
                    return;
                }

                if (SetProperty(ref _selectedIndex, value))
                {
                    RaisePropertiesChanged(nameof(IsLastSlide), nameof(ActionTitle));
                }
            }
        }

        private async Task ExecuteActionAsync()
        {
            if (IsLastSlide)
            {
                Preferences.Set(Constants.Keys.IsOnBoardingShown, true);
                if (VersionTracking.IsFirstLaunchEver || UserSessionProvider.User != null)
                {
                    await NavigationManager.NavigateAsync<MainViewModel>();
                    return;
                }

                await NavigationManager.NavigateAsync<LoginViewModel>();
                return;
            }

            SelectedIndex += 1;
        }

        private void ProduceSlides()
        {
            var items = new[]
            {
                new OnboardingItemViewModel(Resources.OnBoardingFirstTitle, Resources.OnBoardingFirstDescription, OnBoardingPageType.FirstSlide),
                new OnboardingItemViewModel(Resources.OnBoardingSecondTitle, Resources.OnBoardingSecondDescription, OnBoardingPageType.SecondSlide),
                new OnboardingItemViewModel(Resources.OnBoardingThirdTitle, Resources.OnBoardingThirdDescription, OnBoardingPageType.ThirdSlide),
                new OnboardingItemViewModel(Resources.OnBoardingFourthTitle, Resources.OnBoardingFourthDescription, OnBoardingPageType.FourthSlide),
                new OnboardingItemViewModel(Resources.OnBoardingFifthTitle, Resources.OnBoardingFifthDescription, OnBoardingPageType.FifthSlide)
            };

            Items.AddRange(items);
        }
    }
}