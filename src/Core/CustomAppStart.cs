using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.BusinessServices.TaskSchedulers;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.Core.Presentation.ViewModels.Onboarding;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.Core.Providers.UserSession;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core
{
    public class CustomAppStart : MvxAppStart
    {
        private readonly INavigationService _navigationService;
        private readonly IUserSessionProvider _userSessionProvider;
        private readonly IBackgroundTaskScheduler _backgroundTaskScheduler;

        public CustomAppStart(IMvxApplication application,
                              IMvxNavigationService mvxNavigationService,
                              INavigationService navigationService,
                              IUserSessionProvider userSessionProvider,
                              IBackgroundTaskScheduler backgroundTaskScheduler)
            : base(application, mvxNavigationService)
        {
            _navigationService = navigationService;
            _userSessionProvider = userSessionProvider;
            _backgroundTaskScheduler = backgroundTaskScheduler;
        }

        protected override Task NavigateToFirstViewModel(object hint = null)
        {
            _backgroundTaskScheduler.Start();

            if (hint is int orderId)
            {
                return _navigationService.AppStartFromNotification(orderId);
            }

            return StartApp();
        }

        private Task StartApp()
        {
            var isOnBoardingShown = Preferences.Get(Constants.Keys.IsOnBoardingShown, false);
            if (!isOnBoardingShown)
            {
                return NavigationService.Navigate<OnboardingViewModel>();
            }

            if (VersionTracking.IsFirstLaunchEver || _userSessionProvider.User != null)
            {
                return NavigationService.Navigate<MainViewModel>();
            }

            return NavigationService.Navigate<LoginViewModel>();
        }
    }
}
