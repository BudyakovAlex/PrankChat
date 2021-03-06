using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.BusinessServices.TaskSchedulers;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Managers.Navigation;
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
        private readonly INavigationManager _navigationManager;
        private readonly IUserSessionProvider _userSessionProvider;
        private readonly IBackgroundTaskScheduler _backgroundTaskScheduler;

        public CustomAppStart(IMvxApplication application,
                              IMvxNavigationService mvxNavigationService,
                              INavigationManager navigationManager,
                              IUserSessionProvider userSessionProvider,
                              IBackgroundTaskScheduler backgroundTaskScheduler)
            : base(application, mvxNavigationService)
        {
            _navigationManager = navigationManager;
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
                return _navigationManager.NavigateAsync<OnboardingViewModel>();
            }

            if (VersionTracking.IsFirstLaunchEver || _userSessionProvider.User != null)
            {
                return _navigationManager.NavigateAsync<MainViewModel>();
            }

            return _navigationManager.NavigateAsync<LoginViewModel>();
        }
    }
}
