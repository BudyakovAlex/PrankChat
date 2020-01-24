﻿using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.Storages;
using PrankChat.Mobile.Core.Presentation.Messengers;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IMvxMessenger _messenger;
        private readonly IStorageService _storageService;

        private MvxSubscriptionToken _updateProfileToken;
        private string _userImageUrl;

        public string UserImageUrl
        {
            get => _userImageUrl;
            set => SetProperty(ref _userImageUrl, value);
        }


        public MvxAsyncCommand ShowContentCommand
        {
            get
            {
                return new MvxAsyncCommand(() => NavigationService.ShowMainViewContent());
            }
        }

        public MvxAsyncCommand ShowNotificationCommand
        {
            get
            {
                return new MvxAsyncCommand(() => NavigationService.ShowNotificationView());
            }
        }

        public MainViewModel(INavigationService navigationService,
                             IMvxMessenger messenger,
                             IStorageService storageService) : base(navigationService)
        {
            _messenger = messenger;
            _storageService = storageService;
        }

        public override void ViewCreated()
        {
            base.ViewCreated();
            Subscription();
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            Unsubscription();
            base.ViewDestroy(viewFinishing);
        }

        private void Subscription()
        {
            _updateProfileToken = _messenger.SubscribeOnMainThread<UpdateUserProfileMessenger>(OnUserProfileUpdate);
        }

        private void Unsubscription()
        {
            if (_updateProfileToken != null)
            {
                _messenger.Unsubscribe<UpdateUserProfileMessenger>(_updateProfileToken);
                _updateProfileToken.Dispose();
            }
        }

        private void OnUserProfileUpdate(UpdateUserProfileMessenger message)
        {
            var user = _storageService.User;
            if (user == null)
                return;
            UserImageUrl = user.Avatar ?? "https://images.pexels.com/photos/2092709/pexels-photo-2092709.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500";
        }
    }
}
