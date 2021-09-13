using MvvmCross.Commands;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.ViewModels.Abstract;
using PrankChat.Mobile.Core.ViewModels.Profile;
using PrankChat.Mobile.Core.ViewModels.Registration;
using PrankChat.Mobile.Core.Providers.UserSession;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.ViewModels.Search.Items
{
    public class ProfileSearchItemViewModel : BaseViewModel
    {
        private readonly IUserSessionProvider _userSessionProvider;

        private readonly User _user;

        public ProfileSearchItemViewModel(IUserSessionProvider userSessionProvider, User user)
        {
            _userSessionProvider = userSessionProvider;
            _user = user;

            OpenUserProfileCommand = this.CreateRestrictedCommand(
                OpenUserProfileAsync,
                restrictedCanExecute: () => _userSessionProvider.User != null,
                handleFunc: NavigationManager.NavigateAsync<LoginViewModel>);
        }

        public string ProfileName => _user.Login;

        public string ProfileShortName => ProfileName.ToShortenName();

        public string ProfileDescription => _user.Description;

        public string ImageUrl => _user.Avatar;

        public IMvxAsyncCommand OpenUserProfileCommand { get; }

        private Task OpenUserProfileAsync()
        {
            if (_user.Id == _userSessionProvider.User.Id ||
                !Connectivity.NetworkAccess.HasConnection())
            {
                return Task.CompletedTask;
            }

            return NavigationManager.NavigateAsync<UserProfileViewModel, int, bool>(_user.Id);
        }
    }
}