﻿using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Managers.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order;
using PrankChat.Mobile.Core.Presentation.ViewModels.Parameters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile;
using PrankChat.Mobile.Core.Presentation.ViewModels.Walthroughs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Providers
{
    public class WalkthroughsProvider : IWalkthroughsProvider
    {
        private readonly Dictionary<Type, string> _titles = new Dictionary<Type, string>
        {
            [typeof(CompetitionsViewModel)] = Resources.WalkthrouthCompetitionsTitle,
            [typeof(CreateOrderViewModel)] = Resources.WalkthrouthCreateOrderTitle,
            [typeof(OrdersViewModel)] = Resources.WalkthrouthOrdersTitle,
            [typeof(ProfileViewModel)] = Resources.WalkthrouthProfileTitle
        };

        private readonly Dictionary<Type, string> _descriptions = new Dictionary<Type, string>
        {
            [typeof(CompetitionsViewModel)] = Resources.WalkthrouthCompetitionsDescription,
            [typeof(CreateOrderViewModel)] = Resources.WalkthrouthCreateOrderDescription,
            [typeof(OrdersViewModel)] = Resources.WalkthrouthOrdersDescription,
            [typeof(ProfileViewModel)] = Resources.WalkthrouthProfileDescription
        };

        private readonly INavigationManager _navigationManager;

        public WalkthroughsProvider(INavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
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
            var parameters = new WalthroughNavigationParameter(title, description);
            return _navigationManager.NavigateAsync<WalthroughViewModel, WalthroughNavigationParameter>(parameters);
        }
    }
}