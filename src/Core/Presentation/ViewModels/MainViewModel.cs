﻿using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IMvxMessenger _messenger;
        private readonly ISettingsService _settingsService;

        private MvxSubscriptionToken _updateAvatarToken;

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

        public MainViewModel(INavigationService navigationService,
                             IMvxMessenger messenger,
                             ISettingsService settingsService) : base(navigationService)
        {
            _messenger = messenger;
            _settingsService = settingsService;
        }

        public override Task Initialize()
        {
            UpdateUserAvatar();
            return base.Initialize();
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
            _updateAvatarToken = _messenger.Subscribe<UpdateAvatarMessage>(UpdateUserAvatar);
        }

        private void Unsubscription()
        {
            if (_updateAvatarToken != null)
            {
                _messenger.Unsubscribe<UpdateAvatarMessage>(_updateAvatarToken);
                _updateAvatarToken.Dispose();
            }
        }

        private void UpdateUserAvatar(UpdateAvatarMessage message = null)
        {
            UserImageUrl = _settingsService.User?.Avatar;
        }
    }
}
