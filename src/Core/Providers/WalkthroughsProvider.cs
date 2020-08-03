using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Providers
{
    public class WalkthroughsProvider : IWalkthroughsProvider
    {
        private readonly INavigationService _navigationService;

        private readonly Dictionary<Type, string> _titles = new Dictionary<Type, string>
        {
            [typeof(CompetitionsViewModel)] = Resources.Walkthrouth_Competitions_Title,
            [typeof(CreateOrderViewModel)] = Resources.Walkthrouth_CreateOrder_Title,
            [typeof(OrdersViewModel)] = Resources.Walkthrouth_Orders_Title,
            [typeof(ProfileViewModel)] = Resources.Walkthrouth_Profile_Title
        };

        private readonly Dictionary<Type, string> _descriptions = new Dictionary<Type, string>
        {
            [typeof(CompetitionsViewModel)] = Resources.Walkthrouth_Competitions_Description,
            [typeof(CreateOrderViewModel)] = Resources.Walkthrouth_CreateOrder_Description,
            [typeof(OrdersViewModel)] = Resources.Walkthrouth_Orders_Description,
            [typeof(ProfileViewModel)] = Resources.Walkthrouth_Profile_Description
        };

        public WalkthroughsProvider(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public bool CheckCanShowOnFirstLoad<TViewModel>() where TViewModel : IMvxViewModel
        {
            var type = typeof(TViewModel);
            return Xamarin.Essentials.Preferences.Get(type.Name, true);
        }

        public Task ShowWalthroughAsync<TViewModel>() where TViewModel : IMvxViewModel
        {
            var type = typeof(TViewModel);
            Xamarin.Essentials.Preferences.Set(type.Name, false);

            var title = _titles[typeof(TViewModel)];
            var description = _descriptions[typeof(TViewModel)];
            return _navigationService.ShowWalthroughView(title, description);
        }
    }
}