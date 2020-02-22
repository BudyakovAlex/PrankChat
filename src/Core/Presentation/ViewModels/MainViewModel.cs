using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

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

        public MvxAsyncCommand ShowContentCommand { get; }

        public MvxAsyncCommand ShowLoginCommand { get; }

        public MainViewModel(INavigationService navigationService,
                             IMvxMessenger messenger,
                             ISettingsService settingsService,
                             IErrorHandleService errorHandleService,
                             IApiService apiService,
                             IDialogService dialogService)
            : base(navigationService, errorHandleService, apiService, dialogService)
        {
            _messenger = messenger;
            _settingsService = settingsService;

            ShowContentCommand = new MvxAsyncCommand(NavigationService.ShowMainViewContent);
            ShowLoginCommand = new MvxAsyncCommand(NavigationService.ShowLoginView);
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
