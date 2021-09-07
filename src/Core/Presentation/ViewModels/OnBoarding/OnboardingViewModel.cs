using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels.Abstract;
using PrankChat.Mobile.Core.Presentation.ViewModels.Onboarding.Items;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Onboarding
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

        public string ActionTitle => IsLastSlide ? Resources.OnBoarding_Go_To_Main : Resources.LoginView_Continue_Button;

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
                new OnboardingItemViewModel(Resources.OnBoarding_First_Title, Resources.OnBoarding_First_Description, OnBoardingPageType.FirstSlide),
                new OnboardingItemViewModel(Resources.OnBoarding_Second_Title, Resources.OnBoarding_Second_Description, OnBoardingPageType.SecondSlide),
                new OnboardingItemViewModel(Resources.OnBoarding_Third_Title, Resources.OnBoarding_Third_Description, OnBoardingPageType.ThirdSlide),
                new OnboardingItemViewModel(Resources.OnBoarding_Fourth_Title, Resources.OnBoarding_Fourth_Description, OnBoardingPageType.FourthSlide),
                new OnboardingItemViewModel(Resources.OnBoarding_Fifth_Title, Resources.OnBoarding_Fifth_Description, OnBoardingPageType.FifthSlide)
            };

            Items.AddRange(items);
        }
    }
}