using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Presentation.ViewModels.Onboarding.Items;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Onboarding
{
    public class OnboardingViewModel : BaseViewModel
    {
        public OnboardingViewModel(INavigationService navigationService,
                                   IErrorHandleService errorHandleService,
                                   IApiService apiService,
                                   IDialogService dialogService,
                                   ISettingsService settingsService)
            : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
            Items = new MvxObservableCollection<OnboardingItemViewModel>();
            ActionCommand = new MvxAsyncCommand(ExecuteActionAsync);

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
                    RaisePropertyChanged(nameof(IsLastSlide));
                    RaisePropertyChanged(nameof(ActionTitle));
                }
            }
        }

        private async Task ExecuteActionAsync()
        {
            if (IsLastSlide)
            {
                Preferences.Set(Constants.Keys.IsOnBoardingShown, true);
                if (VersionTracking.IsFirstLaunchEver || SettingsService.User != null)
                {
                    await NavigationService.ShowMainView();
                    return;
                }

                await NavigationService.ShowLoginView();
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