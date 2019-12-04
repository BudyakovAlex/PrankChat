using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items;

namespace PrankChat.Mobile.Core.Presentation.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private string _profileName;
        private string _description;
        private string _price;
        private string _ordersValue;
        private string _completedOrdersValue;
        private string _subscriptionsValue;
        private string _subscribersValue;

        public ProfileViewModel(INavigationService navigationService, IDialogService dialogService) : base(navigationService)
        {
            _dialogService = dialogService;

            ProfileName = "Adria";
            Description = "Это профиль Адрии. #хэштег #хэштег #хэштег #хэштег #хэштег";
            Price = "100 000 ₽";
            OrdersValue = "200";
            CompletedOrdersValue = "10";
            SubscribersValue = "1k";
            SubscriptionsValue = "100";
        }

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

            await _dialogService.ShowFilterSelectionAsync(items, Resources.ProfileView_Menu_LogOut);
        });

        public string ProfileName
        {
            get => _profileName;
            set => SetProperty(ref _profileName, value);
        }

        public double DownsampleWidth { get; } = 100;

        public List<ITransformation> Transformations => new List<ITransformation> { new CircleTransformation() };

        public string ProfilePhotoUrl { get; } = "https://images.pexels.com/photos/2092709/pexels-photo-2092709.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500";

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

        public MvxObservableCollection<BaseItemViewModel> Items { get; } = new MvxObservableCollection<BaseItemViewModel>();

        public override async Task Initialize()
        {
            await base.Initialize();

            await InitializePublications();
        }

        private Task InitializePublications()
        {
            Items.Add(new PublicationItemViewModel("Name one",
                                                   "https://images.pexels.com/photos/2092709/pexels-photo-2092709.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500",
                                                   "Name video one",
                                                   "https://ksassets.timeincuk.net/wp/uploads/sites/55/2019/04/GettyImages-1136749971-920x584.jpg",
                                                   134,
                                                   new System.DateTime(2018, 4, 24),
                                                   245));

            Items.Add(new PublicationItemViewModel("Name two",
                                       "https://images.pexels.com/photos/2092709/pexels-photo-2092709.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500",
                                       "Name video two Name video two Name video two Name video two Name video two Name video two Name video two",
                                       "https://cdn.pixabay.com/photo/2016/11/30/09/27/hacker-1872291_960_720.jpg",
                                       134,
                                       new System.DateTime(2018, 4, 24),
                                       245));

            Items.Add(new PublicationItemViewModel("Name three",
                           "https://images.pexels.com/photos/2092709/pexels-photo-2092709.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500",
                           "Name video three",
                           "https://images.pexels.com/photos/326055/pexels-photo-326055.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500",
                           134,
                           new System.DateTime(2018, 4, 24),
                           245));

            return Task.CompletedTask;
        }
    }
}
