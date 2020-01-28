using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using Plugin.Media.Abstractions;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Mediaes;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Platforms;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Exceptions;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Messengers;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items;

namespace PrankChat.Mobile.Core.Presentation.ViewModels
{
    public class ProfileViewModel : BaseViewModel, IVideoListViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IPlatformService _platformService;
        private readonly IApiService _apiService;
        private readonly IMvxMessenger _messenger;
        private readonly IVideoPlayerService _videoPlayerService;
        private readonly ISettingsService _settingsService;
        private readonly IErrorHandleService _errorHandleService;
        private readonly IMediaService _mediaService;

        private PublicationType _selectedPublicationType;
        public PublicationType SelectedPublicationType
        {
            get => _selectedPublicationType;
            set
            {
                SetProperty(ref _selectedPublicationType, value);
                OnLoadVideoFeedAsync().FireAndForget();
            }
        }

        private string _profileName;
        public string ProfileName
        {
            get => _profileName;
            set => SetProperty(ref _profileName, value);
        }

        private string _profilePhotoUrl;
        public string ProfilePhotoUrl
        {
            get => _profilePhotoUrl;
            set => SetProperty(ref _profilePhotoUrl, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
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

        public MvxAsyncCommand ShowMenuCommand => new MvxAsyncCommand(ShowMenuAsync);

        public MvxAsyncCommand ShowRefillCommand => new MvxAsyncCommand(NavigationService.ShowRefillView);

        public MvxAsyncCommand ShowWithdrawalCommand => new MvxAsyncCommand(NavigationService.ShowWithdrawalView);

        public MvxAsyncCommand LoadProfileCommand => new MvxAsyncCommand(OnLoadProfileAsync);

        public MvxAsyncCommand UpdateProfileVideoCommand => new MvxAsyncCommand(OnLoadVideoFeedAsync);

        public MvxAsyncCommand CreateNewAvatarCommand => new MvxAsyncCommand(OnCreateNewAvatarAsync);

        public ProfileViewModel(INavigationService navigationService,
                                IDialogService dialogService,
                                IPlatformService platformService,
                                IApiService apiService,
                                IVideoPlayerService videoPlayerService,
                                IErrorHandleService errorHandleService,
                                ISettingsService settingsService,
                                IMvxMessenger messenger,
                                IMediaService mediaService) : base(navigationService)
        {
            _dialogService = dialogService;
            _platformService = platformService;
            _settingsService = settingsService;
            _apiService = apiService;
            _messenger = messenger;
            _videoPlayerService = videoPlayerService;
            _errorHandleService = errorHandleService;
            _mediaService = mediaService;
        }

        public override Task Initialize()
        {
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

                await _apiService.GetCurrentUser();

                var user = _settingsService.User;

                if (user == null)
                    return;

                ProfileName = user.Name;
                ProfilePhotoUrl = user.Avatar ?? "https://images.pexels.com/photos/2092709/pexels-photo-2092709.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500";
                Price = user.Balance.ToPriceString();
                OrdersValue = user.OrdersExecuteCount.ToCountString();
                CompletedOrdersValue = user.OrdersExecuteFinishedCount.ToCountString();
                SubscribersValue = user.SubscribersCount.ToCountString();
                SubscriptionsValue = user.SubscriptionsCount.ToCountString();
                Description = "Это профиль Адрии. #хэштег #хэштег #хэштег #хэштег #хэштег";

                _messenger.Publish(new UpdateUserProfileMessenger(this));
                await OnLoadVideoFeedAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnLoadVideoFeedAsync()
        {
            if (!CheckValidation())
                return;

            try
            {
                IsBusy = true;

                var videoBundle = await _apiService.GetMyVideoFeedAsync(_settingsService.User.Id, PublicationType.MyFeedComplete);
                SetVideoList(videoBundle);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnCreateNewAvatarAsync()
        {
            var result = await _dialogService.ShowMenuDialogAsync(new string[]
            {
                Resources.TakePhoto,
                Resources.PickPhoto,
            });

            MediaFile file = null;
            if (result == Resources.TakePhoto)
            {
                file = await _mediaService.TakePhotoAsync();
            }
            else if (result == Resources.PickPhoto)
            {
                file = await _mediaService.PickPhotoAsync();
            }

            if (file != null)
            {
                await _apiService.SendAvatarAsync(file.Path);
            }
        }

        private void SetVideoList(VideoMetadataBundleDataModel videoBundle)
        {
            if (videoBundle.Data == null)
                return;

            var publicationViewModels = videoBundle.Data.Select(x =>
                new PublicationItemViewModel(
                    NavigationService,
                    _dialogService,
                    _platformService,
                    _videoPlayerService,
                    "Name one",
                    "https://images.pexels.com/photos/2092709/pexels-photo-2092709.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500",
                    x.Title,
                    x.StreamUri,
                    x.ViewsCount,
                    x.CreatedAt.DateTime,
                    x.RepostsCount,
                    x.ShareUri));

            Items.SwitchTo(publicationViewModels);
        }

        private bool CheckValidation()
        {
            if (_settingsService.User == null)
            {
                _errorHandleService.HandleException(new UserVisibleException("Пользователь не может быть пустым."));
                return false;
            }

            return true;
        }

        private async Task ShowMenuAsync()
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

            var result = await _dialogService.ShowMenuDialogAsync(items, Resources.Cancel);
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
                await LogoutUser();
            }
        }

        private async Task LogoutUser()
        {
            _settingsService.User = null;
            await _settingsService.SetAccessTokenAsync(string.Empty);
            //_apiService.LogoutAsync().FireAndForget();
            await NavigationService.Logout();
        }
    }
}
