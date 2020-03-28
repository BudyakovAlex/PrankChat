﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.ExternalAuth;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Platforms;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.FilterTypes;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Profile
{
    public class ProfileViewModel : BaseProfileViewModel
    {
        private readonly IPlatformService _platformService;
        private readonly IVideoPlayerService _videoPlayerService;
        private readonly IMvxMessenger _mvxMessenger;
        private readonly IExternalAuthService _externalAuthService;

        private ProfileOrderType _selectedOrderType;
        public ProfileOrderType SelectedOrderType
        {
            get => _selectedOrderType;
            set
            {
                if (SetProperty(ref _selectedOrderType, value))
                {
                    LoadProfileCommand.Execute();
                }
            }
        }

        private string _price;
        public string Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }

        private string _ordersValue;
        public string OrdersValue
        {
            get => _ordersValue;
            set => SetProperty(ref _ordersValue, value);
        }

        private string _completedOrdersValue;
        public string CompletedOrdersValue
        {
            get => _completedOrdersValue;
            set => SetProperty(ref _completedOrdersValue, value);
        }

        private string _subscribersValue;
        public string SubscribersValue
        {
            get => _subscribersValue;
            set => SetProperty(ref _subscribersValue, value);
        }

        private string _subscriptionsValue;
        public string SubscriptionsValue
        {
            get => _subscriptionsValue;
            set => SetProperty(ref _subscriptionsValue, value);
        }

        public MvxObservableCollection<OrderItemViewModel> Items { get; set; } = new MvxObservableCollection<OrderItemViewModel>();

        public MvxAsyncCommand ShowMenuCommand => new MvxAsyncCommand(OnShowMenuAsync);

        public MvxAsyncCommand ShowRefillCommand => new MvxAsyncCommand(NavigationService.ShowRefillView);

        public MvxAsyncCommand ShowWithdrawalCommand => new MvxAsyncCommand(NavigationService.ShowWithdrawalView);

        public MvxAsyncCommand LoadProfileCommand => new MvxAsyncCommand(OnLoadProfileAsync);

        public MvxAsyncCommand ShowUpdateProfileCommand => new MvxAsyncCommand(OnShowUpdateProfileAsync);

        public ProfileViewModel(INavigationService navigationService,
                                IDialogService dialogService,
                                IPlatformService platformService,
                                IApiService apiService,
                                IVideoPlayerService videoPlayerService,
                                IErrorHandleService errorHandleService,
                                ISettingsService settingsService,
                                IMvxMessenger mvxMessenger,
                                IExternalAuthService externalAuthService)
            : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
            _platformService = platformService;
            _videoPlayerService = videoPlayerService;
            _mvxMessenger = mvxMessenger;
            _externalAuthService = externalAuthService;
        }

        public override Task Initialize()
        {
            SelectedOrderType = ProfileOrderType.MyOrders;
            base.Initialize();
            return LoadProfileCommand.ExecuteAsync();
        }

        public override void ViewDisappearing()
        {
            _videoPlayerService.Pause();
            base.ViewDisappearing();
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            _videoPlayerService.Play();
        }

        private async Task OnLoadProfileAsync()
        {
            try
            {
                IsBusy = true;
                await ApiService.GetCurrentUserAsync();
                await InitializeProfileData();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnShowMenuAsync()
        {
            var items = new string[]
            {
                Resources.ProfileView_Menu_Favourites,
                Resources.ProfileView_Menu_TaskSubscriptions,
                Resources.ProfileView_Menu_Faq,
                Resources.ProfileView_Menu_Support,
                Resources.ProfileView_Menu_Settings,
                Resources.ProfileView_Menu_LogOut,
            };

            var result = await DialogService.ShowMenuDialogAsync(items, Resources.Cancel);
            if (string.IsNullOrWhiteSpace(result))
                return;

            if (result == Resources.ProfileView_Menu_Favourites)
            {
            }
            else if (result == Resources.ProfileView_Menu_TaskSubscriptions)
            {
            }
            else if (result == Resources.ProfileView_Menu_Faq)
            {
            }
            else if (result == Resources.ProfileView_Menu_Support)
            {
            }
            else if (result == Resources.ProfileView_Menu_Settings)
            {
            }
            else if (result == Resources.ProfileView_Menu_LogOut)
            {
                await LogoutUserAsync();
            }
        }

        private async Task OnShowUpdateProfileAsync()
        {
            var isUpdated = await NavigationService.ShowUpdateProfileView();
            if (isUpdated)
            {
                await InitializeProfileData();
            }
        }

        private async Task LogoutUserAsync()
        {
            SettingsService.User = null;
            await SettingsService.SetAccessTokenAsync(string.Empty);
            _externalAuthService.LogoutFromFacebook();
            _externalAuthService.LogoutFromVkontakte();
            await NavigationService.Logout();
        }

        protected override async Task InitializeProfileData()
        {
            await base.InitializeProfileData();

            if (!IsUserSessionInitialized)
            {
                return;
            }

            var user = SettingsService.User;
            ProfilePhotoUrl = user.Avatar;
            Price = user.Balance.ToPriceString();
            OrdersValue = user.OrdersExecuteCount.ToCountString();
            CompletedOrdersValue = user.OrdersExecuteFinishedCount.ToCountString();
            SubscribersValue = user.SubscribersCount.ToCountString();
            SubscriptionsValue = user.SubscriptionsCount.ToCountString();

            var orders = await GetOrdersAsync();
            Items.SwitchTo(orders.Select(order => new OrderItemViewModel(
                NavigationService,
                SettingsService,
                _mvxMessenger,
                order.Id,
                order.Title,
                order.Customer?.Avatar,
                order.Customer?.Name,
                order.Price,
                order.ActiveTo,
                order.DurationInHours,
                order.Status ?? OrderStatusType.None,
                order.Customer?.Id)));
        }

        protected virtual async Task<IEnumerable<OrderDataModel>> GetOrdersAsync()
        {
            switch (SelectedOrderType)
            {
                case ProfileOrderType.MyOrders:
                case ProfileOrderType.OrdersCompletedByMe:
                    var filterEnum = SelectedOrderType == ProfileOrderType.MyOrders ? OrderFilterType.MyOwn : OrderFilterType.InProgress;
                    var orders = await ApiService.GetOrdersAsync(filterEnum);
                    return orders.OrderBy(x => x.Status);
            }

            return Enumerable.Empty<OrderDataModel>();
        }
    }
}
