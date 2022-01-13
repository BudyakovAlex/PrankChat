using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Managers.Navigation;
using PrankChat.Mobile.Core.ViewModels;
using PrankChat.Mobile.Core.ViewModels.Onboarding;
using PrankChat.Mobile.Core.ViewModels.Order;
using PrankChat.Mobile.Core.ViewModels.Parameters;
using PrankChat.Mobile.Core.ViewModels.Registration;
using PrankChat.Mobile.Core.Providers.UserSession;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Badge.Plugin;

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
            if (_userSessionProvider.User is null)
            {
                MainThread.BeginInvokeOnMainThread(() => CrossBadge.Current.ClearBadge());
            }

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
                NavigationService.WillNavigate += WillNavigateMainViewByPushNotification;
                return _navigationManager.NavigateAsync<MainViewModel>();
            }

            return _navigationManager.NavigateAsync<LoginViewModel>();
        }

        private void WillNavigateMainViewByPushNotification(object sender, MvvmCross.Navigation.EventArguments.IMvxNavigateEventArgs e)
        {
            NavigationService.WillNavigate -= WillNavigateMainViewByPushNotification;

            if (_orderId == null)
            {
                return;
            }

            if (e.ViewModel is MainViewModel mainViewModel)
            {
                var parameter = new OrderDetailsNavigationParameter(_orderId.Value, null, 0);
                _navigationManager.NavigateAsync<OrderDetailsViewModel, OrderDetailsNavigationParameter, bool>(parameter).FireAndForget();
            }
        }
    }
}
