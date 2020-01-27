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

        public MainViewModel(INavigationService navigationService,
                             IMvxMessenger messenger,
                             ISettingsService settingsService) : base(navigationService)
        {
            _messenger = messenger;
            _settingsService = settingsService;
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
            _updateProfileToken = _messenger.SubscribeOnMainThread<UpdateUserProfileMessage>(OnUserProfileUpdate);
        }

        private void Unsubscription()
        {
            if (_updateProfileToken != null)
            {
                _messenger.Unsubscribe<UpdateUserProfileMessage>(_updateProfileToken);
                _updateProfileToken.Dispose();
            }
        }

        private void OnUserProfileUpdate(UpdateUserProfileMessage message)
        {
            var user = _settingsService.User;
            UserImageUrl = user?.Avatar ?? "https://images.pexels.com/photos/2092709/pexels-photo-2092709.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500";
        }
    }
}
