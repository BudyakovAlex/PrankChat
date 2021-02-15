using MvvmCross.Commands;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Providers.UserSession;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Search.Items
{
    public class ProfileSearchItemViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IUserSessionProvider _userSessionProvider;

        private readonly User _user;

        public ProfileSearchItemViewModel(
            INavigationService navigationService,
            IUserSessionProvider userSessionProvider,
            User user)
        {
            _navigationService = navigationService;
            _userSessionProvider = userSessionProvider;
            _user = user;

            OpenUserProfileCommand = new MvxRestrictedAsyncCommand(OpenUserProfileAsync, restrictedCanExecute: () => _userSessionProvider.User != null, handleFunc: _navigationService.ShowLoginView);
        }

        public string ProfileName => _user.Login;

        public string ProfileShortName => ProfileName.ToShortenName();

        public string ProfileDescription => _user.Description;

        public string ImageUrl => _user.Avatar;

        public IMvxAsyncCommand OpenUserProfileCommand { get; }

        private Task OpenUserProfileAsync()
        {
            if (_user.Id == _userSessionProvider.User.Id)
            {
                return Task.CompletedTask;
            }

            return _navigationService.ShowUserProfile(_user.Id);
        }
    }
}