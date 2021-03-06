using MvvmCross.Navigation;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.Navigation.Results;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile.Cashbox;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.Core.Presentation.ViewModels.Video;
using PrankChat.Mobile.Core.Providers.UserSession;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Presentation.Navigation
{
    //TODO: get rid of current service, use MvxNavigationService directly
    public class NavigationService : INavigationService
    {
        private readonly IMvxNavigationService _mvxNavigationService;
        private readonly IUserSessionProvider _userSessionProvider;

        private int? _orderId;

        public NavigationService(IMvxNavigationService mvxNavigationService,
                                 IUserSessionProvider userSessionProvider)
        {
            _mvxNavigationService = mvxNavigationService;
            _userSessionProvider = userSessionProvider;
        }

        public Task ShowCashboxView()
        {
            return _mvxNavigationService.Navigate<CashboxViewModel>();
        }

        public Task ShowLoginView()
        {
            return _mvxNavigationService.Navigate<LoginViewModel>();
        }

        public Task ShowMainView()
        {
            return _mvxNavigationService.Navigate<MainViewModel>();
        }

        public Task<OrderDetailsResult> ShowOrderDetailsView(int orderId, List<FullScreenVideo> fullScreenVideos, int currentIndex)
        {
            var parameter = new OrderDetailsNavigationParameter(orderId, fullScreenVideos, currentIndex);
            return _mvxNavigationService.Navigate<OrderDetailsViewModel, OrderDetailsNavigationParameter, OrderDetailsResult>(parameter);
        }

        public Task<bool> ShowUserProfile(int userId)
        {
            if (!Connectivity.NetworkAccess.HasConnection())
            {
                return Task.FromResult(false);
            }

            return _mvxNavigationService.Navigate<UserProfileViewModel, int, bool>(userId);
        }

        public Task AppStartFromNotification(int orderId)
        {
            _orderId = orderId;
            if (_userSessionProvider.User != null)
            {
                _mvxNavigationService.AfterNavigate += NavigatenAfterMainViewByPushNotification;
                return ShowMainView();
            }

            return ShowLoginView();
        }

        private void NavigatenAfterMainViewByPushNotification(object sender, MvvmCross.Navigation.EventArguments.IMvxNavigateEventArgs e)
        {
            _mvxNavigationService.AfterNavigate -= NavigatenAfterMainViewByPushNotification;

            if (_orderId == null)
            {
                return;
            }

            if (e.ViewModel is MainViewModel mainViewModel)
            {
                ShowOrderDetailsView(_orderId.Value, null, 0).FireAndForget();
            }
        }
    }
}
