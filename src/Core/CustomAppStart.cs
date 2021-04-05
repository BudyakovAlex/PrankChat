using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Managers.Navigation;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.Navigation.Results;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.Core.Presentation.ViewModels.Onboarding;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order;
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
        
        private int? _orderId;

        public CustomAppStart(IMvxApplication application,
                              IMvxNavigationService mvxNavigationService,
                              INavigationManager navigationManager,
                              IUserSessionProvider userSessionProvider)
            : base(application, mvxNavigationService)
        {
            _navigationManager = navigationManager;
            _userSessionProvider = userSessionProvider;
        }

        protected override Task NavigateToFirstViewModel(object hint = null)
        {
            if (hint is int orderId)
            {
                return AppStartFromNotification(orderId);
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

        private Task AppStartFromNotification(int orderId)
        {
            _orderId = orderId;
            if (_userSessionProvider.User != null)
            {
                NavigationService.AfterNavigate += NavigatenAfterMainViewByPushNotification;
                return _navigationManager.NavigateAsync<MainViewModel>();
            }

            return _navigationManager.NavigateAsync<LoginViewModel>();
        }

        private void NavigatenAfterMainViewByPushNotification(object sender, MvvmCross.Navigation.EventArguments.IMvxNavigateEventArgs e)
        {
            NavigationService.AfterNavigate -= NavigatenAfterMainViewByPushNotification;

            if (_orderId == null)
            {
                return;
            }

            if (e.ViewModel is MainViewModel mainViewModel)
            {
                var parameter = new OrderDetailsNavigationParameter(_orderId.Value, null, 0);
                _navigationManager.NavigateAsync<OrderDetailsViewModel, OrderDetailsNavigationParameter, OrderDetailsResult>(parameter).FireAndForget();
            }
        }
    }
}
