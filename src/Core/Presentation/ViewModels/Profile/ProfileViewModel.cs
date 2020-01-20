using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.Platforms;
using PrankChat.Mobile.Core.ApplicationServices.Storages;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items;

namespace PrankChat.Mobile.Core.Presentation.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IPlatformService _platformService;
        private readonly IStorageService _storageService;

        private string _profileName;
        private string _description;
        private string _price;
        private string _ordersValue;
        private string _completedOrdersValue;
        private string _subscriptionsValue;
        private string _subscribersValue;
        private string _profilePhotoUrl;

        public MvxAsyncCommand ShowMenuCommand => new MvxAsyncCommand(async () =>
        {
            var items = new string[]
            {
                Resources.ProfileView_Menu_Favourites,
                Resources.ProfileView_Menu_TaskSubscriptions,
                Resources.ProfileView_Menu_Faq,
                Resources.ProfileView_Menu_Support,
                Resources.ProfileView_Menu_Settings,
            };

            await _dialogService.ShowMenuDialogAsync(items, Resources.ProfileView_Menu_LogOut);
        });

        public ICommand ShowRefillCommand => new MvxAsyncCommand(NavigationService.ShowRefillView);

        public ICommand ShowWithdrawalCommand => new MvxAsyncCommand(NavigationService.ShowWithdrawalView);

        public string ProfileName
        {
            get => _profileName;
            set => SetProperty(ref _profileName, value);
        }

        public string ProfilePhotoUrl
        {
            get => _profilePhotoUrl;
            set => SetProperty(ref _profilePhotoUrl, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public string Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }

        public string OrdersValue
        {
            get => _ordersValue;
            set => SetProperty(ref _ordersValue, value);
        }

        public string CompletedOrdersValue
        {
            get => _completedOrdersValue;
            set => SetProperty(ref _completedOrdersValue, value);
        }

        public string SubscribersValue
        {
            get => _subscribersValue;
            set => SetProperty(ref _subscribersValue, value);
        }

        public string SubscriptionsValue
        {
            get => _subscriptionsValue;
            set => SetProperty(ref _subscriptionsValue, value);
        }

        public MvxObservableCollection<PublicationItemViewModel> Items { get; } = new MvxObservableCollection<PublicationItemViewModel>();

        public ProfileViewModel(INavigationService navigationService,
                                IDialogService dialogService,
                                IPlatformService platformService,
                                IStorageService storageService) : base(navigationService)
        {
            _dialogService = dialogService;
            _platformService = platformService;
            _storageService = storageService;
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            InitializeProfile();

            await InitializePublications();
        }

        private void InitializeProfile()
        {
            var user = _storageService.User;

            ProfileName = user.Name;
            ProfilePhotoUrl = user.Avatar ?? "https://images.pexels.com/photos/2092709/pexels-photo-2092709.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500";
            Price = user.Balance.ToPriceUIString();
            OrdersValue = 1000.ToUICountString();
            CompletedOrdersValue = 1900.ToUICountString();
            SubscribersValue = 1123.ToUICountString();
            SubscriptionsValue = 112312122.ToUICountString();

            Description = "Это профиль Адрии. #хэштег #хэштег #хэштег #хэштег #хэштег";
        }

        private Task InitializePublications()
        {
            Items.Add(new PublicationItemViewModel(
                NavigationService,
                _dialogService,
                _platformService,
                "Name one",
                "https://images.pexels.com/photos/2092709/pexels-photo-2092709.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500",
                "Name video one",
                "https://ksassets.timeincuk.net/wp/uploads/sites/55/2019/04/GettyImages-1136749971-920x584.jpg",
                134,
                new System.DateTime(2018, 4, 24),
                245,
                ""));

            Items.Add(new PublicationItemViewModel(
                NavigationService,
                _dialogService,
                _platformService,
                "Name two",
                "https://images.pexels.com/photos/2092709/pexels-photo-2092709.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500",
                "Name video two Name video two Name video two Name video two Name video two Name video two Name video two",
                "https://cdn.pixabay.com/photo/2016/11/30/09/27/hacker-1872291_960_720.jpg",
                134,
                new System.DateTime(2018, 4, 24),
                245,
                ""));

            Items.Add(new PublicationItemViewModel(
                NavigationService,
                _dialogService,
                _platformService,
                "Name three",
                "https://images.pexels.com/photos/2092709/pexels-photo-2092709.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500",
                "Name video three",
                "https://images.pexels.com/photos/326055/pexels-photo-326055.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500",
                134,
                new System.DateTime(2018, 4, 24),
                245,
                ""));

            return Task.CompletedTask;
        }
    }
}
