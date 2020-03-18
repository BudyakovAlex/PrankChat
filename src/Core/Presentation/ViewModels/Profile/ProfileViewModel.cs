using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross;
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
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Profile
{
    public class ProfileViewModel : BaseProfileViewModel, IVideoListViewModel
    {
        private readonly IPlatformService _platformService;
        private readonly IVideoPlayerService _videoPlayerService;
        private readonly IMvxMessenger _mvxMessenger;
        private readonly IExternalAuthService _externalAuthService;
        private PublicationType _selectedPublicationType;
        public PublicationType SelectedPublicationType
        {
            get => _selectedPublicationType;
            set
            {
                if (SetProperty(ref _selectedPublicationType, value))
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

        public MvxObservableCollection<PublicationItemViewModel> Items { get; } = new MvxObservableCollection<PublicationItemViewModel>();

        public MvxAsyncCommand ShowMenuCommand => new MvxAsyncCommand(OnShowMenuAsync);

        public MvxAsyncCommand ShowRefillCommand => new MvxAsyncCommand(NavigationService.ShowRefillView);

        public MvxAsyncCommand ShowWithdrawalCommand => new MvxAsyncCommand(NavigationService.ShowWithdrawalView);

        public MvxAsyncCommand LoadProfileCommand => new MvxAsyncCommand(OnLoadProfileAsync);

        public MvxAsyncCommand UpdateProfileVideoCommand => new MvxAsyncCommand(OnLoadVideoFeedAsync);

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
            SelectedPublicationType = PublicationType.MyVideosOfCreatedOrders;
            return base.Initialize();
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

        public override void ViewDestroy(bool viewFinishing = true)
        {
            foreach (var publicationItemViewModel in Items)
            {
                publicationItemViewModel.Dispose();
            }

            base.ViewDestroy(viewFinishing);
        }

        private async Task OnLoadProfileAsync()
        {
            try
            {
                IsBusy = true;

                if (!IsUserSessionInitialized)
                {
                    return;  
                }

                await ApiService.GetCurrentUserAsync();
                InitializeProfileData();
                UpdateProfileVideoCommand.Execute();
            }
            finally
            {
                IsBusy = false;
            }
        }

        //TODO: add pagination here
        private async Task OnLoadVideoFeedAsync()
        {
            if (SettingsService.User == null)
                return;

            try
            {
                IsBusy = true;

                var paginationModel = await ApiService.GetMyVideoFeedAsync(SettingsService.User.Id, SelectedPublicationType, 1, Constants.Pagination.DefaultPaginationSize);
                SetVideoList(paginationModel.Items);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void SetVideoList(IEnumerable<VideoDataModel> videos)
        {
            var publicationViewModels = videos.Select(publication =>
                new PublicationItemViewModel(
                    NavigationService,
                    DialogService,
                    _platformService,
                    Mvx.IoCProvider.Resolve<IVideoPlayerService>(),
                    ApiService,
                    ErrorHandleService,
                    _mvxMessenger,
                    SettingsService,
                    publication.User?.Name,
                    publication.User?.Avatar,
                    publication.Id,
                    publication.Title,
                    publication.Description,
                    publication.StreamUri,
                    publication.ViewsCount,
                    publication.CreatedAt.DateTime,
                    publication.LikesCount,
                    publication.ShareUri,
                    publication.IsLiked));

            Items.SwitchTo(publicationViewModels);
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
                InitializeProfileData();
            }
        }

        private async Task LogoutUserAsync()
        {
            SettingsService.User = null;
            await SettingsService.SetAccessTokenAsync(string.Empty);
            //_apiService.LogoutAsync().FireAndForget();
            _externalAuthService.LogoutFromFacebook();
            _externalAuthService.LogoutFromVkontakte();
            await NavigationService.Logout();
        }

        protected override void InitializeProfileData()
        {
            base.InitializeProfileData();

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
        }
    }
}
